namespace AgNext.Core.Simulation;

using AgNext.Core.Guidance;
using AgNext.Core.Kinematics;

public sealed record SimulatorConfig(
    VehicleConfig Vehicle,
    AgNext.Core.Kinematics.AxleCentric.AxleCentricConfig AxleConfig,
    ITerrainModel Terrain,
    FieldBoundary Boundary,
    double SwathHeadingRad,
    double SwathSpacingMeters,
    double StepSeconds,
    double GnssRateHz,
    double ImuRateHz,
    double WheelSpeedRateHz,
    double SteerAngleRateHz);

public sealed class SimulationSnapshot
{
    public AgNext.Core.Runtime.SimTime SimTime { get; init; }
    public double TimeSeconds { get; init; }
    public VehicleState VehicleState { get; init; } = new();
    public Pose2D ImplementPose { get; init; }
    public IReadOnlyList<AgNext.Core.Geometry.Vec2> Boundary { get; init; } = Array.Empty<AgNext.Core.Geometry.Vec2>();
    public IReadOnlyList<AgNext.Core.Geometry.Vec2> RowCenters { get; init; } = Array.Empty<AgNext.Core.Geometry.Vec2>();
    public IReadOnlyList<AgNext.Core.Geometry.Vec2> RowShutoffPoints { get; init; } = Array.Empty<AgNext.Core.Geometry.Vec2>();
    public bool[] SectionStates { get; init; } = Array.Empty<bool>();
    public IReadOnlyList<AgNext.Core.Geometry.Vec2> PreviewPoints { get; init; } = Array.Empty<AgNext.Core.Geometry.Vec2>();
    public IReadOnlyList<GuidancePath> Swaths { get; init; } = Array.Empty<GuidancePath>();
    public int ActiveSwathIndex { get; init; }
    public bool AutoSteerEnabled { get; init; }
    public PoseEstimate PoseEstimate { get; init; } = new();
    public AgNext.Core.Sensors.GnssSample? GnssSample { get; init; }
    public AgNext.Core.Sensors.ImuSample? ImuSample { get; init; }
    public AgNext.Core.Sensors.WheelSpeedSample? WheelSpeedSample { get; init; }
    public AgNext.Core.Sensors.SteeringAngleSample? SteeringAngleSample { get; init; }
}
