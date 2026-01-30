namespace AgNext.Core.Simulation;

using AgNext.Core.Messages;
using AgNext.Core.Modules;

public sealed class SimulationModule : IAgModule, ITickableModule
{
    private readonly Simulator _simulator;
    private readonly bool _publishSensors;
    private IMessageBus? _bus;
    private ManualControlInput _manualInput = new(0, 0);
    private bool _autoSteerEnabled;
    private double _steerCommandRad;
    private Guidance.GuidanceOutput _guidance = new();
    private AgNext.Core.Kinematics.PoseEstimate? _kinematics;
    private IReadOnlyList<AgNext.Core.Geometry.Vec2> _boundary = Array.Empty<AgNext.Core.Geometry.Vec2>();

    public SimulationModule(Simulator simulator, bool publishSensors = true)
    {
        _simulator = simulator;
        _publishSensors = publishSensors;
    }

    public string Name => "Simulation";
    public Version Version => new(0, 1, 0);
    public ModuleCategory Category => ModuleCategory.Navigation;
    public string[] Dependencies => new[] { "Guidance" };

    public Task InitializeAsync(IModuleContext context)
    {
        _bus = context.MessageBus;
        _bus.Subscribe<ManualControlInput>(input => _manualInput = input);
        _bus.Subscribe<ToggleAutoSteer>(_ => _autoSteerEnabled = !_autoSteerEnabled);
        _bus.Subscribe<SteerCommand>(cmd => _steerCommandRad = cmd.SteerTargetRad);
        _bus.Subscribe<Guidance.GuidanceOutput>(output => _guidance = output);
        _bus.Subscribe<KinematicsState>(state => _kinematics = state.Estimate);
        _bus.Subscribe<BoundaryUpdated>(msg => _boundary = msg.Points);
        return Task.CompletedTask;
    }

    public Task StartAsync() => Task.CompletedTask;
    public Task StopAsync() => Task.CompletedTask;
    public Task ShutdownAsync() => Task.CompletedTask;
    public ModuleHealth GetHealth() => ModuleHealth.Healthy;

    public void Tick()
    {
        if (_bus == null)
        {
            return;
        }

        var steer = _autoSteerEnabled ? _steerCommandRad : _manualInput.SteerRad;
        _simulator.SetControlInput(_manualInput.SpeedMps, steer);
        var snapshot = _simulator.Step(_simulator.StepSeconds);
        var boundary = _boundary.Count > 0 ? _boundary : snapshot.Boundary;
        var publish = new SimulationSnapshot
        {
            SimTime = snapshot.SimTime,
            TimeSeconds = snapshot.TimeSeconds,
            VehicleState = snapshot.VehicleState,
            ImplementPose = snapshot.ImplementPose,
            Boundary = boundary,
            RowCenters = snapshot.RowCenters,
            RowShutoffPoints = snapshot.RowShutoffPoints,
            SectionStates = snapshot.SectionStates,
            PreviewPoints = _guidance.PreviewPoints,
            Swaths = _guidance.Swaths,
            ActiveSwathIndex = _guidance.ActiveSwathIndex,
            AutoSteerEnabled = _autoSteerEnabled,
            PoseEstimate = _kinematics ?? snapshot.PoseEstimate,
            GnssSample = snapshot.GnssSample,
            ImuSample = snapshot.ImuSample,
            WheelSpeedSample = snapshot.WheelSpeedSample,
            SteeringAngleSample = snapshot.SteeringAngleSample
        };
        _bus.Publish(publish);
        _bus.Publish(new AgNext.Core.Messages.SimTick(snapshot.SimTime));

        if (_publishSensors)
        {
            if (snapshot.GnssSample != null) _bus.Publish(snapshot.GnssSample);
            if (snapshot.ImuSample != null) _bus.Publish(snapshot.ImuSample);
            if (snapshot.WheelSpeedSample != null) _bus.Publish(snapshot.WheelSpeedSample);
            if (snapshot.SteeringAngleSample != null) _bus.Publish(snapshot.SteeringAngleSample);
        }
    }
}
