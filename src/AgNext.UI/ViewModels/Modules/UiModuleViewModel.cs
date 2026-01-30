namespace AgNext.UI.ViewModels;

using System.ComponentModel;
using System.Runtime.CompilerServices;

public abstract class UiModuleViewModel : INotifyPropertyChanged
{
    private double _x;
    private double _y;
    private double _width;
    private double _height;

    public string Id { get; }
    public string Title { get; }
    public UiModuleRegion Region { get; }
    public bool IsMovable { get; }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected UiModuleViewModel(string id, string title, UiModuleRegion region, bool isMovable)
    {
        Id = id;
        Title = title;
        Region = region;
        IsMovable = isMovable;
    }

    public double X
    {
        get => _x;
        set => SetField(ref _x, value);
    }

    public double Y
    {
        get => _y;
        set => SetField(ref _y, value);
    }

    public double Width
    {
        get => _width;
        set => SetField(ref _width, value);
    }

    public double Height
    {
        get => _height;
        set => SetField(ref _height, value);
    }

    protected void SetField<T>(ref T field, T value, [CallerMemberName] string? name = null)
    {
        if (Equals(field, value)) return;
        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
