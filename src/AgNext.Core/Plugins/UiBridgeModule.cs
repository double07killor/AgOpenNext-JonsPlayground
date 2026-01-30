namespace AgNext.Core.Plugins;

using AgNext.Core.Messages;
using AgNext.Core.Modules;
using AgNext.Core.Simulation;

public sealed class UiBridgeModule : IAgModule, ITickableModule
{
    private IMessageBus? _bus;
    private SimulationSnapshot _snapshot = new();
    private SectionStateMessage _sectionState = new(Array.Empty<bool>());
    private RateCommandMessage _rateCommand = new(Array.Empty<double>());
    private MonitoringState _monitoring = new(0, 0, "None", 0, 0);
    private PlanterMonitoringState _planter = new(Array.Empty<double>());

    public string Name => "UiBridge";
    public Version Version => new(0, 1, 0);
    public ModuleCategory Category => ModuleCategory.Visualization;
    public string[] Dependencies => new[] { "SectionControl", "RateControl", "Monitoring", "PlanterMonitoring" };

    public Task InitializeAsync(IModuleContext context)
    {
        _bus = context.MessageBus;
        _bus.Subscribe<SimulationSnapshot>(snap => _snapshot = snap);
        _bus.Subscribe<SectionStateMessage>(state => _sectionState = state);
        _bus.Subscribe<RateCommandMessage>(rate => _rateCommand = rate);
        _bus.Subscribe<MonitoringState>(monitor => _monitoring = monitor);
        _bus.Subscribe<PlanterMonitoringState>(planter => _planter = planter);
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

        _bus.Publish(new UiState(_snapshot, _sectionState, _rateCommand, _monitoring, _planter));
    }
}
