using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorytmWęgierski
{
    class Sąsiad
    {
        public Wierzchołek Wierzchołek { get; private set; }
        public int Waga { get; private set; }

        public Sąsiad(Wierzchołek wierzchołek, int waga)
        {
            Wierzchołek = wierzchołek;
            Waga = waga;
        }
    }
}