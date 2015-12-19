using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tarzan
{
    class ParaWęzłów
    {
        public Węzeł A { get; set; }
        public Węzeł B { get; set; }

        public static Węzeł Korzeń { get; set; }

        public ParaWęzłów(string a, string b)
        {
            A = Szukaj(a, Korzeń);
            B = Szukaj(b, Korzeń);
        }

        static Węzeł Szukaj(string nazwa, Węzeł węzeł)
        {
            if (węzeł.Nazwa == nazwa)
                return węzeł;

            foreach (Węzeł dziecko in węzeł.Dzieci)
            {
                Węzeł wynik = Szukaj(nazwa, dziecko);

                if (wynik != null)
                    return wynik;
            }

            return null;
        }
    }
}