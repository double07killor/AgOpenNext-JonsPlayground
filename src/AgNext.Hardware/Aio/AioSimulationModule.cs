namespace AgNext.Hardware.Aio;

using AgNext.Core.Modules;
using AgNext.Core.Simulation;

public sealed class AioSimulationModule : IAgModule, ITickableModule
{
    private readonly IAioTransport _transport;
    private SimulationSnapshot? _snapshot;

    public AioSimulationModule(IAioTransport transport)
    {
        _transport = transport;
    }

    public string Name => "AioSimulation";
    public Version Version => new(0, 1, 0);
    public ModuleCategory Category => ModuleCategory.IO;
    public string[] Dependencies => Array.Empty<string>();

    public Task InitializeAsync(IModuleContext context)
    {
        context.MessageBus.Subscribe<SimulationSnapshot>(snap => _snapshot = snap);
        return Task.CompletedTask;
    }

    public Task StartAsync() => Task.CompletedTask;
    public Task StopAsync() => Task.CompletedTask;
    public Task ShutdownAsync() => Task.CompletedTask;
    public ModuleHealth GetHealth() => ModuleHealth.Healthy;

    public void Tick()
    {
        if (_snapshot == null)
        {
            return;
        }

        if (_snapshot.GnssSample != null)
        {
            var payload = new byte[8];
            var lat = (int)(_snapshot.GnssSample.Position.Y * 1e7);
            var lon = (int)(_snapshot.GnssSample.Position.X * 1e7);
            BitConverter.GetBytes(lat).CopyTo(payload, 0);
            BitConverter.GetBytes(lon).CopyTo(payload, 4);
            _transport.Send(new AioFrame(0x7C, AioMessageCatalog.GnssMainPgn, payload));
        }

        if (_snapshot.ImuSample != null)
        {
            var payload = new byte[6];
            var yawRate = (short)(_snapshot.ImuSample.YawRateRadPerSec * 1000);
            var roll = (short)(_snapshot.ImuSample.RollRad * 1000);
            var pitch = (short)(_snapshot.ImuSample.PitchRad * 1000);
            BitConverter.GetBytes(yawRate).CopyTo(payload, 0);
            BitConverter.GetBytes(roll).CopyTo(payload, 2);
            BitConverter.GetBytes(pitch).CopyTo(payload, 4);
            _transport.Send(new AioFrame(0x79, AioMessageCatalog.ImuPgn, payload));
        }
    }
}
