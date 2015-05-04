using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komiwojażer
{
    class Węzeł
    {
        public double[,] Odległości { get; set; }
        public List<Węzeł> Dzieci { get; set; }

        public Węzeł(double[,] odległości)
        {
            for (int i = 0; i < odległości.GetLength(0); i++)
                for (int j = 0; j < odległości.GetLength(1); j++)
                    Odległości[i, j] = odległości[i, j];
        }
    }
}