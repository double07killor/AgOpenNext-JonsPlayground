namespace AgNext.Core.Simulation;

using AgNext.Core.Geometry;
using AgNext.Core.Guidance;
using AgNext.Core.Kinematics;
using AgNext.Core.Runtime;
using AgNext.Core.Sensors;

public sealed class Simulator
{
    private readonly SimulatorConfig _config;
    private readonly VehicleState _vehicle;
    private readonly AgNext.Core.Kinematics.AxleCentric.AxleCentricModel _axleModel;
    private readonly AgNext.Core.Kinematics.AxleCentric.AxleCentricSolver _axleSolver;
    private readonly SimClock _clock;
    private readonly SimulatedSensors _sensors;

    private double _targetSpeedMps;
    private double _steerCommandRad;
    public double TimeSeconds { get; private set; }

    public Simulator(SimulatorConfig config, VehicleState initialState)
    {
        _config = config;
        _vehicle = initialState;
        _axleModel = AgNext.Core.Kinematics.AxleCentric.AxleCentricModel.FromConfig(config.AxleConfig);
        _axleSolver = new AgNext.Core.Kinematics.AxleCentric.AxleCentricSolver(_axleModel, config.AxleConfig.RootId, config.AxleConfig.SteerAxleId, config.Terrain);
        _axleSolver.SetState(_vehicle.Position, _vehicle.HeadingRad, _vehicle.SpeedMps, _vehicle.SteerAngleRad);
        _clock = new SimClock(config.StepSeconds);
        _sensors = new SimulatedSensors(config.GnssRateHz, config.ImuRateHz, config.WheelSpeedRateHz, config.SteerAngleRateHz);
    }

    public void SetControlInput(double targetSpeedMps, double steerCommandRad)
    {
        _targetSpeedMps = targetSpeedMps;
        _steerCommandRad = steerCommandRad;
    }

    public double StepSeconds => _config.StepSeconds;
    public IReadOnlyList<AgNext.Core.Geometry.Vec2> Boundary => _config.Boundary.Points;
    public double SwathHeadingRad => _config.SwathHeadingRad;
    public double SwathSpacingMeters => _config.SwathSpacingMeters;

    public SimulationSnapshot Step(double dt)
    {
        TimeSeconds += dt;
        var simTime = _clock.Tick();
        _axleSolver.Step(_targetSpeedMps, _steerCommandRad, dt);
        _vehicle.Position = _axleSolver.Position;
        _vehicle.HeadingRad = _axleSolver.HeadingRad;
        _vehicle.SpeedMps = _axleSolver.SpeedMps;
        _vehicle.SteerAngleRad = _axleSolver.SteerAngleRad;

        var gnssPos = new AgNext.Core.Geometry.Vec3(_vehicle.Position.X, _vehicle.Position.Y, 0);
        var gnssTransform = _axleModel.GetAttachmentTransform("gnss-1");
        if (gnssTransform.HasValue)
        {
            gnssPos = gnssTransform.Value.Translation;
        }

        var gnss = _sensors.TryEmitGnss(simTime, dt, gnssPos, _vehicle.SpeedMps, _vehicle.HeadingRad, FixQuality.RtkFixed);
        var yawRate = _axleSolver.YawRateRadPerSec;
        var imu = _sensors.TryEmitImu(simTime, dt, yawRate, _axleSolver.RollRad, _axleSolver.PitchRad);
        var wheel = _sensors.TryEmitWheelSpeed(simTime, dt, _vehicle.SpeedMps);
        var steer = _sensors.TryEmitSteerAngle(simTime, dt, _vehicle.SteerAngleRad);

        var implementPose = _axleModel.GetPose2D("implement-axle");
        var rowCenters = _axleModel.GetRowCenters();
        var rowShutoffPoints = _axleModel.GetRowShutoffPoints();

        return new SimulationSnapshot
        {
            SimTime = simTime,
            TimeSeconds = TimeSeconds,
            VehicleState = new VehicleState
            {
                Position = _vehicle.Position,
                HeadingRad = _vehicle.HeadingRad,
                SpeedMps = _vehicle.SpeedMps,
                SteerAngleRad = _vehicle.SteerAngleRad
            },
            PoseEstimate = new PoseEstimate
            {
                Pose = _vehicle.Pose,
                SpeedMps = _vehicle.SpeedMps,
                FixQuality = FixQuality.None,
                PositionVariance = 1.0,
                HeadingVariance = 0.5,
                RollRad = _axleSolver.RollRad,
                PitchRad = _axleSolver.PitchRad
            },
            ImplementPose = implementPose,
            Boundary = _config.Boundary.Points,
            RowCenters = rowCenters,
            RowShutoffPoints = rowShutoffPoints,
            SectionStates = Array.Empty<bool>(),
            PreviewPoints = Array.Empty<AgNext.Core.Geometry.Vec2>(),
            Swaths = Array.Empty<GuidancePath>(),
            ActiveSwathIndex = 0,
            AutoSteerEnabled = false,
            GnssSample = gnss,
            ImuSample = imu,
            WheelSpeedSample = wheel,
            SteeringAngleSample = steer
        };
    }
}
