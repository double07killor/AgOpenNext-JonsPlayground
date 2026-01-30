namespace AgNext.Core.Messages;

public sealed record ManualControlInput(double SpeedMps, double SteerRad);
public sealed record ToggleAutoSteer;
public sealed record NextSwath;
public sealed record PreviousSwath;
public sealed record SteerCommand(double SteerTargetRad);
public sealed record ToolSteerCommand(double ToolAngleRad);
