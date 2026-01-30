namespace AgNext.Core.Modules;

public interface IAgModule
{
    string Name { get; }
    Version Version { get; }
    ModuleCategory Category { get; }
    string[] Dependencies { get; }

    Task InitializeAsync(IModuleContext context);
    Task StartAsync();
    Task StopAsync();
    Task ShutdownAsync();
    ModuleHealth GetHealth();
}

public interface ITickableModule
{
    void Tick();
}

public enum ModuleCategory
{
    IO = 0,
    DataProcessing = 10,
    Navigation = 20,
    Control = 30,
    Visualization = 40,
    Logging = 50,
    Integration = 60,
    Monitoring = 70
}

public enum ModuleHealth
{
    Healthy,
    Degraded,
    Unhealthy,
    Unknown
}
