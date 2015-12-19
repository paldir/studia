using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komiwojażer
{
    class ElementZerowy
    {
        public int I { get; set; }
        public int J { get; set; }
        public double PoprzedniaWartość { get; set; }

        public ElementZerowy(int i, int j)
        {
            I = i;
            J = j;
        }

        public ElementZerowy(int i, int j, double poprzedniaWartość)
            : this(i, j)
        {
            PoprzedniaWartość = poprzedniaWartość;
        }
    }
}