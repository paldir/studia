using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace plecak
{
    class Program
    {
        static float _V;
        static List<Przedmiot> _przedmioty;

        static float Lowerbound(ref IEnumerable<Przedmiot> J, IEnumerable<Przedmiot> K)
        {
            float x = _V - J.Sum(p => p.w);
            float y = J.Sum(p => p.p);

            for (int i = 0; i < _przedmioty.Count(); i++)
            {
                Przedmiot przedmiot = _przedmioty.ElementAt(i);
                IEnumerable<Przedmiot> sumaJK = Enumerable.Union(J, K);
                float wagaPrzedmiotu = przedmiot.w;

                if (!sumaJK.Contains(przedmiot) && wagaPrzedmiotu <= x)
                {
                    J = Enumerable.Union(J, new List<Przedmiot>() { przedmiot });
                    x -= wagaPrzedmiotu;
                    y += przedmiot.p;
                }
            }

            return y;
        }

        static float KNAP(int k, out IEnumerable<Przedmiot> plecak)
        {
            float Q = 0;
            plecak = null;
            List<List<Przedmiot>> podzbiory = new List<List<Przedmiot>>();
            int liczbaPrzedmiotów = _przedmioty.Count;

            for (int i = 1; i < Math.Pow(liczbaPrzedmiotów, 2); i++)
            {
                IEnumerable<bool> maska = Convert.ToString(i, 2).ToCharArray().Select(z => z == '1');

                if (maska.Count(b => b) <= k)
                {
                    List<Przedmiot> podzbiór = new List<Przedmiot>();
                    int rozmiarMaski = maska.Count();

                    for (int j = rozmiarMaski - 1; j >= 0; j--)
                        if (maska.ElementAt(j))
                            podzbiór.Add(_przedmioty[rozmiarMaski - j - 1]);

                    if (podzbiór.Sum(p => p.w) <= _V)
                        podzbiory.Add(podzbiór);
                }
            }

            for (int i = 0; i < podzbiory.Count; i++)
            {
                IEnumerable<Przedmiot> podzbiór = podzbiory[i];
                float lowerbound = Lowerbound(ref podzbiór, new List<Przedmiot>());

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
            IEnumerable<Przedmiot> plecak;
            _przedmioty = new List<Przedmiot>();
            int k = Int32.Parse(System.IO.File.ReadAllText("k.txt"));
            _V = Int32.Parse(System.IO.File.ReadAllText("pojemność.txt"));

            using (System.IO.StreamReader strumień = new System.IO.StreamReader("przedmioty.txt"))
                while (!strumień.EndOfStream)
                {
                    IEnumerable<int> linia = strumień.ReadLine().Split('\t').Select(p => Int32.Parse(p));

                    _przedmioty.Add(new Przedmiot(linia.First(), linia.Last()));
                }

            _przedmioty = _przedmioty.OrderByDescending(p => p.p / p.w).ToList();

            KNAP(k, out plecak);

            foreach (Przedmiot przedmiot in plecak)
                Console.WriteLine("w: {0}\tp: {1}", przedmiot.w, przedmiot.p);

            Console.WriteLine("sw: {0}\tsp: {1}", plecak.Sum(p => p.w), plecak.Sum(p => p.p));
            Console.ReadKey();
        }
    }
}