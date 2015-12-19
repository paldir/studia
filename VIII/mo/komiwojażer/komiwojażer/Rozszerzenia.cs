using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komiwojażer
{
    public static class Rozszerzenia
    {
        public static double ToRadians(this double stopnie)
        {
            return Math.PI / 180 * stopnie;
        }
    }
}