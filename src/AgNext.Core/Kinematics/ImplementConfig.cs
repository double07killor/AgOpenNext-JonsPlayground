namespace AgNext.Core.Kinematics;

public sealed record ImplementConfig(
    double HitchOffsetMeters,
    double ToolForwardOffsetMeters,
    int RowCount,
    double RowSpacingMeters,
    double ShutoffOffsetMeters);
