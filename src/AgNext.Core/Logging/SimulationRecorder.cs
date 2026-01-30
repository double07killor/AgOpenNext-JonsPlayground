namespace AgNext.Core.Logging;

using System.Text.Json;
using AgNext.Core.Simulation;

public sealed class SimulationRecorder
{
    private readonly ILogSink _sink;

    public SimulationRecorder(ILogSink sink)
    {
        _sink = sink;
    }

    public void RecordSnapshot(SimulationSnapshot snapshot)
    {
        var payload = JsonSerializer.Serialize(snapshot, new JsonSerializerOptions { WriteIndented = false });
        _sink.Write(new LogRecord("snapshot", snapshot.TimeSeconds, payload));
    }
}
