namespace AgNext.Core.Logging;

using System.Text.Json;

public sealed class JsonlLogSink : ILogSink
{
    private readonly StreamWriter _writer;

    public JsonlLogSink(string filePath)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(filePath) ?? ".");
        _writer = new StreamWriter(File.Open(filePath, FileMode.Create, FileAccess.Write, FileShare.Read))
        {
            AutoFlush = true
        };
    }

    public void Write(LogRecord record)
    {
        var json = JsonSerializer.Serialize(record);
        _writer.WriteLine(json);
    }

    public void Dispose()
    {
        _writer.Dispose();
    }
}
