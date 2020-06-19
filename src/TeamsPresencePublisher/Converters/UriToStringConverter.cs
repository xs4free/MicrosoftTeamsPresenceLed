using System;
using System.Globalization;
using System.Windows.Data;

namespace TeamsPresencePublisher.Converters
{
    public class UriToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((Uri)value).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Uri((string)value);
        }
    }
}
