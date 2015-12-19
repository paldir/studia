using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hetmani
{
    class Program
    {
        const int N = 8;
        static readonly Dictionary<int, bool> W = new Dictionary<int, bool>();
        static readonly Dictionary<int, bool> NE = new Dictionary<int, bool>();
        static readonly Dictionary<int, bool> NW = new Dictionary<int, bool>();
        static readonly Dictionary<int, int> K = new Dictionary<int, int>();

        static void Main(string[] args)
        {
            for (int i = 1; i <= N; i++)
            {
                W.Add(i, true);
                K.Add(i, 0);
            }

            for (int i = 2; i <= 2 * N; i++)
                NE.Add(i, true);

            for (int i = 1 - N; i <= N - 1; i++)
                NW.Add(i, true);

            bool s;

            Ustaw(1, out s);

            if (s)
                Wyświetl();
            else
                Console.WriteLine("Fiasko.");

            Console.ReadKey();
        }

        static void Ustaw(int k, out bool s)
        {
            int j = 0;

            do
            {
                j++;
                s = false;

                if (W[j] && NE[k + j] && NW[k - j])
                {
                    K[k] = j;
                    W[j] = NE[k + j] = NW[k - j] = false;

                    if (k == N)
                        s = true;
                    else
                    {
                        Ustaw(k + 1, out s);

                        if (!s)
                            W[j] = NE[k + j] = NW[k - j] = true;
                    }
                }
            }
            while (!(j >= N || s));
        }

        static void Wyświetl()
        {
            const int szerokość = N * 4 + 1;

            for (int j = 1; j <= szerokość; j++)
                Console.Write("-");

            Console.WriteLine();

            for (int i = 1; i <= N; i++)
            {
                Console.Write("|");

                for (int j = 1; j <= N; j++)
                    Console.Write("{0}|", K[j] == i ? " x " : "   ");

                Console.WriteLine();

                for (int j = 1; j <= szerokość; j++)
                    Console.Write("-");

                Console.WriteLine();
            }
        }
    }
}