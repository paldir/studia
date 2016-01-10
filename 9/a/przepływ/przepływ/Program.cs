using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace przepływ
{
    class Program
    {
        static void Main()
        {
            Wierzchołek źródło;
            Wierzchołek ujście;
            List<Wierzchołek> graf = UtwórzGraf(out źródło, out ujście);
            float przepływ = EdmondsKarp(graf, źródło, ujście);

            Console.Write(przepływ);
        }

        static float EdmondsKarp(List<Wierzchołek> graf, Wierzchołek s, Wierzchołek t)
        {
            float flow = 0;

            do
            {
                Queue<Wierzchołek> q = new Queue<Wierzchołek>();

                q.Enqueue(s);
                graf.ForEach(w => w.Pred = null);

                while (q.Any())
                {
                    Wierzchołek cur = q.Dequeue();

                    foreach (Krawędź e in cur.DostępneKrawędzie)
                        if (e.T.Pred == null && e.T != s && e.Cap > e.Flow)
                        {
                            e.T.Pred = e;

                            q.Enqueue(e.T);
                        }
                }

                if (t.Pred == null)
                    break;

                float df = Single.PositiveInfinity;

                for (Krawędź e = t.Pred; e != null; e = e.S.Pred)
                    df = Math.Min(df, e.Cap - e.Flow);

                for (Krawędź e = t.Pred; e != null; e = e.S.Pred)
                {
                    e.Flow += df;
                    e.Rev.Flow -= df;
                }

                flow += df;
            } while (true);

            return flow;
        }

        static List<Wierzchołek> UtwórzGraf(out Wierzchołek źródło, out Wierzchołek ujście)
        {
            string[] linie = File.ReadAllLines("graf.txt").Where(l => !l.StartsWith("#")).ToArray();
            List<Wierzchołek> graf = new List<Wierzchołek>();
            int liczbaLinii = linie.Length;
            string[] pierwszaLinia = linie[0].Split(' ');

            for (int i = 1; i < liczbaLinii; i++)
            {
                string[] zawartość = linie[i].Split(' ');
                char nazwaPierwszego = Convert.ToChar(zawartość[0]);
                float pojemność = Convert.ToSingle(zawartość[1]);
                char nazwaDrugiego = Convert.ToChar(zawartość[2]);
                Wierzchołek pierwszy = graf.SingleOrDefault(w => w.Litera == nazwaPierwszego);
                Wierzchołek drugi = graf.SingleOrDefault(w => w.Litera == nazwaDrugiego);

                if (pierwszy == null)
                {
                    pierwszy = new Wierzchołek(nazwaPierwszego);

                    graf.Add(pierwszy);
                }

                if (drugi == null)
                {
                    drugi = new Wierzchołek(nazwaDrugiego);

                    graf.Add(drugi);
                }

                Krawędź krawędź = new Krawędź(pierwszy, drugi, pojemność, true);
                Krawędź krawędźOdwrotna = new Krawędź(drugi, pierwszy, pojemność, false);
                krawędź.Rev = krawędźOdwrotna;
                krawędźOdwrotna.Rev = krawędź;

                pierwszy.Krawędzie.Add(krawędź);
                drugi.Krawędzie.Add(krawędźOdwrotna);
            }

            źródło = graf.Single(w => w.Litera == Convert.ToChar(pierwszaLinia[0]));
            ujście = graf.Single(w => w.Litera == Convert.ToChar(pierwszaLinia[1]));

            graf.Sort((x, y) => x.Nazwa - y.Nazwa);

            foreach (Wierzchołek wierzchołek in graf)
                wierzchołek.Krawędzie = wierzchołek.Krawędzie.OrderBy(k => k.T.Nazwa).ToList();

            return graf;
        }
    }
}