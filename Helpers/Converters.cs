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

/// bool → Color，用于分类标签高亮
public class BoolToColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isSelected && isSelected)
            return Color.FromArgb("#E85D04");  // 选中：橙色
        return Color.FromArgb("#FFF0E6");       // 未选中：浅橙
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

/// bool → Color，分类标签文字颜色
public class BoolToTextColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isSelected && isSelected)
            return Colors.White;   // 选中：白色文字
        return Color.FromArgb("#E85D04");  // 未选中：橙色文字
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
