namespace AgNext.Core.Kinematics;

using AgNext.Core.Geometry;

public interface ITerrainModel
{
    double GetHeight(Vec2 position);
    Vec3 GetNormal(Vec2 position);
}

public sealed class FlatTerrainModel : ITerrainModel
{
    public double GetHeight(Vec2 position) => 0;
    public Vec3 GetNormal(Vec2 position) => new Vec3(0, 0, 1);
}

public sealed class TiltedPlaneTerrainModel : ITerrainModel
{
    public double SlopeX { get; }
    public double SlopeY { get; }

    public TiltedPlaneTerrainModel(double slopeX, double slopeY)
    {
        SlopeX = slopeX;
        SlopeY = slopeY;
    }

    public double GetHeight(Vec2 position) => (position.X * SlopeX) + (position.Y * SlopeY);

    public Vec3 GetNormal(Vec2 position)
    {
        var normal = new Vec3(-SlopeX, -SlopeY, 1);
        return normal.Normalized();
    }
}
