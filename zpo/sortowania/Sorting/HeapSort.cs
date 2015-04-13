using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sortowania
{
    public class HeapSort<T> : ISortMethod<T> where T : IComparable, IComparable<T>
    {
        public void Sort(IList<T> collection)
        {
            int count = collection.Count;

            Heapify(collection, count);

            int end = count - 1;

            while (end > 0)
            {
                collection.Swap(end, 0);

                end--;

                SiftDown(collection, 0, end);
            }
        }

        void Heapify(IList<T> a, int count)
        {
            int start = (int)Math.Floor((count - 2.0) / 2);

            while (start >= 0)
            {
                SiftDown(a, start, count - 1);

                start--;
            }
        }

        void SiftDown(IList<T> a, int start, int end)
        {
            int root = start;

            while (root * 2 + 1 <= end)
            {
                int child = root * 2 + 1;
                int swap = root;

                if (a[swap].CompareTo(a[child]) < 0)
                    swap = child;

                if (child + 1 <= end && a[swap].CompareTo(a[child + 1]) < 0)
                    swap = child + 1;

                if (swap == root)
                    break;
                else
                {
                    a.Swap(root, swap);

                    root = swap;
                }
            }
        }
    }
}