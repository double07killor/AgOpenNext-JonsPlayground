namespace AgNext.Core.Messages;

public sealed record SectionStateMessage(bool[] SectionsOn);
public sealed record RateCommandMessage(double[] TargetRates);

public sealed record MonitoringState(
    double SpeedMps,
    double HeadingDeg,
    string FixQuality,
    double PositionVariance,
    double HeadingVariance);

public sealed record PlanterMonitoringState(double[] RowPopulation);

public sealed record UiState(
    AgNext.Core.Simulation.SimulationSnapshot Snapshot,
    SectionStateMessage SectionState,
    RateCommandMessage RateCommand,
    MonitoringState Monitoring,
    PlanterMonitoringState Planter);
