using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace YtDlpWPF.Converters;

public class BoolToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type t, object p, CultureInfo c)
        => value is true ? Visibility.Visible : Visibility.Collapsed;
    public object ConvertBack(object value, Type t, object p, CultureInfo c)
        => throw new NotImplementedException();
}

public class BoolToInverseVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type t, object p, CultureInfo c)
        => value is true ? Visibility.Collapsed : Visibility.Visible;
    public object ConvertBack(object value, Type t, object p, CultureInfo c)
        => throw new NotImplementedException();
}

public class BoolNegateConverter : IValueConverter
{
    public object Convert(object value, Type t, object p, CultureInfo c)
        => value is bool b ? !b : value;
    public object ConvertBack(object value, Type t, object p, CultureInfo c)
        => value is bool b ? !b : value;
}
