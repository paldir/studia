using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace plecak
{
    class Program
    {
        float _V;
        List<Przedmiot> _przedmioty;

        float Lowerbound(IEnumerable<Przedmiot> J, IEnumerable<Przedmiot> K)
        {
            float x = _V - J.Sum(p => p.w);
            float y = J.Sum(p => p.p);

            for (int i = 0; i < _przedmioty.Count(); i++)
            {
                Przedmiot przedmiot = _przedmioty.ElementAt(i);
                IEnumerable<Przedmiot> sumaJK = Enumerable.Union(J, K);
                float wagaPrzedmiotu = przedmiot.w;

                if (!(sumaJK.Count() == 1 && sumaJK.Contains(przedmiot)) && wagaPrzedmiotu <= x)
                {
                    J = Enumerable.Union(J, new List<Przedmiot>() { przedmiot });
                    x -= wagaPrzedmiotu;
                    y += przedmiot.p;
                }
            }

            return y;
        }

        float KNAP(int k, out List<Przedmiot> plecak)
        {
            float Q = 0;
            plecak = null;
            List<List<Przedmiot>> podzbiory = new List<List<Przedmiot>>();

            foreach (List<Przedmiot> podzbiór in podzbiory)
            {
                float lowerbound = Lowerbound(podzbiór, new List<Przedmiot>());

                if (lowerbound > Q)
                {
                    Q = lowerbound;
                    plecak = podzbiór;
                }
            }

            return Q;
        }

        static void Main(string[] args)
        {

        }
    }
}