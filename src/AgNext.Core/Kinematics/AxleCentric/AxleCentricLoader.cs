namespace AgNext.Core.Kinematics.AxleCentric;

using System.Text.Json;

public static class AxleCentricLoader
{
    public static AxleCentricConfig Load(string path)
    {
        var json = File.ReadAllText(path);
        var config = JsonSerializer.Deserialize<AxleCentricConfig>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (config == null)
        {
            throw new InvalidOperationException("Invalid axle-centric config");
        }

        return config;
    }
}
