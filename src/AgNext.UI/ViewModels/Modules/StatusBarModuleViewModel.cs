namespace AgNext.UI.ViewModels;

public sealed class StatusBarModuleViewModel : UiModuleViewModel
{
    private string _text;

    public StatusBarModuleViewModel(string id, string text)
        : base(id, "status", UiModuleRegion.StatusBar, isMovable: false)
    {
        _text = text;
    }

    public string Text
    {
        get => _text;
        set => SetField(ref _text, value);
    }
}
