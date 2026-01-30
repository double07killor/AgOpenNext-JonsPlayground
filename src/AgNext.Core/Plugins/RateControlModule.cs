namespace AgNext.Core.Plugins;

using AgNext.Core.Messages;
using AgNext.Core.Modules;

public sealed class RateControlModule : IAgModule, ITickableModule
{
    private IMessageBus? _bus;
    private bool[] _sections = Array.Empty<bool>();
    private double _speedMps;

    public string Name => "RateControl";
    public Version Version => new(0, 1, 0);
    public ModuleCategory Category => ModuleCategory.Control;
    public string[] Dependencies => new[] { "SectionControl" };

    public Task InitializeAsync(IModuleContext context)
    {
        _bus = context.MessageBus;
        _bus.Subscribe<SectionStateMessage>(state => _sections = state.SectionsOn);
        _bus.Subscribe<AgNext.Core.Simulation.SimulationSnapshot>(snap => _speedMps = snap.VehicleState.SpeedMps);
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

        var rates = new double[_sections.Length];
        for (var i = 0; i < _sections.Length; i++)
        {
            rates[i] = _sections[i] ? 120.0 + (_speedMps * 5) : 0.0;
        }
        _bus.Publish(new RateCommandMessage(rates));
    }
}
