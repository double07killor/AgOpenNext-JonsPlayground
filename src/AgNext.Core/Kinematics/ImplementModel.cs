namespace AgNext.Core.Kinematics;

using AgNext.Core.Geometry;

public sealed class ImplementModel
{
    private readonly ImplementConfig _config;

    public ImplementModel(ImplementConfig config)
    {
        _config = config;
    }

    public Pose2D GetImplementPose(Pose2D vehiclePose)
    {
        var hitchOffset = vehiclePose.Forward * -_config.HitchOffsetMeters;
        var toolOffset = vehiclePose.Forward * -_config.ToolForwardOffsetMeters;
        var position = vehiclePose.Position + hitchOffset + toolOffset;
        return new Pose2D(position, vehiclePose.HeadingRad);
    }

    public IReadOnlyList<Vec2> GetRowCenters(Pose2D implementPose)
    {
        var rows = new Vec2[_config.RowCount];
        var half = (_config.RowCount - 1) / 2.0;
        for (var i = 0; i < _config.RowCount; i++)
        {
            var lateral = (i - half) * _config.RowSpacingMeters;
            var offset = (implementPose.Forward * 0) + (implementPose.Left * lateral);
            rows[i] = implementPose.Position + offset;
        }
        return rows;
    }

    public IReadOnlyList<Vec2> GetRowShutoffPoints(Pose2D implementPose)
    {
        var rows = new Vec2[_config.RowCount];
        var half = (_config.RowCount - 1) / 2.0;
        var forwardOffset = implementPose.Forward * _config.ShutoffOffsetMeters;
        for (var i = 0; i < _config.RowCount; i++)
        {
            var lateral = (i - half) * _config.RowSpacingMeters;
            var offset = forwardOffset + (implementPose.Left * lateral);
            rows[i] = implementPose.Position + offset;
        }
        return rows;
    }
}
