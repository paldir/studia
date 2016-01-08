using System;

namespace przepływ
{
    class Krawędź
    {
        public Wierzchołek S { get; private set; }
        public Wierzchołek T { get; private set; }
        public float Flow { get; set; }
        public float Cap { get; private set; }
        public Krawędź Rev { get; set; }

        public string Informacje
        {
            get { return String.Format("{0}->{1}:{2}/{3}", S.Litera, T.Litera, Flow, Cap); }
        }

        public Krawędź(Wierzchołek s, Wierzchołek koniec, float cap)
        {
            S = s;
            T = koniec;
            Cap = cap;
        }
    }
}