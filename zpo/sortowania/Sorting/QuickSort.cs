using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sortowania
{
    public class QuickSort<T> : ISortMethod<T> where T : IComparable, IComparable<T>
    {
        public void Sort(IList<T> collection)
        {
            Quick(collection, 0, collection.Count - 1);
        }

        void Quick(IList<T> A, int lo, int hi)
        {
            if (lo < hi)
            {
                int p = Partition(A, lo, hi);

                Quick(A, lo, p - 1);
                Quick(A, p + 1, hi);
            }
        }

        int Partition(IList<T> A, int lo, int hi)
        {
            int pivotIndex = (lo + hi) / 2;
            T pivotValue = A[pivotIndex];

            A.Swap(pivotIndex, hi);

            int storeIndex = lo;

            for (int i = lo; i <= hi - 1; i++)
                if (A[i].CompareTo(pivotValue) < 0)
                {
                    A.Swap(i, storeIndex);

                    storeIndex++;
                }

            A.Swap(storeIndex, hi);

            return storeIndex;
        }
    }
}