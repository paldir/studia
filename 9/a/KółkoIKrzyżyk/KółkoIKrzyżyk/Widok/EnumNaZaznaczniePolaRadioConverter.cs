﻿using System;
using System.Windows.Data;

namespace KółkoIKrzyżyk.Widok
{
    internal class EnumNaZaznaczniePolaRadioConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value.Equals(true))
                return parameter;
            else
                return Binding.DoNothing;
        }
    }
}