using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sortowania
{
    public static class Extensions
    {
        public static void Swap<T>(this IList<T> collection, int index1, int index2)
        {
            T tmp = collection[index1];
            collection[index1] = collection[index2];
            collection[index2] = tmp;
        }

        public static void Sort<T>(this IList<T> collection, ISortMethod<T> sortMethod) where T : IComparable, IComparable<T>
        {
            Console.WriteLine("{0}: start", sortMethod.GetType().Name);
            sortMethod.Sort(collection);
            Console.WriteLine("{0}: end", sortMethod.GetType().Name);
        }
    }
}