using System;
using System.Windows.Data;
using System.Windows;

namespace KółkoIKrzyżyk.Widok
{
    class BooleanNaWidocznośćKontrolki : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool wartość = (bool)value;

            if ((bool)parameter)
                wartość = !wartość;

            if (wartość)
                return Visibility.Visible;
            else
                return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}