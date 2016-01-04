using System.Collections.Generic;

namespace skoczek
{
    class Pole
    {
        public int P;
        public int Q;
        public List<Pole> Sąsiedzi;
        public int IlośćWolnychSąsiadów;
        public int NumerKroku;

        public Pole(int p, int q)
        {
            P = p;
            Q = q;
        }
    }
}