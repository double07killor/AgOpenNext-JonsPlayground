namespace AgNext.Core.Kinematics;

public sealed record VehicleConfig(
    double WheelbaseMeters,
    double MaxSteerAngleRad,
    double MaxSpeedMps,
    double MaxSteerRateRadPerSec);
