using System;
using System.Collections.Generic;
using System.Text;

namespace silnieSpójneSkładowe
{
    class Wierzchołek
    {
        public List<Wierzchołek> Sąsiedzi { get; set; }
        public string Nazwa { get; private set; }
        public bool Odwiedzony { get; private set; }
        public int CzasPrzetworzenia { get; set; }
        public bool Negacja { get; private set; }
        public string NazwaLogiczna { get; private set; }
        public Nullable<bool> WartośćLogiczna { get; set; }

        int _czasOdwiedzenia;
        public int CzasOdwiedzenia
        {
            get { return _czasOdwiedzenia; }

            set
            {
                if (value == 0)
                    Odwiedzony = false;
                else
                {
                    if (Odwiedzony)
                        throw new Exception("Wierzchołek był już odwiedzony.");

                    Odwiedzony = true;
                }

                _czasOdwiedzenia = value;
            }
        }

        public string NazwySąsiadów
        {
            get
            {
                StringBuilder budowniczyNapisu = new StringBuilder();

                foreach (Wierzchołek sąsiad in Sąsiedzi)
                    budowniczyNapisu.AppendFormat("{0} ", sąsiad.NazwaLogiczna);

                return budowniczyNapisu.ToString();
            }
        }

        public Wierzchołek(string nazwa)
        {
            Nazwa = nazwa;
            Sąsiedzi = new List<Wierzchołek>();
        }

        public Wierzchołek(string nazwa, bool negacja)
            : this(nazwa)
        {
            Negacja = negacja;
            NazwaLogiczna = String.Concat(Negacja ? "~" : String.Empty, Nazwa);
        }
    }
}