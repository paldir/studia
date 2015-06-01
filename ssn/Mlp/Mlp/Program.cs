using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mlp
{
    class Program
    {
        static void Main(string[] args)
        {
            Random los = new Random();
            double bias = los.NastępnaMałaLiczba();
            int[] wartości = new int[] { -1, 1 };
            List<PrzykładXor> przykłady = new List<PrzykładXor>();
            List<double[,]> wagi = new List<double[,]>()
            {
                new double[2, 2],
                new double[2, 1]
            };

            foreach (double[,] warstwa in wagi)
                for (int i = 0; i < warstwa.GetLength(0); i++)
                    for (int j = 0; j < warstwa.GetLength(1); j++)
                        warstwa[i, j] = Math.Round(los.NastępnaMałaLiczba(), 2);

            foreach (int wartość1 in wartości)
                foreach (int wartość2 in wartości)
                    przykłady.Add(new PrzykładXor(wartość1, wartość2));

            while (true)
            {
                PrzykładXor przykład = przykłady[los.Next(0, przykłady.Count)];
                double[] x = new double[] { przykład.X1, przykład.X2 };
                List<double[]> Z = new List<double[]>();
                List<double[]> O = new List<double[]>();

                for (int l = 0; l < wagi.Count; l++)
                {
                    double[,] wagiWarstwy = wagi[l];
                    int liczbaWejść = wagiWarstwy.GetLength(0);
                    int liczbaWyjść = wagiWarstwy.GetLength(1);
                    double[] o = new double[liczbaWyjść];
                    double[] z = new double[liczbaWyjść];

                    for (int numerWyjścia = 0; numerWyjścia < liczbaWyjść; numerWyjścia++)
                    {
                        double zTmp = 0;

                        for (int numerWejścia = 0; numerWejścia < liczbaWejść; numerWejścia++)
                            zTmp += wagiWarstwy[numerWejścia, numerWyjścia] * x[numerWejścia];

                        z[numerWyjścia] = zTmp;
                        o[numerWyjścia] = Math.Tanh(zTmp);
                    }

                    O.Add(o);
                    Z.Add(z);

                    x = o;
                }
            }
        }
    }
}