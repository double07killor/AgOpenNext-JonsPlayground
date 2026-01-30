namespace AgNext.Core.Logging;

public interface ILogSink : IDisposable
{
    void Write(LogRecord record);
}
