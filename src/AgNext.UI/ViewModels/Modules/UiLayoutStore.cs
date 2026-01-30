namespace AgNext.UI.ViewModels;

using System.Text.Json;
using System.Linq;

public sealed class UiLayoutStore
{
    private readonly string _layoutPath;

    public UiLayoutStore(string? layoutPath = null)
    {
        _layoutPath = layoutPath ?? Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "AgOpenNext",
            "ui-layout.json");
    }

    public IReadOnlyDictionary<string, UiModuleLayout> Load()
    {
        if (!File.Exists(_layoutPath))
        {
            return new Dictionary<string, UiModuleLayout>();
        }

        try
        {
            var json = File.ReadAllText(_layoutPath);
            var layouts = JsonSerializer.Deserialize<List<UiModuleLayout>>(json) ?? new List<UiModuleLayout>();
            return layouts.Where(layout => !string.IsNullOrWhiteSpace(layout.Id))
                .ToDictionary(layout => layout.Id, layout => layout);
        }
        catch
        {
            return new Dictionary<string, UiModuleLayout>();
        }
    }

    public void Save(IEnumerable<UiModuleViewModel> modules)
    {
        var layouts = modules
            .Where(module => module.IsMovable)
            .Select(module => new UiModuleLayout
            {
                Id = module.Id,
                X = module.X,
                Y = module.Y,
                Width = module.Width,
                Height = module.Height
            })
            .ToList();

        var directory = Path.GetDirectoryName(_layoutPath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var json = JsonSerializer.Serialize(layouts, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_layoutPath, json);
    }
}

public sealed class UiModuleLayout
{
    public string Id { get; set; } = string.Empty;
    public double X { get; set; }
    public double Y { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
}
