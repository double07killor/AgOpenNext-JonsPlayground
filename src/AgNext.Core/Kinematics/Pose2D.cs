namespace AgNext.Core.Kinematics;

using AgNext.Core.Geometry;

public readonly struct Pose2D
{
    public Vec2 Position { get; }
    public double HeadingRad { get; }

    public Pose2D(Vec2 position, double headingRad)
    {
        Position = position;
        HeadingRad = headingRad;
    }

    public Vec2 Forward => Vec2.FromAngle(HeadingRad);
    public Vec2 Left => Vec2.FromAngle(HeadingRad + (Math.PI / 2));
}
