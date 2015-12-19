using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku
{
    class Wierzchołek
    {
        public int Kolor { get; set; }
        public List<Wierzchołek> Sąsiedzi { get; set; }
        public int Kwadrat { get; private set; }
        public int I { get; private set; }
        public int J { get; private set; }

        public string KolorySąsiadów
        {
            get
            {
                System.Text.StringBuilder kolory = new StringBuilder();

                foreach (Wierzchołek sąsiad in Sąsiedzi)
                    kolory.AppendFormat("{0} ", sąsiad.Kolor);

                return kolory.ToString();
            }
        }

        public Wierzchołek(int kolor, int kwadrat, int i, int j)
        {
            Kolor = kolor;
            Kwadrat = kwadrat;
            I = i;
            J = j;
            Sąsiedzi = new List<Wierzchołek>();
        }
    }
}