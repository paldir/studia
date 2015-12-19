using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komiwojażer
{
    class Węzeł
    {
        public double[,] Odległości { get; set; }
        public List<Węzeł> Dzieci { get; set; }
        public Węzeł Rodzic { get; set; }
        public double LB { get; set; }
        public List<ElementZerowy> ElementyZerowe { get; set; }
        public List<ElementZerowy> WybraneRozwiązania { get; set; }

        public Węzeł(double[,] odległości, double lB, Węzeł rodzic = null)
        {
            int wiersze = odległości.GetLength(0);
            int kolumny = odległości.GetLength(1);
            Odległości = new double[wiersze, kolumny];
            Rodzic = rodzic;
            LB = lB;
            Dzieci = new List<Węzeł>();
            ElementyZerowe = new List<ElementZerowy>();

            if (rodzic == null)
                WybraneRozwiązania = new List<ElementZerowy>();
            else
                WybraneRozwiązania = new List<ElementZerowy>(rodzic.WybraneRozwiązania);

            for (int i = 0; i < wiersze; i++)
                for (int j = 0; j < kolumny; j++)
                {
                    double element = odległości[i, j];

                    Odległości[i, j] = element;

                    if (element == 0)
                        ElementyZerowe.Add(new ElementZerowy(i, j));
                }
        }

        public Węzeł(double[,] odległości, double lB, ElementZerowy rozwiązanie, Węzeł rodzic = null)
            : this(odległości, lB, rodzic)
        {
            WybraneRozwiązania.Add(rozwiązanie);
        }
    }
}