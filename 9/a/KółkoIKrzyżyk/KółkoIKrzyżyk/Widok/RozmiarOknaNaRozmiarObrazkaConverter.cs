using System;
using System.Windows.Data;

namespace KółkoIKrzyżyk.Widok
{
    class RozmiarOknaNaRozmiarObrazkaConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (System.Convert.ToDouble(values[0]) * 0.85) / System.Convert.ToDouble(values[1]);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}