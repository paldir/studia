using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mlp
{
    struct PrzykładXor
    {
        public int X1 { get; set; }
        public int X2 { get; set; }

        public int Wynik
        {
            get
            {
                return X1 == X2 ? 0 : 1;
            }
        }

        public PrzykładXor(int x1, int x2)
            : this()
        {
            X1 = x1;
            X2 = x2;
        }
    }
}