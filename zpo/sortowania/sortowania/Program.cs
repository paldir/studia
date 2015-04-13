using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sortowania
{
    class Program
    {
        const int length = (int)1e7;

        static uint IntegerKey(int value)
        {
            return (uint)Math.Abs(value);
        }

        static uint DoubleKey(double value)
        {
            return (uint)Math.Abs(Math.Floor(value));
        }

        static void Main(string[] args)
        {
            IList<int> array = new int[length];
            Random random = new Random();

            for (int i = 0; i < array.Count; i++)
                array[i] = random.Next(1, length * 2);

            SortWithVisualization sortWithVisualization = new SortWithVisualization(new List<System.Threading.ThreadStart>() 
            { 
                ()=>new List<int>(array).Sort(new BubbleSort<int>()),
                ()=>new List<int>(array).Sort(new CountingSort<int>(IntegerKey)),
                ()=>new List<int>(array).Sort(new HeapSort<int>()),
                ()=>new List<int>(array).Sort(new MergeSort<int>()),
                ()=>new List<int>(array).Sort(new QuickSort<int>()),
                ()=>new List<int>(array).Sort(new SelectionSort<int>()),
            }, Dummy);

            Comparison comparison = new Comparison(sortWithVisualization);

            comparison.Compare();

            Console.Write("Koniec.");
            Console.ReadKey();
        }

        static void DisplayCollection<T>(IList<T> collection)
        {
            foreach (T element in collection)
                Console.Write("{0} ", element);

            Console.WriteLine();
        }

        static void Dummy()
        { }
    }
}