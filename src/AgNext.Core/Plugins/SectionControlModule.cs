namespace AgNext.Core.Plugins;

using AgNext.Core.Guidance;
using AgNext.Core.Messages;
using AgNext.Core.Modules;
using AgNext.Core.Simulation;

public sealed class SectionControlModule : IAgModule, ITickableModule
{
    private readonly SectionLookaheadPlanner _planner = new(leadDistanceMeters: 2.0);
    private IMessageBus? _bus;
    private SimulationSnapshot? _snapshot;
    private IReadOnlyList<AgNext.Core.Geometry.Vec2> _boundary = Array.Empty<AgNext.Core.Geometry.Vec2>();

    public string Name => "SectionControl";
    public Version Version => new(0, 1, 0);
    public ModuleCategory Category => ModuleCategory.Control;
    public string[] Dependencies => new[] { "Simulation" };

    public Task InitializeAsync(IModuleContext context)
    {
        _bus = context.MessageBus;
        _bus.Subscribe<SimulationSnapshot>(snap => _snapshot = snap);
        _bus.Subscribe<BoundaryUpdated>(msg => _boundary = msg.Points);
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

        var boundary = _boundary.Count > 0 ? _boundary : _snapshot.Boundary;
        var states = _planner.ComputeSectionStates(_snapshot.ImplementPose, _snapshot.RowShutoffPoints, new FieldBoundary(boundary));
        _bus.Publish(new SectionStateMessage(states));
    }
}
