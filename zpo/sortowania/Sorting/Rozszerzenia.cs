using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sortowania
{
    public static class Rozszerzenia
    {
        /*public static void Zamień<T>(this IList<T> kolekcja, int indeks1, int indeks2)
        {
            T tmp = kolekcja[indeks1];
            kolekcja[indeks1] = kolekcja[indeks2];
            kolekcja[indeks2] = tmp;
        }*/

        public static void Sortuj<T>(this IList<T> kolekcja, IMetodaSortowania<T> metodaSortowania) where T : IComparable, IComparable<T>
        {
            metodaSortowania.Sortuj(kolekcja);
        }
    }
}