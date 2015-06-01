using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mlp
{
    public static class Rozszerzenia
    {
        public static double NastępnaMałaLiczba(this Random los)
        {
            return los.NextDouble() * 0.4 - 0.2;
        }
    }
}