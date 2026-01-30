namespace AgNext.Core.Simulation;

using AgNext.Core.Geometry;
using AgNext.Core.Runtime;
using AgNext.Core.Sensors;

public sealed class SimulatedSensors
{
    private readonly double _gnssInterval;
    private readonly double _imuInterval;
    private readonly double _wheelInterval;
    private readonly double _steerInterval;
    private double _gnssAccumulator;
    private double _imuAccumulator;
    private double _wheelAccumulator;
    private double _steerAccumulator;

    public SimulatedSensors(double gnssRateHz, double imuRateHz, double wheelRateHz, double steerRateHz)
    {
        _gnssInterval = 1.0 / gnssRateHz;
        _imuInterval = 1.0 / imuRateHz;
        _wheelInterval = 1.0 / wheelRateHz;
        _steerInterval = 1.0 / steerRateHz;
    }

    public GnssSample? TryEmitGnss(SimTime simTime, double dt, Vec3 position, double speedMps, double headingRad, FixQuality fix)
    {
        _gnssAccumulator += dt;
        if (_gnssAccumulator < _gnssInterval)
        {
            return null;
        }
        _gnssAccumulator -= _gnssInterval;
        return new GnssSample(simTime, position, speedMps, headingRad, fix);
    }

    public ImuSample? TryEmitImu(SimTime simTime, double dt, double yawRateRadPerSec, double rollRad, double pitchRad)
    {
        _imuAccumulator += dt;
        if (_imuAccumulator < _imuInterval)
        {
            return null;
        }
        _imuAccumulator -= _imuInterval;
        return new ImuSample(simTime, yawRateRadPerSec, rollRad, pitchRad);
    }

    public WheelSpeedSample? TryEmitWheelSpeed(SimTime simTime, double dt, double speedMps)
    {
        _wheelAccumulator += dt;
        if (_wheelAccumulator < _wheelInterval)
        {
            return null;
        }
        _wheelAccumulator -= _wheelInterval;
        return new WheelSpeedSample(simTime, speedMps);
    }

    public SteeringAngleSample? TryEmitSteerAngle(SimTime simTime, double dt, double steerAngleRad)
    {
        _steerAccumulator += dt;
        if (_steerAccumulator < _steerInterval)
        {
            return null;
        }
        _steerAccumulator -= _steerInterval;
        return new SteeringAngleSample(simTime, steerAngleRad);
    }
}
