namespace AgNext.Core.Geometry;

public readonly struct Vec3
{
    public double X { get; }
    public double Y { get; }
    public double Z { get; }

    public Vec3(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public double Length => Math.Sqrt((X * X) + (Y * Y) + (Z * Z));

    public Vec3 Normalized()
    {
        var len = Length;
        return len > 1e-9 ? new Vec3(X / len, Y / len, Z / len) : new Vec3(0, 0, 1);
    }

    public static Vec3 operator +(Vec3 a, Vec3 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    public static Vec3 operator -(Vec3 a, Vec3 b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    public static Vec3 operator *(Vec3 a, double s) => new(a.X * s, a.Y * s, a.Z * s);
    public static Vec3 operator -(Vec3 a) => new(-a.X, -a.Y, -a.Z);
}
