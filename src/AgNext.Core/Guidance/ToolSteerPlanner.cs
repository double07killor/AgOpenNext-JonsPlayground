namespace AgNext.Core.Guidance;

using AgNext.Core.Geometry;
using AgNext.Core.Kinematics;

public sealed class ToolSteerPlanner
{
    public double ComputeToolAngle(Pose2D implementPose, GuidancePath path)
    {
        if (path.Points.Count < 2)
        {
            return 0;
        }

        var a = path.Points[0];
        var b = path.Points[^1];
        var closest = ProjectPointOnLine(implementPose.Position, a, b);
        var error = (closest - implementPose.Position).Length;
        return MathUtil.Clamp(error * 0.05, -0.25, 0.25);
    }

    private static Vec2 ProjectPointOnLine(Vec2 p, Vec2 a, Vec2 b)
    {
        var ab = b - a;
        var t = ((p.X - a.X) * ab.X + (p.Y - a.Y) * ab.Y) / ((ab.X * ab.X) + (ab.Y * ab.Y));
        t = MathUtil.Clamp(t, 0, 1);
        return new Vec2(a.X + ab.X * t, a.Y + ab.Y * t);
    }
}
