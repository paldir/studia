using System.Collections.Generic;
using System.Text;

namespace mosty
{
    class Wierzchołek
    {
        public List<Wierzchołek> Sąsiedzi { get; set; }
        public string Nazwa { get; set; }
        public int F { get; set; }
        public int L { get; set; }
        public bool Odwiedzony { get; set; }
        public Wierzchołek Rodzic { get; set; }

        public string NazwySąsiadów
        {
            get
            {
                StringBuilder budowniczyNapisu = new StringBuilder();

                foreach (Wierzchołek sąsiad in Sąsiedzi)
                    budowniczyNapisu.AppendFormat("{0} ", sąsiad.Nazwa);

                return budowniczyNapisu.ToString();
            }
        }

        public Wierzchołek(string nazwa)
        {
            Nazwa = nazwa;
            Sąsiedzi = new List<Wierzchołek>();
        }
    }
}