namespace AgNext.Core.Modules;

public sealed class ModuleHost
{
    private readonly List<IAgModule> _modules = new();
    private readonly IMessageBus _bus;
    private readonly IServiceProvider _services;

    public ModuleHost(IMessageBus bus, IServiceProvider services)
    {
        _bus = bus;
        _services = services;
    }

    public IReadOnlyList<IAgModule> Modules => _modules;

    public void Register(IAgModule module)
    {
        _modules.Add(module);
    }

    public async Task InitializeAsync()
    {
        var context = new ModuleContext(_bus, _services);
        foreach (var module in OrderModules())
        {
            await module.InitializeAsync(context);
        }
    }

    public async Task StartAsync()
    {
        foreach (var module in OrderModules())
        {
            await module.StartAsync();
        }
    }

    public async Task StopAsync()
    {
        foreach (var module in OrderModules().Reverse())
        {
            await module.StopAsync();
        }
    }

    public async Task ShutdownAsync()
    {
        foreach (var module in OrderModules().Reverse())
        {
            await module.ShutdownAsync();
        }
    }

    public void Tick()
    {
        foreach (var tickable in OrderModules().OfType<ITickableModule>())
        {
            tickable.Tick();
        }
    }

    private IEnumerable<IAgModule> OrderModules()
    {
        var map = _modules.ToDictionary(m => m.Name, StringComparer.OrdinalIgnoreCase);
        var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var ordered = new List<IAgModule>();

        foreach (var module in _modules.OrderBy(m => m.Category).ThenBy(m => m.Name))
        {
            Visit(module);
        }

        return ordered;

        void Visit(IAgModule module)
        {
            if (!visited.Add(module.Name))
            {
                return;
            }

            foreach (var dep in module.Dependencies)
            {
                if (map.TryGetValue(dep, out var depModule))
                {
                    Visit(depModule);
                }
            }

            ordered.Add(module);
        }
    }
}
