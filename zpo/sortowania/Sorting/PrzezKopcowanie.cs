using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sortowania
{
    public class PrzezKopcowanie<T> : IMetodaSortowania<T> where T : IComparable, IComparable<T>
    {
        public void Sortuj(IList<T> kolekcja)
        {
            int ilość = kolekcja.Count;

            Kopcuj(kolekcja, ilość);

            int koniec = ilość - 1;

            while (koniec > 0)
            {
                kolekcja.Zamień(koniec, 0);

                koniec--;

                Przesiej(kolekcja, 0, koniec);
            }
        }

        void Kopcuj(IList<T> a, int ilość)
        {
            int początek = (int)Math.Floor((ilość - 2.0) / 2);

            while (początek >= 0)
            {
                Przesiej(a, początek, ilość - 1);

                początek--;
            }
        }

        void Przesiej(IList<T> a, int początek, int koniec)
        {
            int korzeń = początek;

            while (korzeń * 2 + 1 <= koniec)
            {
                int dziecko = korzeń * 2 + 1;
                int zamiana = korzeń;

                if (a[zamiana].CompareTo(a[dziecko]) < 0)
                    zamiana = dziecko;

                if (dziecko + 1 <= koniec && a[zamiana].CompareTo(a[dziecko + 1]) < 0)
                    zamiana = dziecko + 1;

                if (zamiana == korzeń)
                    break;
                else
                {
                    a.Zamień(korzeń, zamiana);

                    korzeń = zamiana;
                }
            }
        }
    }
}