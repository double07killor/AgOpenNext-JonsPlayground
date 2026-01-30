namespace AgNext.Core.Guidance;

using AgNext.Core.Geometry;

public sealed class GuidanceOutput
{
    public double SteerTargetRad { get; init; }
    public IReadOnlyList<Vec2> PreviewPoints { get; init; } = Array.Empty<Vec2>();
    public GuidancePath ActivePath { get; init; } = new(new List<Vec2>());
    public IReadOnlyList<GuidancePath> Swaths { get; init; } = Array.Empty<GuidancePath>();
    public int ActiveSwathIndex { get; init; }
}
