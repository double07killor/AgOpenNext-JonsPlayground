namespace AgNext.Core.Plugins;

using AgNext.Core.Messages;
using AgNext.Core.Modules;
using AgNext.Core.Simulation;

public sealed class MonitoringModule : IAgModule, ITickableModule
{
    private IMessageBus? _bus;
    private SimulationSnapshot? _snapshot;
    private AgNext.Core.Kinematics.PoseEstimate? _pose;

    public string Name => "Monitoring";
    public Version Version => new(0, 1, 0);
    public ModuleCategory Category => ModuleCategory.Monitoring;
    public string[] Dependencies => new[] { "Simulation", "Kinematics" };

    public Task InitializeAsync(IModuleContext context)
    {
        _bus = context.MessageBus;
        _bus.Subscribe<SimulationSnapshot>(snap => _snapshot = snap);
        _bus.Subscribe<KinematicsState>(state => _pose = state.Estimate);
        return Task.CompletedTask;
    }

    public Task StartAsync() => Task.CompletedTask;
    public Task StopAsync() => Task.CompletedTask;
    public Task ShutdownAsync() => Task.CompletedTask;
    public ModuleHealth GetHealth() => ModuleHealth.Healthy;

    public void Tick()
    {
        if (_bus == null || _snapshot == null)
        {
            return;
        }

        var estimate = _pose ?? _snapshot.PoseEstimate;
        var state = new MonitoringState(
            SpeedMps: _snapshot.VehicleState.SpeedMps,
            HeadingDeg: _snapshot.VehicleState.HeadingRad * 180 / Math.PI,
            FixQuality: estimate.FixQuality.ToString(),
            PositionVariance: estimate.PositionVariance,
            HeadingVariance: estimate.HeadingVariance);
        _bus.Publish(state);
    }
}
