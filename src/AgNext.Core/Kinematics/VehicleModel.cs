namespace AgNext.Core.Kinematics;

using AgNext.Core.Geometry;

public sealed class VehicleModel
{
    private readonly VehicleConfig _config;

    public VehicleModel(VehicleConfig config)
    {
        _config = config;
    }

    public void Step(VehicleState state, double targetSteerRad, double targetSpeedMps, double dt)
    {
        targetSpeedMps = MathUtil.Clamp(targetSpeedMps, -_config.MaxSpeedMps, _config.MaxSpeedMps);
        var steerRate = _config.MaxSteerRateRadPerSec;
        var steerDelta = MathUtil.Clamp(targetSteerRad - state.SteerAngleRad, -steerRate * dt, steerRate * dt);
        state.SteerAngleRad = MathUtil.Clamp(state.SteerAngleRad + steerDelta, -_config.MaxSteerAngleRad, _config.MaxSteerAngleRad);

        var accel = (targetSpeedMps - state.SpeedMps) * 2.5;
        state.SpeedMps += accel * dt;

        var yawRate = state.SpeedMps / _config.WheelbaseMeters * Math.Tan(state.SteerAngleRad);
        state.HeadingRad = MathUtil.WrapAngle(state.HeadingRad + yawRate * dt);

        var forward = Vec2.FromAngle(state.HeadingRad);
        state.Position += forward * (state.SpeedMps * dt);
    }
}
