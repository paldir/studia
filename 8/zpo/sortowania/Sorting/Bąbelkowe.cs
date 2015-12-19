using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sortowania
{
    public class Bąbelkowe<T> : IMetodaSortowania<T> where T : IComparable, IComparable<T>
    {
        public void Sortuj(IList<T> kolekcja)
        {
            int n = kolekcja.Count;
            bool zamiana;

            do
            {
                zamiana = false;

                for (int i = 1; i < n; i++)
                    if (kolekcja[i - 1].CompareTo(kolekcja[i]) > 0)
                    {
                        //kolekcja.Zamień(i - 1, i);

                        T tmp = kolekcja[i - 1];
                        kolekcja[i - 1] = kolekcja[i];
                        kolekcja[i] = tmp;

                        zamiana = true;
                    }

                n--;
            }
            while (zamiana);
        }
    }
}