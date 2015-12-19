using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kieszonka
{
    public class Kieszonka
    {
        public int Wiek { get; set; }
        public double[] W { get; private set; }
        public double B { get; set; }

        public Kieszonka(int wiek, double[] w, double b)
        {
            Wiek = wiek;
            W = new double[w.Length];
            B = b;

            Array.Copy(w, W, w.Length);
        }
    }
}