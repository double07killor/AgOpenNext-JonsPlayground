namespace AgNext.Core.Plugins;

using AgNext.Core.Guidance;
using AgNext.Core.Messages;
using AgNext.Core.Modules;

public sealed class GuidanceModule : IAgModule, ITickableModule
{
    private readonly ToolSteerPlanner _toolPlanner = new();
    private readonly double _swathHeadingRad;
    private readonly double _swathSpacingMeters;
    private readonly IReadOnlyList<AgNext.Core.Geometry.Vec2> _initialBoundary;
    private GuidanceEngine _engine;
    private readonly MpcSteerController _controller;
    private IMessageBus? _bus;
    private AgNext.Core.Kinematics.PoseEstimate? _pose;
    private GuidanceOutput _output = new();
    private AgNext.Core.Kinematics.Pose2D _implementPose;
    private bool _hasImplementPose;
    private double _speedMps;

    public GuidanceModule(IReadOnlyList<AgNext.Core.Geometry.Vec2> boundary, double swathHeadingRad, double swathSpacingMeters)
    {
        var planner = new PathPlanner();
        _controller = new MpcSteerController(wheelbaseMeters: 2.8, lookaheadMeters: 6);
        _initialBoundary = boundary;
        _swathHeadingRad = swathHeadingRad;
        _swathSpacingMeters = swathSpacingMeters;
        _engine = new GuidanceEngine(_controller, planner, boundary, swathHeadingRad, swathSpacingMeters, previewMeters: 30, previewStepMeters: 2.0);
    }

    public string Name => "Guidance";
    public Version Version => new(0, 1, 0);
    public ModuleCategory Category => ModuleCategory.Navigation;
    public string[] Dependencies => new[] { "Kinematics" };

    public Task InitializeAsync(IModuleContext context)
    {
        _bus = context.MessageBus;
        _bus.Subscribe<KinematicsState>(state =>
        {
            _pose = state.Estimate;
            _speedMps = state.Estimate.SpeedMps;
        });
        _bus.Subscribe<AgNext.Core.Simulation.SimulationSnapshot>(snap =>
        {
            _implementPose = snap.ImplementPose;
            _hasImplementPose = true;
        });
        _bus.Subscribe<BoundaryUpdated>(msg =>
        {
            var planner = new PathPlanner();
            _engine = new GuidanceEngine(_controller, planner, msg.Points, _swathHeadingRad, _swathSpacingMeters, previewMeters: 30, previewStepMeters: 2.0);
        });
        _bus.Subscribe<NextSwath>(_ => _engine.NextSwath());
        _bus.Subscribe<PreviousSwath>(_ => _engine.PreviousSwath());
        return Task.CompletedTask;
    }

    public Task StartAsync() => Task.CompletedTask;
    public Task StopAsync() => Task.CompletedTask;
    public Task ShutdownAsync() => Task.CompletedTask;
    public ModuleHealth GetHealth() => ModuleHealth.Healthy;

    public void Tick()
    {
        if (_bus == null || _pose == null)
        {
            return;
        }

        _controller.SetSpeed(_speedMps);
        _output = _engine.Update(_pose.Pose);
        _bus.Publish(_output);
        _bus.Publish(new SteerCommand(_output.SteerTargetRad));

        var toolAngle = 0.0;
        if (_hasImplementPose)
        {
            toolAngle = _toolPlanner.ComputeToolAngle(_implementPose, _output.ActivePath);
        }
        _bus.Publish(new ToolSteerCommand(toolAngle));
    }
}
