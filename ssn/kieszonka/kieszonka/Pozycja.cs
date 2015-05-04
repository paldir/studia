using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kieszonka
{
    class Pozycja
    {
        public double[] X { get; set; }

        int _klasa;
        public int Klasa
        {
            get { return _klasa; }

            set
            {
                if (Math.Abs(value) != 1)
                    throw new Exception("Klasa może przyjmować wartości -1 lub 1.");

                _klasa = value;
            }
        }

        public Pozycja() { }

        public Pozycja(IEnumerable<double> x, int klasa)
        {
            X = x.ToArray();
            Klasa = klasa;
        }
    }
}