namespace AgNext.Core.Guidance;

using AgNext.Core.Geometry;

public sealed class PathPreviewPlanner
{
    public IReadOnlyList<Vec2> BuildPreview(GuidancePath path, Vec2 currentPosition, double previewMeters, double stepMeters)
    {
        if (path.Points.Count < 2)
        {
            return Array.Empty<Vec2>();
        }

        var a = path.Points[0];
        var b = path.Points[^1];
        var direction = (b - a).Normalized();
        var start = ProjectPointOnLine(currentPosition, a, b);

        var points = new List<Vec2>();
        var steps = (int)Math.Max(1, previewMeters / stepMeters);
        for (var i = 0; i <= steps; i++)
        {
            var distance = i * stepMeters;
            points.Add(start + direction * distance);
        }

        return points;
    }

    private static Vec2 ProjectPointOnLine(Vec2 p, Vec2 a, Vec2 b)
    {
        var ab = b - a;
        var t = ((p.X - a.X) * ab.X + (p.Y - a.Y) * ab.Y) / ((ab.X * ab.X) + (ab.Y * ab.Y));
        t = MathUtil.Clamp(t, 0, 1);
        return new Vec2(a.X + ab.X * t, a.Y + ab.Y * t);
    }
}
