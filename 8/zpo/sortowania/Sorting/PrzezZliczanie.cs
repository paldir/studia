using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sortowania
{
    public class PrzezZliczanie<T> : IMetodaSortowania<T> where T : IComparable, IComparable<T>
    {
        Func<T, int> _metodaKlucza;

        public PrzezZliczanie(Func<T, int> metodaKlucza)
        {
            _metodaKlucza = metodaKlucza;
        }

        public void Sortuj(IList<T> kolekcja)
        {
            int n = kolekcja.Count;
            int k = kolekcja.Max(_metodaKlucza) + 1;
            int[] ilość = new int[k];
            List<T> A = new List<T>(kolekcja);

            for (int i = 0; i < n; i++)
            {
                int j = _metodaKlucza(A[i]);
                ilość[j]++;
            }

            for (int i = 1; i < k; i++)
                ilość[i] += ilość[i - 1];

            for (int i = n - 1; i >= 0; i--)
            {
                int j = _metodaKlucza(A[i]);
                kolekcja[ilość[j] - 1] = A[i];
                ilość[j]--;
            }
        }
    }
}