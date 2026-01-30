namespace AgNext.UI.ViewModels;

using System.ComponentModel;
using System.Runtime.CompilerServices;

public sealed class RowStatusViewModel : INotifyPropertyChanged
{
    private bool _isOn;
    private double _rate;
    private double _population;

    public int Index { get; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public RowStatusViewModel(int index)
    {
        Index = index;
    }

    public bool IsOn
    {
        get => _isOn;
        set => SetField(ref _isOn, value);
    }

    public double Rate
    {
        get => _rate;
        set => SetField(ref _rate, value);
    }

    public double Population
    {
        get => _population;
        set => SetField(ref _population, value);
    }

    private void SetField<T>(ref T field, T value, [CallerMemberName] string? name = null)
    {
        if (Equals(field, value)) return;
        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
