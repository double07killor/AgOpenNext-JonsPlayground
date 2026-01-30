namespace AgNext.Core.Runtime;

using AgNext.Core.Modules;

public sealed class CoreRuntime
{
    private readonly SimClock _clock;
    private readonly ModuleHost _host;

    public CoreRuntime(SimClock clock, ModuleHost host)
    {
        _clock = clock;
        _host = host;
    }

    public SimTime Current => _clock.Current;

    public void Tick()
    {
        _clock.Tick();
        _host.Tick();
    }
}
