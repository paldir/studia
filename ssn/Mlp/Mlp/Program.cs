using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mlp
{
    class Program
    {
        const double Eta = 1;

        static void Main(string[] args)
        {
            int liczbaIteracji;
            Random los = new Random();
            int[] wartości = new int[] { -1, 1 };
            List<PrzykładXor> przykłady = new List<PrzykładXor>();
            List<double[,]> wagi = new List<double[,]>()
            {
                new double[3, 2],
                new double[2, 1]
            };

            int liczbaWarstw = wagi.Count;

            foreach (double[,] warstwa in wagi)
                for (int i = 0; i < warstwa.GetLength(0); i++)
                    for (int j = 0; j < warstwa.GetLength(1); j++)
                        warstwa[i, j] = los.NastępnaMałaLiczba();

            using (System.IO.StreamReader strumień = new System.IO.StreamReader("dane.txt"))
                while (!strumień.EndOfStream)
                {
                    string[] linia = strumień.ReadLine().Split(' ', '\t');
                    PrzykładXor przykład = new PrzykładXor(Convert.ToInt32(linia[0]), Convert.ToInt32(linia[1]));

                    przykłady.Add(przykład);
                }

            /*foreach (int wartość1 in wartości)
                foreach (int wartość2 in wartości)
                    przykłady.Add(new PrzykładXor(wartość1, wartość2));*/

            Console.Write("Podaj liczbę iteracji: ");

            liczbaIteracji = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine();

            for (int i = 0; i < liczbaIteracji; i++)
            {
                PrzykładXor przykład = przykłady[los.Next(0, przykłady.Count)];
                List<double[]> Z;
                List<double[]> O;
                List<double[]> Delty = new List<double[]>();

                PrzepuśćPrzezSieć(wagi, przykład, out Z, out O);
                Delty.Add(new double[] { (O.Last().Single() - przykład.Wynik) * FPrim(Z.Last().Single()) });

                for (int l = liczbaWarstw - 2; l >= 0; l--)
                {
                    int liczbaNeuronów = wagi[l].GetLength(1);
                    double[] delty = new double[liczbaNeuronów];
                    double[] deltyKolejnejWarstwy = Delty.First();

                    for (int numerNeuronu = 0; numerNeuronu < liczbaNeuronów; numerNeuronu++)
                    {
                        double delta = 0;

                        for (int numerNeuronuKolejnejWarstwy = 0; numerNeuronuKolejnejWarstwy < deltyKolejnejWarstwy.Length; numerNeuronuKolejnejWarstwy++)
                            delta += deltyKolejnejWarstwy[numerNeuronuKolejnejWarstwy] * wagi[l + 1][numerNeuronu, numerNeuronuKolejnejWarstwy] * FPrim(Z[l][numerNeuronu]);

                        delty[numerNeuronu] = delta;
                    }

                    Delty.Insert(0, delty);
                }

                for (int l = 0; l < wagi.Count; l++)
                {
                    double[,] wagiDanejWarstwy = wagi[l];

                    for (int numerWejścia = 0; numerWejścia < wagiDanejWarstwy.GetLength(0); numerWejścia++)
                        for (int numerWyjścia = 0; numerWyjścia < wagiDanejWarstwy.GetLength(1); numerWyjścia++)
                        {
                            double deltaW = -Eta * Delty[l][numerWyjścia] * O[l][numerWejścia];
                            wagiDanejWarstwy[numerWejścia, numerWyjścia] += deltaW;
                        }
                }
            }

            foreach (PrzykładXor przykład in przykłady)
            {
                List<double[]> Z;
                List<double[]> O;
                double wynik = PrzepuśćPrzezSieć(wagi, przykład, out Z, out O);

                Console.WriteLine("{0,2} XOR {1,2} = {2}", przykład.X1, przykład.X2, wynik);
            }

            Console.ReadKey();
        }

        static double PrzepuśćPrzezSieć(List<double[,]> wagi, PrzykładXor przykład, out List<double[]> Z, out List<double[]> O)
        {
            int liczbaWarstw = wagi.Count;
            double[] x = new double[] { przykład.X1, przykład.X2, 1 };
            Z = new List<double[]>();
            O = new List<double[]> { x };

            for (int l = 0; l < liczbaWarstw; l++)
            {
                double[,] wagiWarstwy = wagi[l];
                int liczbaWejść = wagiWarstwy.GetLength(0);
                int liczbaWyjść = wagiWarstwy.GetLength(1);
                double[] z = new double[liczbaWyjść];
                double[] o = new double[liczbaWyjść];
                double[] wejście = O.Last();

                for (int numerWyjścia = 0; numerWyjścia < liczbaWyjść; numerWyjścia++)
                {
                    double zTmp = 0;

                    for (int numerWejścia = 0; numerWejścia < liczbaWejść; numerWejścia++)
                        zTmp += wagiWarstwy[numerWejścia, numerWyjścia] * wejście[numerWejścia];

                    z[numerWyjścia] = zTmp;
                    o[numerWyjścia] = F(zTmp);
                }

                Z.Add(z);
                O.Add(o);
            }

            return O.Last().Single();
        }

        static double F(double value)
        {
            return Math.Tanh(value);
            //return 1 / (1 + Math.Exp(-value));
        }

        static double FPrim(double value)
        {
            return (4 * Math.Pow(Math.Cosh(value), 2)) / Math.Pow(Math.Cosh(2 * value) + 1, 2);
            //return Math.Exp(value) / Math.Pow(Math.Exp(value) + 1, 2);
            //return F(value) * (1 - F(value));
        }
    }
}