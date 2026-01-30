namespace AgNext.Hardware.Aio;

using AgNext.Core.Modules;
using AgNext.Core.Sensors;

public sealed class AioBridgeModule : IAgModule
{
    private readonly AioBridge _bridge;
    private IMessageBus? _bus;

    public AioBridgeModule(IAioTransport transport)
    {
        _bridge = new AioBridge(transport);
    }

    public string Name => "AioBridge";
    public Version Version => new(0, 1, 0);
    public ModuleCategory Category => ModuleCategory.IO;
    public string[] Dependencies => Array.Empty<string>();

    public Task InitializeAsync(IModuleContext context)
    {
        _bus = context.MessageBus;
        _bridge.GnssReceived += sample => _bus?.Publish(sample);
        _bridge.ImuReceived += sample => _bus?.Publish(sample);
        return Task.CompletedTask;
    }

    public Task StartAsync() => Task.CompletedTask;
    public Task StopAsync() => Task.CompletedTask;
    public Task ShutdownAsync() => Task.CompletedTask;
    public ModuleHealth GetHealth() => ModuleHealth.Healthy;
}
