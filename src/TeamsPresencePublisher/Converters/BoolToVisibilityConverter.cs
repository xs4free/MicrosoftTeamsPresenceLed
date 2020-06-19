using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TeamsPresencePublisher.Converters
{
    public class BoolToVisibilityConverter : DependencyObject, IValueConverter
    {
        public static readonly DependencyProperty TrueValueProperty = 
            DependencyProperty.Register("TrueValue", typeof(Visibility), typeof(BoolToVisibilityConverter), new PropertyMetadata(Visibility.Visible));
        public static readonly DependencyProperty FalseValueProperty = 
            DependencyProperty.Register("FalseValue", typeof(Visibility), typeof(BoolToVisibilityConverter), new PropertyMetadata(Visibility.Collapsed));

        public Visibility TrueValue
        {
            get
            {
                return (Visibility)GetValue(TrueValueProperty);
            }
            set
            {
                SetValue(TrueValueProperty, value);
            }
        }

        public Visibility FalseValue
        {
            get
            {
                return (Visibility)GetValue(FalseValueProperty);
            }
            set
            {
                SetValue(FalseValueProperty, value);
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool flag = value is bool && (bool)value;

            return flag ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
