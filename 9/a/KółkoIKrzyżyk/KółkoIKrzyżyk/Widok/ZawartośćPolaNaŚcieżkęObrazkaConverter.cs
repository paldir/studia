using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace KółkoIKrzyżyk.Widok
{
    class ZawartośćPolaNaŚcieżkęDoObrazkaConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            const string folder = "Obrazki";
            string nazwaObrazka;
            ModelWidoku.Pole zawartość = values[0] as ModelWidoku.Pole;
            ModelWidoku.Pole ostatnioWypełnionePole = values[1] as ModelWidoku.Pole;

            switch (zawartość.Zawartość)
            {
                case Algorytmy.Pole.Kółko:
                    nazwaObrazka = "kółko.png";

                    break;

                case Algorytmy.Pole.Krzyżyk:
                    nazwaObrazka = "krzyżyk.png";

                    break;

                case Algorytmy.Pole.ZwycięskieKółko:
                    nazwaObrazka = "zwycięskieKółko.png";

                    break;

                case Algorytmy.Pole.ZwycięskiKrzyżyk:
                    nazwaObrazka = "zwycięskiKrzyżyk.png";

                    break;

                default:
                    nazwaObrazka = "puste.png";

                    break;
            }

            if (zawartość == ostatnioWypełnionePole)
                nazwaObrazka = String.Concat("niebieski", nazwaObrazka);

            return new BitmapImage(new Uri(Path.GetFullPath(Path.Combine(folder, nazwaObrazka))));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}