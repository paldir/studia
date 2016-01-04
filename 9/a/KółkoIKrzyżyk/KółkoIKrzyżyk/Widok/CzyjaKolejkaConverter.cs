using System;
using System.Windows.Data;

namespace KółkoIKrzyżyk.Widok
{
    class CzyjaKolejkaConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value)
                return "gracz";
            else
                return "komputer";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}