using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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