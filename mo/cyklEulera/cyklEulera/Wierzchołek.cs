using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cyklEulera
{
    class Wierzchołek
    {
        public int Nazwa { get; private set; }

        public List<int> Sąsiedzi { get; private set; }

        public Wierzchołek(int nazwa, params int[] sąsiedzi)
        {
            Nazwa = nazwa;
            Sąsiedzi = new List<int>();

            foreach (int sąsiad in sąsiedzi)
                Sąsiedzi.Add(sąsiad);
        }

        public Wierzchołek(int nazwa, IEnumerable<int> sąsiedzi) : this(nazwa, sąsiedzi.ToArray()) { }
    }
}