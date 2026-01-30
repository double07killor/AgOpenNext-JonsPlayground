namespace AgNext.Core.Kinematics;

using AgNext.Core.Geometry;

public sealed class VehicleState
{
    public Vec2 Position { get; set; }
    public double HeadingRad { get; set; }
    public double SpeedMps { get; set; }
    public double SteerAngleRad { get; set; }

    public Pose2D Pose => new(Position, HeadingRad);
}
