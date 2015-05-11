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
        public int Kwadrat { get; set; }

        public string KolorySąsiadów
        {
            get
            {
                System.Text.StringBuilder kolory = new StringBuilder();

                foreach (Wierzchołek sąsiad in Sąsiedzi)
                    kolory.Append(String.Format("{0} ", sąsiad));

                return kolory.ToString();
            }
        }

        public Wierzchołek(int kolor, int kwadrat)
        {
            Kolor = kolor;
            Kwadrat = kwadrat;
            Sąsiedzi = new List<Wierzchołek>();
        }
    }
}