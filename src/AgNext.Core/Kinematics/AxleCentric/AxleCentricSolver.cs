namespace AgNext.Core.Kinematics.AxleCentric;

using AgNext.Core.Geometry;
using AgNext.Core.Kinematics;

public sealed class AxleCentricSolver
{
    private readonly AxleCentricModel _model;
    private readonly ITerrainModel _terrain;
    private readonly string _rootId;
    private readonly string _steerAxleId;
    private readonly double _wheelbaseMeters;
    private readonly double _steerAxleOffsetX;
    private readonly double _maxSteerRad;
    private readonly double _maxSteerRateRadPerSec;
    private readonly double _speedResponse;
    private readonly List<AxleFrame> _rigidAxles;
    private readonly HashSet<string> _rigidAxleIds;
    private readonly bool _hasPassiveSteerAxles;
    private readonly Dictionary<string, double> _drawbarYaw = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, double> _axleYaw = new(StringComparer.OrdinalIgnoreCase);

    private Vec2 _position;
    private double _headingRad;
    private double _speedMps;
    private double _steerRad;
    private double _rollRad;
    private double _pitchRad;
    private double _yawRateRadPerSec;

    public AxleCentricSolver(AxleCentricModel model, string rootId, string steerAxleId, ITerrainModel? terrain = null, double maxSteerRateRadPerSec = 1.5, double speedResponse = 2.5)
    {
        _model = model;
        _terrain = terrain ?? new FlatTerrainModel();
        _rootId = rootId;
        _steerAxleId = steerAxleId;
        _maxSteerRateRadPerSec = maxSteerRateRadPerSec;
        _speedResponse = speedResponse;

        _wheelbaseMeters = ResolveWheelbaseMeters(model, rootId, steerAxleId);
        _maxSteerRad = ResolveMaxSteer(model, steerAxleId);
        _rigidAxles = BuildRigidAxles(model, rootId);
        _rigidAxleIds = new HashSet<string>(_rigidAxles.Select(axle => axle.Id), StringComparer.OrdinalIgnoreCase);
        _steerAxleOffsetX = ResolveSteerAxleOffsetX(_rigidAxles, steerAxleId, _wheelbaseMeters);
        _hasPassiveSteerAxles = HasPassiveSteerAxles(_rigidAxles, steerAxleId);
    }

    public Vec2 Position => _position;
    public double HeadingRad => _headingRad;
    public double RollRad => _rollRad;
    public double PitchRad => _pitchRad;
    public double SpeedMps => _speedMps;
    public double SteerAngleRad => _steerRad;
    public double YawRateRadPerSec => _yawRateRadPerSec;

    public void SetState(Vec2 position, double headingRad, double speedMps, double steerAngleRad)
    {
        _position = position;
        _headingRad = headingRad;
        _speedMps = speedMps;
        _steerRad = steerAngleRad;
    }

    public void Step(double targetSpeedMps, double targetSteerRad, double dt)
    {
        targetSteerRad = MathUtil.Clamp(targetSteerRad, -_maxSteerRad, _maxSteerRad);
        var steerDelta = MathUtil.Clamp(targetSteerRad - _steerRad, -_maxSteerRateRadPerSec * dt, _maxSteerRateRadPerSec * dt);
        _steerRad = MathUtil.Clamp(_steerRad + steerDelta, -_maxSteerRad, _maxSteerRad);

        var accel = (targetSpeedMps - _speedMps) * _speedResponse;
        _speedMps += accel * dt;

        var steerAngles = ComputeSteerAngles(_steerRad);
        var twist = SolveTwist(_speedMps, steerAngles);
        if (_hasPassiveSteerAxles)
        {
            steerAngles = ComputeSteerAngles(_steerRad, twist);
            twist = SolveTwist(_speedMps, steerAngles);
        }
        _yawRateRadPerSec = twist.Omega;
        _headingRad = MathUtil.WrapAngle(_headingRad + _yawRateRadPerSec * dt);
        var worldVel = BodyToWorld(new Vec2(twist.VelocityForward, twist.VelocityLeft));
        _position += worldVel * dt;
        _speedMps = Math.Sqrt((twist.VelocityForward * twist.VelocityForward) + (twist.VelocityLeft * twist.VelocityLeft));

        var height = _terrain.GetHeight(_position);
        var normal = _terrain.GetNormal(_position);
        (_rollRad, _pitchRad) = ComputeRollPitchFromNormal(normal, _headingRad);
        var rootRotation = Quaternion.FromYawPitchRoll(_headingRad, _pitchRad, _rollRad);
        _model.SetRootPose(_rootId, new Vec3(_position.X, _position.Y, height), rootRotation);
        ApplySteerToModel(steerAngles);
        _model.Recalculate();

        UpdateDrawbars(twist, dt);
        _model.Recalculate();
    }

    private Dictionary<string, AxleSteer> ComputeSteerAngles(double steerRad, (double VelocityForward, double VelocityLeft, double Omega)? twist = null)
    {
        var angles = new Dictionary<string, AxleSteer>(StringComparer.OrdinalIgnoreCase);
        var curvature = ComputeCurvature(steerRad);
        var hasTwist = false;
        var baseVelocity = new Vec2(0, 0);
        var omega = 0.0;
        if (twist is { } solved)
        {
            hasTwist = true;
            baseVelocity = new Vec2(solved.VelocityForward, solved.VelocityLeft);
            omega = solved.Omega;
        }

        foreach (var axle in _rigidAxles)
        {
            if (!axle.IsSteered)
            {
                angles[axle.Id] = new AxleSteer(0, 0, 0);
                continue;
            }

            var usePassive = axle.IsDriven == false && !string.Equals(axle.Id, _steerAxleId, StringComparison.OrdinalIgnoreCase);
            double center;
            if (usePassive && hasTwist)
            {
                var bodyVelocity = GetBodyVelocityAtPoint(baseVelocity, omega, axle.Offset);
                if (bodyVelocity.Length < 1e-4)
                {
                    center = 0.0;
                }
                else
                {
                    var velocityHeading = Math.Atan2(bodyVelocity.Y, bodyVelocity.X);
                    center = MathUtil.WrapAngle(velocityHeading - axle.BaseYawRad);
                }
            }
            else if (usePassive)
            {
                center = 0.0;
            }
            else
            {
                center = Math.Atan(curvature * axle.Offset.X);
            }

            var maxSteer = axle.MaxSteerRad;
            center = MathUtil.Clamp(center, -maxSteer, maxSteer);
            var (left, right) = ComputeAckermann(center, axle.Offset.X, axle.TrackWidthMeters);
            left = MathUtil.Clamp(left, -maxSteer, maxSteer);
            right = MathUtil.Clamp(right, -maxSteer, maxSteer);
            angles[axle.Id] = new AxleSteer(center, left, right);
        }

        return angles;
    }

    private double ComputeCurvature(double steerRad)
    {
        if (Math.Abs(steerRad) < 1e-6)
        {
            return 0.0;
        }

        var offset = Math.Abs(_steerAxleOffsetX);
        if (offset < 1e-3)
        {
            offset = _wheelbaseMeters;
        }

        return offset > 1e-3 ? Math.Tan(steerRad) / offset : 0.0;
    }

    private void ApplySteerToModel(IReadOnlyDictionary<string, AxleSteer> steerAngles)
    {
        var appliedPrimary = false;

        foreach (var axle in _rigidAxles)
        {
            if (!axle.IsSteered)
            {
                continue;
            }

            if (steerAngles.TryGetValue(axle.Id, out var steer))
            {
                _model.SetLocalYaw(axle.Id, steer.Center);
            }

            if (string.Equals(axle.Id, _steerAxleId, StringComparison.OrdinalIgnoreCase))
            {
                appliedPrimary = true;
            }
        }

        if (!appliedPrimary && _model.TryGetNode(_steerAxleId, out var steerNode) && steerNode.Axle != null && steerNode.Axle.IsSteered)
        {
            _model.SetLocalYaw(_steerAxleId, _steerRad);
        }
    }

    private Vec2 BodyToWorld(Vec2 bodyVelocity)
    {
        var forward = Vec2.FromAngle(_headingRad);
        var left = Vec2.FromAngle(_headingRad + Math.PI / 2);
        return (forward * bodyVelocity.X) + (left * bodyVelocity.Y);
    }

    private static Vec2 GetBodyVelocityAtPoint(Vec2 baseVelocity, double omega, Vec2 offset)
    {
        var angular = new Vec2(-omega * offset.Y, omega * offset.X);
        return baseVelocity + angular;
    }

    private void UpdateDrawbars((double VelocityForward, double VelocityLeft, double Omega) twist, double dt)
    {
        var rootWorld = _model.GetWorldTransform(_rootId);
        var rootInv = rootWorld.Inverse();
        var baseVelocity = new Vec2(twist.VelocityForward, twist.VelocityLeft);
        var rootYaw = rootWorld.Rotation.ToYaw();
        var rootWorldVelocity = BodyToWorld(baseVelocity);

        var frameTwists = new Dictionary<string, FrameTwist>(StringComparer.OrdinalIgnoreCase)
        {
            [_rootId] = new FrameTwist(rootWorldVelocity, twist.Omega, rootYaw)
        };
        var updatedWorld = new Dictionary<string, Transform3>(StringComparer.OrdinalIgnoreCase)
        {
            [_rootId] = rootWorld
        };

        foreach (var node in GetNonRigidNodesByDepth())
        {
            var parentId = node.ParentId ?? _rootId;
            var parentWorld = GetWorld(parentId);
            var parentTwist = GetTwist(parentId, parentWorld);
            var parentYaw = parentTwist.Yaw;
            var pivotOffsetWorld = parentWorld.Rotation.Rotate(node.BaseLocal.Translation);
            var pivotVelocity = parentTwist.WorldVelocity + new Vec2(-parentTwist.Omega * pivotOffsetWorld.Y, parentTwist.Omega * pivotOffsetWorld.X);

            if (node.Drawbar != null)
            {
                var length = Math.Max(0.1, node.Drawbar.LengthMeters);

                if (!_drawbarYaw.TryGetValue(node.Id, out var drawbarYaw))
                {
                    drawbarYaw = parentYaw;
                }

                var parentForward = Vec2.FromAngle(parentYaw);
                var parentForwardSpeed = (pivotVelocity.X * parentForward.X) + (pivotVelocity.Y * parentForward.Y);

                var previousYaw = drawbarYaw;
                var yawRate = (parentForwardSpeed / length) * Math.Sin(parentYaw - drawbarYaw);
                var proposedYaw = MathUtil.WrapAngle(drawbarYaw + yawRate * dt);
                var relativeYaw = MathUtil.WrapAngle(proposedYaw - parentYaw);
                relativeYaw = MathUtil.Clamp(relativeYaw, node.Drawbar.Limits.YawMinRad, node.Drawbar.Limits.YawMaxRad);
                drawbarYaw = MathUtil.WrapAngle(parentYaw + relativeYaw);

                var pivotWorld = parentWorld.Translation + pivotOffsetWorld;
                var worldNormal = _terrain.GetNormal(new Vec2(pivotWorld.X, pivotWorld.Y));
                var parentInvRot = parentWorld.Rotation.Conjugate().Normalize();
                var localNormal = parentInvRot.Rotate(worldNormal);
                var (drawbarRoll, drawbarPitch) = ComputeRollPitchFromNormal(localNormal, relativeYaw);
                drawbarPitch = MathUtil.Clamp(drawbarPitch, node.Drawbar.Limits.PitchMinRad, node.Drawbar.Limits.PitchMaxRad);
                drawbarRoll = MathUtil.Clamp(drawbarRoll, node.Drawbar.Limits.RollMinRad, node.Drawbar.Limits.RollMaxRad);

                var appliedYawRate = MathUtil.WrapAngle(drawbarYaw - previousYaw) / Math.Max(dt, 1e-6);
                var drawbarForward = Vec2.FromAngle(drawbarYaw);
                var drawbarForwardSpeed = (pivotVelocity.X * drawbarForward.X) + (pivotVelocity.Y * drawbarForward.Y);

                var localRotation = node.BaseLocal.Rotation * Quaternion.FromYawPitchRoll(relativeYaw, drawbarPitch, drawbarRoll);
                node.Local = new Transform3(node.BaseLocal.Translation, localRotation);
                _drawbarYaw[node.Id] = drawbarYaw;
                frameTwists[node.Id] = new FrameTwist(drawbarForward * drawbarForwardSpeed, appliedYawRate, drawbarYaw);
                updatedWorld[node.Id] = parentWorld.Combine(node.Local);
                continue;
            }

            if (node.Axle == null)
            {
                continue;
            }

            var baseYaw = node.BaseLocal.Rotation.ToYaw();
            var localYaw = 0.0;
            if (node.Axle.IsSteered)
            {
                if (pivotVelocity.Length >= 1e-4)
                {
                    var desiredYaw = Math.Atan2(pivotVelocity.Y, pivotVelocity.X);
                    localYaw = MathUtil.WrapAngle(desiredYaw - parentYaw - baseYaw);
                }
                localYaw = MathUtil.Clamp(localYaw, -Math.Abs(node.Axle.MaxSteerAngleRad), Math.Abs(node.Axle.MaxSteerAngleRad));
            }

            _model.SetLocalYaw(node.Id, localYaw);

            var axleYaw = MathUtil.WrapAngle(parentYaw + baseYaw + localYaw);
            if (!_axleYaw.TryGetValue(node.Id, out var previousAxleYaw))
            {
                previousAxleYaw = axleYaw;
            }
            var axleYawRate = MathUtil.WrapAngle(axleYaw - previousAxleYaw) / Math.Max(dt, 1e-6);
            _axleYaw[node.Id] = axleYaw;

            var axleForward = Vec2.FromAngle(axleYaw);
            var axleForwardSpeed = (pivotVelocity.X * axleForward.X) + (pivotVelocity.Y * axleForward.Y);
            frameTwists[node.Id] = new FrameTwist(axleForward * axleForwardSpeed, axleYawRate, axleYaw);

            updatedWorld[node.Id] = parentWorld.Combine(node.Local);
        }

        Transform3 GetWorld(string id)
        {
            if (updatedWorld.TryGetValue(id, out var world))
            {
                return world;
            }

            return _model.GetWorldTransform(id);
        }

        FrameTwist GetTwist(string id, Transform3 world)
        {
            if (frameTwists.TryGetValue(id, out var cached))
            {
                return cached;
            }

            var offsetRoot = rootInv.TransformPoint(world.Translation);
            var offset = new Vec2(offsetRoot.X, offsetRoot.Y);
            var bodyVelocity = GetBodyVelocityAtPoint(baseVelocity, twist.Omega, offset);
            var worldVelocity = BodyToWorld(bodyVelocity);
            var yaw = world.Rotation.ToYaw();
            var computed = new FrameTwist(worldVelocity, twist.Omega, yaw);
            frameTwists[id] = computed;
            return computed;
        }
    }

    private (double VelocityForward, double VelocityLeft, double Omega) SolveTwist(double driveSpeed, IReadOnlyDictionary<string, AxleSteer> steerAngles)
    {
        if (_rigidAxles.Count == 0)
        {
            return (0, 0, 0);
        }

        var constraints = new List<WheelConstraint>(_rigidAxles.Count * 2);
        var hasDrive = false;

        foreach (var axle in _rigidAxles)
        {
            var halfTrack = axle.TrackWidthMeters * 0.5;
            var axleLeft = Vec2.FromAngle(axle.BaseYawRad + Math.PI / 2);
            var leftPos = axle.Offset + (axleLeft * halfTrack);
            var rightPos = axle.Offset - (axleLeft * halfTrack);

            steerAngles.TryGetValue(axle.Id, out var steer);
            var leftHeading = Vec2.FromAngle(axle.BaseYawRad + steer.Left);
            var rightHeading = Vec2.FromAngle(axle.BaseYawRad + steer.Right);

            if (axle.IsDriven)
            {
                hasDrive = true;
            }
            var drive = axle.IsDriven ? driveSpeed : (double?)null;
            constraints.Add(new WheelConstraint(leftPos, leftHeading, drive));
            constraints.Add(new WheelConstraint(rightPos, rightHeading, drive));
        }

        if (!hasDrive)
        {
            constraints.Add(new WheelConstraint(new Vec2(0, 0), Vec2.FromAngle(0), driveSpeed));
        }

        return SolveLeastSquares(constraints);
    }

    private static (double Left, double Right) ComputeAckermann(double steerRad, double axleOffsetMeters, double trackMeters)
    {
        var wheelbaseMeters = Math.Abs(axleOffsetMeters);
        if (Math.Abs(steerRad) < 1e-4 || wheelbaseMeters < 0.1)
        {
            return (steerRad, steerRad);
        }

        var radius = wheelbaseMeters / Math.Tan(steerRad);
        var left = Math.Atan(wheelbaseMeters / (radius - trackMeters * 0.5));
        var right = Math.Atan(wheelbaseMeters / (radius + trackMeters * 0.5));
        return (left, right);
    }

    private static (double VelocityForward, double VelocityLeft, double Omega) SolveLeastSquares(IReadOnlyList<WheelConstraint> constraints)
    {
        if (constraints.Count == 0)
        {
            return (0, 0, 0);
        }

        var ata = new double[3, 3];
        var atb = new double[3];

        foreach (var c in constraints)
        {
            var t = c.Heading;
            var l = new Vec2(-t.Y, t.X);
            AddRow(l, c.Position, 0.0);

            if (c.DriveSpeed.HasValue)
            {
                AddRow(t, c.Position, c.DriveSpeed.Value);
            }
        }

        ata[1, 0] = ata[0, 1];
        ata[2, 0] = ata[0, 2];
        ata[2, 1] = ata[1, 2];

        if (!Solve3x3(ata, atb, out var x))
        {
            return (0, 0, 0);
        }

        return (x[0], x[1], x[2]);

        void AddRow(Vec2 dir, Vec2 pos, double value)
        {
            var a = dir.X;
            var b = dir.Y;
            var c = (-dir.X * pos.Y) + (dir.Y * pos.X);

            ata[0, 0] += a * a;
            ata[0, 1] += a * b;
            ata[0, 2] += a * c;
            ata[1, 1] += b * b;
            ata[1, 2] += b * c;
            ata[2, 2] += c * c;

            atb[0] += a * value;
            atb[1] += b * value;
            atb[2] += c * value;
        }
    }

    private static bool Solve3x3(double[,] ata, double[] atb, out double[] x)
    {
        x = new double[3];
        var m = new double[3, 4];
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                m[i, j] = ata[i, j];
            }
            m[i, 3] = atb[i];
        }

        for (var k = 0; k < 3; k++)
        {
            var pivot = k;
            for (var i = k + 1; i < 3; i++)
            {
                if (Math.Abs(m[i, k]) > Math.Abs(m[pivot, k]))
                {
                    pivot = i;
                }
            }

            if (Math.Abs(m[pivot, k]) < 1e-8)
            {
                return false;
            }

            if (pivot != k)
            {
                for (var j = k; j < 4; j++)
                {
                    (m[k, j], m[pivot, j]) = (m[pivot, j], m[k, j]);
                }
            }

            var inv = 1.0 / m[k, k];
            for (var j = k; j < 4; j++)
            {
                m[k, j] *= inv;
            }

            for (var i = 0; i < 3; i++)
            {
                if (i == k) continue;
                var factor = m[i, k];
                for (var j = k; j < 4; j++)
                {
                    m[i, j] -= factor * m[k, j];
                }
            }
        }

        x[0] = m[0, 3];
        x[1] = m[1, 3];
        x[2] = m[2, 3];
        return true;
    }

    private static List<AxleFrame> BuildRigidAxles(AxleCentricModel model, string rootId)
    {
        var baseWorld = BuildBaseWorldTransforms(model);
        if (!baseWorld.TryGetValue(rootId, out var rootWorld))
        {
            rootWorld = Transform3.Identity;
        }

        var rootInv = rootWorld.Inverse();
        var axles = new List<AxleFrame>();

        foreach (var node in model.Nodes)
        {
            if (node.Axle == null)
            {
                continue;
            }

            if (!IsRigidToRoot(model, node, rootId))
            {
                continue;
            }

            if (!baseWorld.TryGetValue(node.Id, out var axleWorld))
            {
                continue;
            }

            var relative = rootInv.Combine(axleWorld);
            var offset = new Vec2(relative.Translation.X, relative.Translation.Y);
            var baseYaw = relative.Rotation.ToYaw();
            var trackWidth = Math.Max(0.1, node.Axle.TrackWidthMeters);
            var maxSteer = Math.Abs(node.Axle.MaxSteerAngleRad);
            axles.Add(new AxleFrame(node.Id, offset, baseYaw, trackWidth, maxSteer, node.Axle.IsDriven, node.Axle.IsSteered));
        }

        return axles;
    }

    private static bool HasPassiveSteerAxles(IReadOnlyList<AxleFrame> axles, string steerAxleId)
    {
        foreach (var axle in axles)
        {
            if (!axle.IsSteered || axle.IsDriven)
            {
                continue;
            }

            if (!string.Equals(axle.Id, steerAxleId, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    private static Dictionary<string, Transform3> BuildBaseWorldTransforms(AxleCentricModel model)
    {
        var baseWorld = new Dictionary<string, Transform3>(StringComparer.OrdinalIgnoreCase);
        foreach (var node in model.Nodes)
        {
            Build(node);
        }
        return baseWorld;

        Transform3 Build(FrameNode node)
        {
            if (baseWorld.TryGetValue(node.Id, out var cached))
            {
                return cached;
            }

            Transform3 world;
            if (node.ParentId != null && model.TryGetNode(node.ParentId, out var parent))
            {
                world = Build(parent).Combine(node.BaseLocal);
            }
            else
            {
                world = node.BaseLocal;
            }

            baseWorld[node.Id] = world;
            return world;
        }
    }

    private static bool IsRigidToRoot(AxleCentricModel model, FrameNode node, string rootId)
    {
        if (string.Equals(node.Id, rootId, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        var current = node;
        while (current.ParentId != null && model.TryGetNode(current.ParentId, out var parent))
        {
            if (parent.Drawbar != null)
            {
                return false;
            }

            if (string.Equals(parent.Id, rootId, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            current = parent;
        }

        return false;
    }

    private static double ResolveSteerAxleOffsetX(IReadOnlyList<AxleFrame> axles, string steerAxleId, double fallback)
    {
        foreach (var axle in axles)
        {
            if (string.Equals(axle.Id, steerAxleId, StringComparison.OrdinalIgnoreCase))
            {
                return axle.Offset.X;
            }
        }

        return fallback;
    }

    private IReadOnlyList<FrameNode> GetNonRigidNodesByDepth()
    {
        var nodes = _model.Nodes
            .Where(node => node.Drawbar != null || (node.Axle != null && !_rigidAxleIds.Contains(node.Id)))
            .ToArray();
        var depthCache = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        Array.Sort(nodes, (left, right) =>
        {
            var leftDepth = GetDepth(left);
            var rightDepth = GetDepth(right);
            var diff = leftDepth.CompareTo(rightDepth);
            if (diff != 0)
            {
                return diff;
            }

            return StringComparer.OrdinalIgnoreCase.Compare(left.Id, right.Id);
        });

        return nodes;

        int GetDepth(FrameNode node)
        {
            if (depthCache.TryGetValue(node.Id, out var cached))
            {
                return cached;
            }

            var depth = 0;
            var current = node;
            while (current.ParentId != null && _model.TryGetNode(current.ParentId, out var parent))
            {
                depth++;
                current = parent;
            }

            depthCache[node.Id] = depth;
            return depth;
        }
    }

    private static double ResolveWheelbaseMeters(AxleCentricModel model, string rootId, string steerAxleId)
    {
        model.SetRootPose(rootId, new Vec3(0, 0, 0), 0);
        var root = model.GetWorldTransform(rootId).Translation;
        var steer = model.GetWorldTransform(steerAxleId).Translation;
        var dx = steer.X - root.X;
        var dy = steer.Y - root.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }

    private static double ResolveMaxSteer(AxleCentricModel model, string steerAxleId)
    {
        if (model.TryGetNode(steerAxleId, out var node) && node.Axle != null)
        {
            return Math.Abs(node.Axle.MaxSteerAngleRad);
        }
        return 0.6;
    }

    private static (double RollRad, double PitchRad) ComputeRollPitchFromNormal(Vec3 normal, double yawRad)
    {
        var n = normal.Normalized();
        var cy = Math.Cos(-yawRad);
        var sy = Math.Sin(-yawRad);
        var nx = (n.X * cy) - (n.Y * sy);
        var ny = (n.X * sy) + (n.Y * cy);
        var nz = n.Z;

        var roll = -Math.Asin(MathUtil.Clamp(ny, -1, 1));
        var pitch = Math.Atan2(nx, nz);
        return (roll, pitch);
    }

    private sealed record AxleFrame(
        string Id,
        Vec2 Offset,
        double BaseYawRad,
        double TrackWidthMeters,
        double MaxSteerRad,
        bool IsDriven,
        bool IsSteered);

    private readonly record struct AxleSteer(double Center, double Left, double Right);
    private readonly record struct FrameTwist(Vec2 WorldVelocity, double Omega, double Yaw);

    private sealed record WheelConstraint(Vec2 Position, Vec2 Heading, double? DriveSpeed);
}
