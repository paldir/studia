using System;
using System.Collections.Generic;
using System.Text;

namespace przepływ
{
    class Wierzchołek
    {
        int _nazwa;

        public int Nazwa
        {
            get { return _nazwa; }

            set
            {
                _nazwa = value;
                Litera = Convert.ToChar(Nazwa + 64);
            }
        }

        public char Litera { get; private set; }
        public List<Krawędź> Krawędzie { get; set; }
        public Krawędź Pred { get; set; }

        public string Informacje
        {
            get
            {
                StringBuilder budowniczy = new StringBuilder();

                foreach (Krawędź krawędź in Krawędzie)
                    budowniczy.AppendFormat("{0} ", krawędź.Informacje);

                return budowniczy.ToString();
            }
        }

        public Wierzchołek(int nazwa)
        {
            Nazwa = nazwa;
            Krawędzie = new List<Krawędź>();
        }

        public Wierzchołek(char litera)
            : this(Convert.ToInt32(litera) - 64)
        {
        }
    }
}