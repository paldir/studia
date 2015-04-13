using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sortowania
{
    public class SelectionSort<T> : ISortMethod<T> where T : IComparable, IComparable<T>
    {
        public void Sort(IList<T> collection)
        {
            int iMin;
            int n = collection.Count;

            for (int j = 0; j < n - 1; j++)
            {
                iMin = j;

                for (int i = j + 1; i < n; i++)
                    if (collection[i].CompareTo(collection[iMin]) < 0)
                        iMin = i;

                if (iMin != j)
                    collection.Swap(j, iMin);
            }
        }
    }
}