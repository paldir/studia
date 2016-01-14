using System;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace KółkoIKrzyżyk.Widok
{
    internal class ZawartośćPolaNaŚcieżkęDoObrazkaConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string nazwaObrazka = null;
            ModelWidoku.Pole zawartość = values[0] as ModelWidoku.Pole;
            ModelWidoku.Pole ostatnioWypełnionePole = values[1] as ModelWidoku.Pole;

            if (zawartość != null)
            {
                switch (zawartość.Zawartość)
                {
                    case Algorytmy.Pole.Kółko:
                        nazwaObrazka = "kolko.png";

                        break;

                    case Algorytmy.Pole.Krzyżyk:
                        nazwaObrazka = "krzyzyk.png";

                        break;

                    case Algorytmy.Pole.ZwycięskieKółko:
                        nazwaObrazka = "zwycieskieKolko.png";

                        break;

                    case Algorytmy.Pole.ZwycięskiKrzyżyk:
                        nazwaObrazka = "zwycieskiKrzyzyk.png";

                        break;

                    default:
                        nazwaObrazka = "puste.png";

                        break;
                }

                if (zawartość == ostatnioWypełnionePole)
                    nazwaObrazka = string.Concat("niebieski", nazwaObrazka);
            }

            nazwaObrazka = string.Concat("/Obrazki/", nazwaObrazka);

            return new BitmapImage(new Uri(nazwaObrazka, UriKind.Relative));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}