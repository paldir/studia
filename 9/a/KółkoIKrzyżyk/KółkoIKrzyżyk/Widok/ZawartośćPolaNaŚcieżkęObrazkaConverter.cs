using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Windows.Data;

namespace KółkoIKrzyżyk.Widok
{
    class ZawartośćPolaNaŚcieżkęDoObrazkaConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            const string folder = "Obrazki";
            string nazwaObrazka;
            ModelWidoku.Pole zawartość = value as ModelWidoku.Pole;

            switch (zawartość.Zawartość)
            {
                case Algorytmy.Pole.Kółko:
                    nazwaObrazka = "kółko.png";

                    break;

                case Algorytmy.Pole.Krzyżyk:
                    nazwaObrazka = "krzyżyk.png";

                    break;

                default:
                    nazwaObrazka = "puste.png";

                    break;
            }

            return Path.GetFullPath(Path.Combine(folder, nazwaObrazka));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}