namespace AgNext.Core.Geometry;

public readonly struct Quaternion
{
    public double X { get; }
    public double Y { get; }
    public double Z { get; }
    public double W { get; }

    public Quaternion(double x, double y, double z, double w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    public static Quaternion Identity => new(0, 0, 0, 1);

    public static Quaternion FromYaw(double yawRad)
    {
        var half = yawRad * 0.5;
        return new Quaternion(0, 0, Math.Sin(half), Math.Cos(half));
    }

    public static Quaternion FromYawPitchRoll(double yawRad, double pitchRad, double rollRad)
    {
        var cy = Math.Cos(yawRad * 0.5);
        var sy = Math.Sin(yawRad * 0.5);
        var cp = Math.Cos(pitchRad * 0.5);
        var sp = Math.Sin(pitchRad * 0.5);
        var cr = Math.Cos(rollRad * 0.5);
        var sr = Math.Sin(rollRad * 0.5);

        return new Quaternion(
            sr * cp * cy - cr * sp * sy,
            cr * sp * cy + sr * cp * sy,
            cr * cp * sy - sr * sp * cy,
            cr * cp * cy + sr * sp * sy);
    }

    public Quaternion Conjugate() => new(-X, -Y, -Z, W);

    public Quaternion Normalize()
    {
        var mag = Math.Sqrt((X * X) + (Y * Y) + (Z * Z) + (W * W));
        return mag > 1e-9 ? new Quaternion(X / mag, Y / mag, Z / mag, W / mag) : Identity;
    }

    public Vec3 Rotate(Vec3 v)
    {
        var qv = new Quaternion(v.X, v.Y, v.Z, 0);
        var result = this * qv * Conjugate();
        return new Vec3(result.X, result.Y, result.Z);
    }

    public double ToYaw()
    {
        var siny = 2.0 * (W * Z + X * Y);
        var cosy = 1.0 - 2.0 * (Y * Y + Z * Z);
        return Math.Atan2(siny, cosy);
    }

    public static Quaternion operator *(Quaternion a, Quaternion b)
    {
        return new Quaternion(
            a.W * b.X + a.X * b.W + a.Y * b.Z - a.Z * b.Y,
            a.W * b.Y - a.X * b.Z + a.Y * b.W + a.Z * b.X,
            a.W * b.Z + a.X * b.Y - a.Y * b.X + a.Z * b.W,
            a.W * b.W - a.X * b.X - a.Y * b.Y - a.Z * b.Z);
    }
}
