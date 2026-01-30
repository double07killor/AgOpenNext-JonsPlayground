namespace AgNext.Core.Logging;

using System.Text.Json;
using AgNext.Core.Simulation;

public sealed class SimulationPlayback
{
    public static IReadOnlyList<SimulationSnapshot> ReadSnapshots(string filePath)
    {
        var snapshots = new List<SimulationSnapshot>();
        foreach (var line in File.ReadLines(filePath))
        {
            var record = JsonSerializer.Deserialize<LogRecord>(line);
            if (record is null || record.Type != "snapshot")
            {
                continue;
            }

            var snapshot = JsonSerializer.Deserialize<SimulationSnapshot>(record.JsonPayload);
            if (snapshot != null)
            {
                snapshots.Add(snapshot);
            }
        }
        return snapshots;
    }
}
