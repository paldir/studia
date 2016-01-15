using System;

namespace przepływ
{
    internal class Krawędź
    {        
        public Wierzchołek S { get; private set; }
        public Wierzchołek T { get; private set; }
        public int Flow { get; set; }
        public int Cap { get; private set; }
        public int Residual { get; set; }
        public Krawędź Rev { get; set; }
        public bool Normal { get; private set; }

        public string Informacje
        {
            get { return String.Format("{0}->{1}:{2}/{3}", S.Litera, T.Litera, Flow, Cap); }
        }

        public Krawędź(Wierzchołek s, Wierzchołek koniec, int cap, bool normal)
        {
            S = s;
            T = koniec;
            Cap = cap;
            Normal = normal;

            if (Normal)
                Residual = Cap;
            else
                Residual = 0;
        }
    }
}