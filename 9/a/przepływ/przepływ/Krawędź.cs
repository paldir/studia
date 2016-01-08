using System;

namespace przepływ
{
    class Krawędź
    {
        public Wierzchołek Wierzchołek { get; private set; }
        public float Przepływ { get; set; }
        public float Pojemność { get; private set; }

        public string Informacje
        {
            get { return String.Format("{0} {1}/{2}", Wierzchołek.Litera, Przepływ, Pojemność); }
        }

        public Krawędź(Wierzchołek wierzchołek, float pojemność)
        {
            Wierzchołek = wierzchołek;
            Pojemność = pojemność;
        }
    }
}