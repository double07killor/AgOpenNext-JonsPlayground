namespace AgNext.Core.Plugins;

using AgNext.Core.Messages;
using AgNext.Core.Modules;
using AgNext.Core.Simulation;

public sealed class PlanterMonitoringModule : IAgModule, ITickableModule
{
    private IMessageBus? _bus;
    private bool[] _sections = Array.Empty<bool>();
    private double[] _population = Array.Empty<double>();
    private double _speedMps;

    public string Name => "PlanterMonitoring";
    public Version Version => new(0, 1, 0);
    public ModuleCategory Category => ModuleCategory.Monitoring;
    public string[] Dependencies => new[] { "SectionControl" };

    public Task InitializeAsync(IModuleContext context)
    {
        _bus = context.MessageBus;
        _bus.Subscribe<SectionStateMessage>(state =>
        {
            _sections = state.SectionsOn;
            if (_population.Length != _sections.Length)
            {
                _population = new double[_sections.Length];
            }
        });
        _bus.Subscribe<SimulationSnapshot>(snap => _speedMps = snap.VehicleState.SpeedMps);
        return Task.CompletedTask;
    }

    public Task StartAsync() => Task.CompletedTask;
    public Task StopAsync() => Task.CompletedTask;
    public Task ShutdownAsync() => Task.CompletedTask;
    public ModuleHealth GetHealth() => ModuleHealth.Healthy;

    public void Tick()
    {
        if (_bus == null || _population.Length == 0)
        {
            return;
        }

        for (var i = 0; i < _population.Length; i++)
        {
            if (_sections[i])
            {
                _population[i] = 25000 + (_speedMps * 500);
            }
            else
            {
                _population[i] = 0;
            }
        }

        _bus.Publish(new PlanterMonitoringState(_population.ToArray()));
    }
}
