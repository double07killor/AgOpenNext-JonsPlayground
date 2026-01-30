namespace AgNext.Core.Guidance;

using AgNext.Core.Geometry;
using AgNext.Core.Kinematics;

public sealed class GuidanceEngine
{
    private readonly IAutoSteerController _controller;
    private readonly PathPreviewPlanner _previewPlanner;

    private readonly IReadOnlyList<GuidancePath> _swaths;
    private int _activeSwathIndex;

    private readonly double _previewMeters;
    private readonly double _previewStepMeters;

    public GuidanceEngine(IAutoSteerController controller, PathPlanner planner, IReadOnlyList<Vec2> boundary, double swathHeadingRad, double swathSpacingMeters, double previewMeters, double previewStepMeters)
    {
        _controller = controller;
        _previewPlanner = new PathPreviewPlanner();
        _swaths = planner.GenerateSwaths(boundary, swathHeadingRad, swathSpacingMeters);
        _previewMeters = previewMeters;
        _previewStepMeters = previewStepMeters;
    }

    public IReadOnlyList<GuidancePath> Swaths => _swaths;
    public int ActiveSwathIndex => _activeSwathIndex;

    public void NextSwath() => _activeSwathIndex = (_activeSwathIndex + 1) % _swaths.Count;
    public void PreviousSwath() => _activeSwathIndex = (_activeSwathIndex - 1 + _swaths.Count) % _swaths.Count;

    public GuidanceOutput Update(Pose2D pose)
    {
        var path = _swaths[_activeSwathIndex];
        var steerTarget = _controller.ComputeSteerTarget(pose, path);
        var preview = _previewPlanner.BuildPreview(path, pose.Position, _previewMeters, _previewStepMeters);
        return new GuidanceOutput
        {
            SteerTargetRad = steerTarget,
            PreviewPoints = preview,
            ActivePath = path,
            Swaths = _swaths,
            ActiveSwathIndex = _activeSwathIndex
        };
    }
}
