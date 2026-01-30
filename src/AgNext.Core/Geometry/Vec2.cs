namespace AgNext.Core.Geometry;

public readonly struct Vec2
{
    public double X { get; }
    public double Y { get; }

    public Vec2(double x, double y)
    {
        X = x;
        Y = y;
    }

    public double Length => Math.Sqrt(X * X + Y * Y);

    public Vec2 Normalized()
    {
        var len = Length;
        return len > 1e-9 ? new Vec2(X / len, Y / len) : new Vec2(0, 0);
    }

    public static Vec2 operator +(Vec2 a, Vec2 b) => new(a.X + b.X, a.Y + b.Y);
    public static Vec2 operator -(Vec2 a, Vec2 b) => new(a.X - b.X, a.Y - b.Y);
    public static Vec2 operator *(Vec2 a, double s) => new(a.X * s, a.Y * s);

    public static Vec2 FromAngle(double radians) => new(Math.Cos(radians), Math.Sin(radians));

    public Vec2 Rotate(double radians)
    {
        var c = Math.Cos(radians);
        var s = Math.Sin(radians);
        return new Vec2((X * c) - (Y * s), (X * s) + (Y * c));
    }
}
