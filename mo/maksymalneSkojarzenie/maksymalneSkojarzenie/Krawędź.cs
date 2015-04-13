using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace maksymalneSkojarzenie
{
    struct Krawędź
    {
        public int Wierzchołek1 { get; private set; }
        public int Wierzchołek2 { get; private set; }

        public Krawędź(int wierzchołek1, int wierzchołek2)
            : this()
        {
            List<int> wierzchołki = new List<int>() { wierzchołek1, wierzchołek2 };
            Wierzchołek1 = wierzchołki.Min();
            Wierzchołek2 = wierzchołki.Max();
        }
    }
}