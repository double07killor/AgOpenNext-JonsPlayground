namespace AgNext.Core.Logging;

public sealed class InMemoryLogSink : ILogSink
{
    private readonly List<LogRecord> _records = new();

    public IReadOnlyList<LogRecord> Records => _records;

    public void Write(LogRecord record)
    {
        _records.Add(record);
    }

    public void Dispose()
    {
    }
}
