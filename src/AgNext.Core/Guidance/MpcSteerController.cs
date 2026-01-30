namespace AgNext.Core.Guidance;

using AgNext.Core.Geometry;
using AgNext.Core.Kinematics;

public sealed class MpcSteerController : IAutoSteerController
{
    private readonly AutoSteerController _fallback;
    private readonly double _wheelbaseMeters;
    private double _speedMps = 4.0;
    private readonly double _maxSteerRad = 0.6;
    private readonly int _horizonSteps = 15;
    private readonly double _stepSeconds = 0.1;

    public MpcSteerController(double wheelbaseMeters, double lookaheadMeters)
    {
        _fallback = new AutoSteerController(wheelbaseMeters, lookaheadMeters);
        _wheelbaseMeters = wheelbaseMeters;
    }

    public double ComputeSteerTarget(Pose2D pose, GuidancePath path)
    {
        if (path.Points.Count < 2)
        {
            return 0;
        }

        var candidates = 11;
        var bestSteer = 0.0;
        var bestCost = double.MaxValue;

        for (var i = 0; i < candidates; i++)
        {
            var steer = -_maxSteerRad + (2 * _maxSteerRad * i / (candidates - 1));
            var cost = EvaluateCost(pose, path, steer);
            if (cost < bestCost)
            {
                bestCost = cost;
                bestSteer = steer;
            }
        }

        return bestSteer;
    }

    public void SetSpeed(double speedMps)
    {
        _speedMps = speedMps;
    }

    private double EvaluateCost(Pose2D pose, GuidancePath path, double steerRad)
    {
        var a = path.Points[0];
        var b = path.Points[^1];
        var heading = pose.HeadingRad;
        var position = pose.Position;
        var cost = 0.0;

        for (var step = 0; step < _horizonSteps; step++)
        {
            var yawRate = _speedMps / _wheelbaseMeters * Math.Tan(steerRad);
            heading = MathUtil.WrapAngle(heading + yawRate * _stepSeconds);
            position += Vec2.FromAngle(heading) * (_speedMps * _stepSeconds);

            var closest = ProjectPointOnLine(position, a, b);
            var error = (position - closest).Length;
            var headingError = Math.Abs(MathUtil.WrapAngle(Math.Atan2(b.Y - a.Y, b.X - a.X) - heading));
            cost += (error * error) + (headingError * headingError * 0.5);
        }

        return cost;
    }

    private static Vec2 ProjectPointOnLine(Vec2 p, Vec2 a, Vec2 b)
    {
        var ab = b - a;
        var t = ((p.X - a.X) * ab.X + (p.Y - a.Y) * ab.Y) / ((ab.X * ab.X) + (ab.Y * ab.Y));
        t = MathUtil.Clamp(t, 0, 1);
        return new Vec2(a.X + ab.X * t, a.Y + ab.Y * t);
    }
}
