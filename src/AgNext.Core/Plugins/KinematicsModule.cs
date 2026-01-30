namespace AgNext.Core.Plugins;

using AgNext.Core.Kinematics;
using AgNext.Core.Messages;
using AgNext.Core.Modules;
using AgNext.Core.Runtime;
using AgNext.Core.Sensors;

public sealed class KinematicsModule : IAgModule, ITickableModule
{
    private readonly SensorFusion _fusion;
    private IMessageBus? _bus;
    private GnssSample? _gnss;
    private ImuSample? _imu;
    private WheelSpeedSample? _wheel;
    private SimTime _lastTime;
    private SimTime _prevTime;
    private bool _hasTime;

    public KinematicsModule()
    {
        _fusion = new SensorFusion(new Pose2D(new AgNext.Core.Geometry.Vec2(0, 0), 0));
    }

    public string Name => "Kinematics";
    public Version Version => new(0, 1, 0);
    public ModuleCategory Category => ModuleCategory.DataProcessing;
    public string[] Dependencies => new[] { "AioBridge" };

    public Task InitializeAsync(IModuleContext context)
    {
        _bus = context.MessageBus;
        _bus.Subscribe<GnssSample>(sample => _gnss = sample);
        _bus.Subscribe<ImuSample>(sample => _imu = sample);
        _bus.Subscribe<WheelSpeedSample>(sample => _wheel = sample);
        _bus.Subscribe<SimTick>(tick =>
        {
            _prevTime = _lastTime;
            _lastTime = tick.Time;
            _hasTime = true;
        });
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

        var dt = _hasTime ? Math.Max(0.001, _lastTime.TimeSeconds - _prevTime.TimeSeconds) : 0.02;
        var estimate = _fusion.Update(_gnss, _imu, _wheel, dt);
        _bus.Publish(new KinematicsState(estimate));
    }
}
