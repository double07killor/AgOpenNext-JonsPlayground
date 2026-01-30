namespace AgNext.Core.Guidance;

using AgNext.Core.Geometry;

public sealed class PathPlanner
{
    public IReadOnlyList<GuidancePath> GenerateSwaths(IReadOnlyList<Vec2> boundary, double headingRad, double swathSpacingMeters)
    {
        var bounds = GetBounds(boundary);
        var axis = Vec2.FromAngle(headingRad);
        var normal = new Vec2(-axis.Y, axis.X);

        var min = double.PositiveInfinity;
        var max = double.NegativeInfinity;
        foreach (var corner in bounds)
        {
            var proj = Dot(corner, normal);
            min = Math.Min(min, proj);
            max = Math.Max(max, proj);
        }

        var paths = new List<GuidancePath>();
        var index = 0;
        for (var offset = min; offset <= max; offset += swathSpacingMeters)
        {
            var center = normal * offset;
            var line = new List<Vec2>
            {
                center + axis * -1000,
                center + axis * 1000
            };
            paths.Add(new GuidancePath(line));
            index++;
        }

        return paths;
    }

    public GuidancePath GenerateHeadlandPath(IReadOnlyList<Vec2> boundary)
    {
        return new GuidancePath(boundary.ToList());
    }

    private static IReadOnlyList<Vec2> GetBounds(IReadOnlyList<Vec2> boundary)
    {
        var minX = boundary.Min(p => p.X);
        var maxX = boundary.Max(p => p.X);
        var minY = boundary.Min(p => p.Y);
        var maxY = boundary.Max(p => p.Y);
        return new[]
        {
            new Vec2(minX, minY),
            new Vec2(maxX, minY),
            new Vec2(maxX, maxY),
            new Vec2(minX, maxY)
        };
    }

    private static double Dot(Vec2 a, Vec2 b) => (a.X * b.X) + (a.Y * b.Y);
}
