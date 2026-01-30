namespace AgNext.Core.Simulation;

using AgNext.Core.Geometry;

public sealed class FieldBoundary
{
    public IReadOnlyList<Vec2> Points { get; }

    public FieldBoundary(IReadOnlyList<Vec2> points)
    {
        Points = points;
    }

    public bool Contains(Vec2 point)
    {
        var inside = false;
        for (int i = 0, j = Points.Count - 1; i < Points.Count; j = i++)
        {
            var pi = Points[i];
            var pj = Points[j];
            var intersect = ((pi.Y > point.Y) != (pj.Y > point.Y)) &&
                            (point.X < (pj.X - pi.X) * (point.Y - pi.Y) / (pj.Y - pi.Y + 1e-9) + pi.X);
            if (intersect)
            {
                inside = !inside;
            }
        }
        return inside;
    }
}
