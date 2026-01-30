namespace AgNext.UI.ViewModels;

public sealed class MetricModuleViewModel : UiModuleViewModel
{
    private string _value;
    private string _accent;

    public MetricModuleViewModel(string id, string title, string value, double x, double y, double width, double height, string accent)
        : base(id, title, UiModuleRegion.Overlay, isMovable: true)
    {
        _value = value;
        _accent = accent;
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public string Value
    {
        get => _value;
        set => SetField(ref _value, value);
    }

    public string Accent
    {
        get => _accent;
        set => SetField(ref _accent, value);
    }
}
