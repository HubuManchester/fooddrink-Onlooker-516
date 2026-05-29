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

/// 字符串是否为空 → 用于显示"添加评价"提示
public class IsEmptyConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => string.IsNullOrWhiteSpace(value as string);

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

/// 字符串是否非空 → 用于显示笔记内容
public class IsNotEmptyConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => !string.IsNullOrWhiteSpace(value as string);

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
