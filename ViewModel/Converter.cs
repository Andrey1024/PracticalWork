using System;
using System.Windows.Data;
using System.Windows;
using System.Globalization;

namespace ViewModel
{   
    [ValueConversion(typeof(Visibility), typeof(bool))]
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool tmp = (bool)value;
            if (tmp)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Visibility)value == Visibility.Collapsed;
        }
    }
}
