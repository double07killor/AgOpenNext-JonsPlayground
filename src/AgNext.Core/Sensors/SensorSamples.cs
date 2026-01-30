namespace AgNext.Core.Sensors;

using AgNext.Core.Runtime;
using AgNext.Core.Geometry;

public enum FixQuality
{
    None,
    Standalone,
    Dgps,
    RtkFloat,
    RtkFixed
}

public sealed record GnssSample(
    SimTime Stamp,
    Vec3 Position,
    double SpeedMps,
    double HeadingRad,
    FixQuality Fix);

public sealed record ImuSample(
    SimTime Stamp,
    double YawRateRadPerSec,
    double RollRad,
    double PitchRad);

public sealed record WheelSpeedSample(
    SimTime Stamp,
    double SpeedMps);

public sealed record SteeringAngleSample(
    SimTime Stamp,
    double SteerAngleRad);
