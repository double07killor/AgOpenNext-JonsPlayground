namespace AgNext.Core.Messages;

using AgNext.Core.Geometry;

public sealed record BoundaryUpdated(IReadOnlyList<Vec2> Points);
