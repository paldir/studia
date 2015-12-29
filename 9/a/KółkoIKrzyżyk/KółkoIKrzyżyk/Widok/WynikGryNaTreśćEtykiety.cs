using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Data;

namespace KółkoIKrzyżyk.Widok
{
    class WynikGryNaTreśćEtykiety : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch ((Algorytmy.WynikGry)value)
            {
                case Algorytmy.WynikGry.Trwająca:
                    return "Gra w toku.";

                case Algorytmy.WynikGry.Remis:
                    return "Remis.";

                case Algorytmy.WynikGry.Kółko:
                    return "Zwycięstwo kółka.";

                case Algorytmy.WynikGry.Krzyżyk:
                    return "Zwycięstwo krzyżyka.";

                default:
                    return "Gra nierozpoczęta.";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}