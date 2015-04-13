using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sortowania
{
    public class BubbleSort<T> : ISortMethod<T> where T : IComparable, IComparable<T>
    {
        public void Sort(IList<T> collection)
        {
            int n = collection.Count;
            bool swapped;

            do
            {
                swapped = false;

                for (int i = 1; i < n; i++)
                    if (collection[i - 1].CompareTo(collection[i]) > 0)
                    {
                        collection.Swap(i - 1, i);

                        swapped = true;
                    }

                n--;
            }
            while (swapped);
        }
    }
}