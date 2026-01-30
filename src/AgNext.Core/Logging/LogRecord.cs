namespace AgNext.Core.Logging;

public sealed record LogRecord(string Type, double TimeSeconds, string JsonPayload);
