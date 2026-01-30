namespace AgNext.Core.Kinematics.AxleCentric;

public sealed record AxleConfig(
    double TrackWidthMeters,
    double MaxSteerAngleRad,
    bool IsDriven,
    bool IsSteered);

public sealed record DrawbarConfig(
    double LengthMeters,
    JointLimits Limits);

public sealed record JointLimits(
    double YawMinRad,
    double YawMaxRad,
    double PitchMinRad,
    double PitchMaxRad,
    double RollMinRad,
    double RollMaxRad);

public sealed record NodeConfig(
    string Id,
    string Type,
    string? ParentId,
    double[] Translation,
    double[] Rotation,
    AxleConfig? Axle,
    DrawbarConfig? Drawbar);

public sealed record AttachmentConfig(
    string Id,
    string ParentId,
    string Type,
    double[] Translation,
    double[] Rotation);

public sealed record RowConfig(
    string ParentId,
    int RowCount,
    double RowSpacingMeters,
    double ShutoffOffsetMeters);

public sealed record AxleCentricConfig(
    string RootId,
    string SteerAxleId,
    IReadOnlyList<NodeConfig> Nodes,
    IReadOnlyList<AttachmentConfig> Attachments,
    RowConfig Rows);
