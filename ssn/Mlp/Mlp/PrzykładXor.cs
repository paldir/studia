using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mlp
{
    struct PrzykładXor
    {
        int _x1;
        public int X1
        {
            get
            {
                return _x1;
            }

            set
            {
                if (value == -1 || value == 1)
                    _x1 = value;
                else
                    throw new Exception("Dozwolone wartości to -1 i 1.");
            }
        }

        int _x2;
        public int X2
        {
            get
            {
                return _x2;
            }

            set
            {
                if (value == -1 || value == 1)
                    _x2 = value;
                else
                    throw new Exception("Dozwolone wartości to -1 i 1.");
            }
        }

        public int Wynik
        {
            get
            {
                return X1 == X2 ? -1 : 1;
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