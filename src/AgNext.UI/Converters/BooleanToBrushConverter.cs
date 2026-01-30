namespace AgNext.UI;

using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

public sealed class BooleanToBrushConverter : IValueConverter
{
    public static readonly BooleanToBrushConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool enabled && enabled)
        {
            return Brushes.LimeGreen;
        }
        return Brushes.DarkRed;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
