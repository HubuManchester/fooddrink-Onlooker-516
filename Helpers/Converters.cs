using System.Globalization;

namespace FoodPicker.Helpers;

/// 布尔值取反转换器，用于 XAML 绑定
/// e.g. IsVisible="{Binding IsEmpty, Converter={StaticResource InvertedBoolConverter}}"
public class InvertedBoolConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool b)
            return !b;
        return value;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool b)
            return !b;
        return value;
    }
}
