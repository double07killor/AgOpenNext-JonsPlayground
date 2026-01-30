namespace AgNext.Core.Guidance;

using AgNext.Core.Geometry;
using AgNext.Core.Kinematics;

public sealed class AutoSteerController : IAutoSteerController
{
    private readonly double _wheelbaseMeters;
    private readonly double _lookaheadMeters;

    public AutoSteerController(double wheelbaseMeters, double lookaheadMeters)
    {
        _wheelbaseMeters = wheelbaseMeters;
        _lookaheadMeters = lookaheadMeters;
    }

    public double ComputeSteerTarget(Pose2D pose, GuidancePath path)
    {
        if (path.Points.Count < 2)
        {
            return 0;
        }

        var a = path.Points[0];
        var b = path.Points[^1];
        var closest = ProjectPointOnLine(pose.Position, a, b);
        var lookaheadPoint = closest + (b - a).Normalized() * _lookaheadMeters;

        var toTarget = lookaheadPoint - pose.Position;
        var targetHeading = Math.Atan2(toTarget.Y, toTarget.X);
        var alpha = MathUtil.WrapAngle(targetHeading - pose.HeadingRad);

        var curvature = (2 * Math.Sin(alpha)) / Math.Max(_lookaheadMeters, 0.1);
        return Math.Atan(curvature * _wheelbaseMeters);
    }

    private static Vec2 ProjectPointOnLine(Vec2 p, Vec2 a, Vec2 b)
    {
        var ab = b - a;
        var t = ((p.X - a.X) * ab.X + (p.Y - a.Y) * ab.Y) / ((ab.X * ab.X) + (ab.Y * ab.Y));
        t = MathUtil.Clamp(t, 0, 1);
        return new Vec2(a.X + ab.X * t, a.Y + ab.Y * t);
    }
}
