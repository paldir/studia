using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace przepływ
{
    internal class Program
    {
        private static void Main()
        {
            Wierzchołek źródło;
            Wierzchołek ujście;
            List<Krawędź> krawędzie = UtwórzGraf(out źródło, out ujście);
            int przepływ = EdmondsKarp(krawędzie, źródło, ujście);

            //Console.Write(przepływ);
        }

        private static int EdmondsKarp(List<Krawędź> krawędzie, Wierzchołek s, Wierzchołek t)
        {
            int flow = 0;

            do
            {
                Queue<Wierzchołek> q = new Queue<Wierzchołek>();

                q.Enqueue(s);
                krawędzie.ForEach(k => k.S.Pred = null);
                krawędzie.ForEach(k => k.T.Pred = null);

                while (q.Any())
                {
                    Wierzchołek cur = q.Dequeue();

                    foreach (Krawędź e in krawędzie.Where(k => k.S == cur))
                        if ((e.T.Pred == null) && (e.T != s) && (e.Cap - e.Flow > 0) && e.Residual > 0)
                        {
                            e.T.Pred = e;

                            q.Enqueue(e.T);
                        }
                }

                if (t.Pred == null)
                    break;

                int df = int.MaxValue;

                for (Krawędź e = t.Pred; e != null; e = e.S.Pred)
                    df = Math.Min(df, e.Cap - e.Flow);

                for (Krawędź e = t.Pred; e != null; e = e.S.Pred)
                {
                    e.Flow += df;
                    e.Rev.Flow -= df;
                }

                flow += df;
            }
            while (true);

            return flow;
        }

        private static List<Krawędź> UtwórzGraf(out Wierzchołek źródło, out Wierzchołek ujście)
        {
            string[] linie = File.ReadAllLines("graf.txt").Where(l => !l.StartsWith("#")).ToArray();
            List<Wierzchołek> wierzchołki = new List<Wierzchołek>();
            List<Krawędź> krawędzie = new List<Krawędź>();
            int liczbaLinii = linie.Length;
            string[] pierwszaLinia = linie[0].Split(' ');

            for (int i = 1; i < liczbaLinii; i++)
            {
                string[] zawartość = linie[i].Split(' ');
                char nazwaPierwszego = Convert.ToChar(zawartość[0]);
                int pojemność = int.Parse(zawartość[1]);
                char nazwaDrugiego = Convert.ToChar(zawartość[2]);
                Wierzchołek pierwszy = wierzchołki.SingleOrDefault(w => w.Litera == nazwaPierwszego);
                Wierzchołek drugi = wierzchołki.SingleOrDefault(w => w.Litera == nazwaDrugiego);

                if (pierwszy == null)
                {
                    pierwszy = new Wierzchołek(nazwaPierwszego);

                    wierzchołki.Add(pierwszy);
                }

                if (drugi == null)
                {
                    drugi = new Wierzchołek(nazwaDrugiego);

                    wierzchołki.Add(drugi);
                }

                Krawędź krawędź = new Krawędź(pierwszy, drugi, pojemność, true);
                Krawędź krawędźOdwrotna = new Krawędź(drugi, pierwszy, pojemność, false);
                krawędź.Rev = krawędźOdwrotna;
                krawędźOdwrotna.Rev = krawędź;

                krawędzie.Add(krawędź);
                krawędzie.Add(krawędźOdwrotna);
            }

            źródło = wierzchołki.Single(w => w.Litera == Convert.ToChar(pierwszaLinia[0]));
            ujście = wierzchołki.Single(w => w.Litera == Convert.ToChar(pierwszaLinia[1]));
            krawędzie = krawędzie.OrderBy(k => k.S.Nazwa).ThenBy(k => k.T.Nazwa).ToList();

            return krawędzie;
        }
    }
}