using System;
using System.Windows.Data;

namespace ClientMessenger.Services;

class FirstCharValueConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        string s = value.ToString();
        int prefixLength;
        if (!int.TryParse(parameter.ToString(), out prefixLength) ||
            s.Length <= prefixLength)
        {
            return s;
        }
        return s.Substring(0, prefixLength);
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}