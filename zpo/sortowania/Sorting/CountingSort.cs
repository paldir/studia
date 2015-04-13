using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sortowania
{
    public class CountingSort<T> : ISortMethod<T> where T : IComparable, IComparable<T>
    {
        KeyDelegate _keyDelegate;

        public delegate uint KeyDelegate(T key);

        public CountingSort(KeyDelegate keyDelegate)
        {
            _keyDelegate = keyDelegate;
        }

        public void Sort(IList<T> collection)
        {
            int n = collection.Count;
            uint k = collection.Max(c => _keyDelegate(c)) + 1;
            int[] count = new int[k];
            List<T> A = new List<T>(collection);

            for (int i = 0; i < k; i++)
                count[i] = 0;

            for (int i = 0; i < n; i++)
            {
                uint j = _keyDelegate(A[i]);
                count[j]++;
            }

            for (int i = 1; i < k; i++)
                count[i] = count[i] + count[i - 1];

            for (int i = n - 1; i >= 0; i--)
            {
                uint j = _keyDelegate(A[i]);
                collection[count[j] - 1] = A[i];
                count[j]--;
            }
        }
    }
}