using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sortowania
{
    public class PrzezWybór<T> : IMetodaSortowania<T> where T : IComparable, IComparable<T>
    {
        public void Sortuj(IList<T> kolekcja)
        {
            int iMinimum;
            int n = kolekcja.Count;

            for (int j = 0; j < n - 1; j++)
            {
                iMinimum = j;

                for (int i = j + 1; i < n; i++)
                    if (kolekcja[i].CompareTo(kolekcja[iMinimum]) < 0)
                        iMinimum = i;

                if (iMinimum != j)
                    kolekcja.Zamień(j, iMinimum);
            }
        }
    }
}