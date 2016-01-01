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
                    return "w toku";

                case Algorytmy.WynikGry.Remis:
                    return "remis";

                case Algorytmy.WynikGry.Kółko:
                    return "zwycięstwo kółka";

                case Algorytmy.WynikGry.Krzyżyk:
                    return "zwycięstwo krzyżyka";

                default:
                    return "nierozpoczęta";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}