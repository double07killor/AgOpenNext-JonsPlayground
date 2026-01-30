namespace AgNext.Core.Kinematics;

using AgNext.Core.Geometry;
using AgNext.Core.Sensors;

public sealed class PoseEstimate
{
    public Pose2D Pose { get; init; }
    public double SpeedMps { get; init; }
    public FixQuality FixQuality { get; init; }
    public double PositionVariance { get; init; }
    public double HeadingVariance { get; init; }
    public double RollRad { get; init; }
    public double PitchRad { get; init; }
}

public sealed class SensorFusion
{
    private Pose2D _pose;
    private double _speedMps;
    private double _rollRad;
    private double _pitchRad;

    public SensorFusion(Pose2D initialPose)
    {
        _pose = initialPose;
    }

    public PoseEstimate Update(GnssSample? gnss, ImuSample? imu, WheelSpeedSample? wheelSpeed, double dt)
    {
        if (imu != null)
        {
            _rollRad = imu.RollRad;
            _pitchRad = imu.PitchRad;
        }

        if (gnss != null)
        {
            _pose = new Pose2D(new Vec2(gnss.Position.X, gnss.Position.Y), gnss.HeadingRad);
            _speedMps = gnss.SpeedMps;
            return new PoseEstimate
            {
                Pose = _pose,
                SpeedMps = _speedMps,
                FixQuality = gnss.Fix,
                PositionVariance = gnss.Fix == FixQuality.RtkFixed ? 0.0004 : 0.09,
                HeadingVariance = gnss.Fix == FixQuality.RtkFixed ? 0.0001 : 0.02,
                RollRad = _rollRad,
                PitchRad = _pitchRad
            };
        }

        if (wheelSpeed != null)
        {
            _speedMps = wheelSpeed.SpeedMps;
        }

        var yawRate = imu?.YawRateRadPerSec ?? 0.0;
        var heading = MathUtil.WrapAngle(_pose.HeadingRad + yawRate * dt);
        var forward = Vec2.FromAngle(heading);
        var position = _pose.Position + forward * (_speedMps * dt);
        _pose = new Pose2D(position, heading);

        return new PoseEstimate
        {
            Pose = _pose,
            SpeedMps = _speedMps,
            FixQuality = FixQuality.None,
            PositionVariance = 1.0,
            HeadingVariance = 0.5,
            RollRad = _rollRad,
            PitchRad = _pitchRad
        };
    }
}
