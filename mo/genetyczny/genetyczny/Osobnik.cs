using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace genetyczny
{
    class Osobnik : IComparable
    {
        public int[] Geny { get; private set; }
        public double DługośćTrasy { get; private set; }
        public double Przystosowanie
        {
            get { return 1 / DługośćTrasy; }
        }
        public double PrzystosowanieProcentowo { get; set; }

        public Osobnik(int[] geny)
        {
            Geny = geny;

            PrzeliczDługośćTrasy();
        }

        public int CompareTo(object obj)
        {
            return DługośćTrasy.CompareTo((obj as Osobnik).DługośćTrasy);
        }

        public void PrzeliczDługośćTrasy()
        {
            double odległość = 0;
            int liczbaGenów = Geny.Length;

            for (int i = 0; i < liczbaGenów - 1; i++)
                odległość += Odległości[Geny[i], Geny[i + 1]];

            odległość += Odległości[Geny[liczbaGenów - 1], Geny[0]];

            DługośćTrasy = odległość;
        }

        public static double[,] Odległości { get; set; }
    }
}