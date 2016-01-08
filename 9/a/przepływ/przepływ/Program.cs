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
            List<Wierzchołek> graf = UtwórzGraf();
            float przepływ = EdmondsKarp(graf, graf.First(), graf.Last());

            Console.Write(przepływ);
        }

        static float EdmondsKarp(List<Wierzchołek> graf, Wierzchołek źródło, Wierzchołek ujście)
        {
            float maksymalnyPrzepływ = 0;

            while (true)
            {
                float m = Bfs(graf, źródło, ujście);

                if (Math.Abs(m) < Single.Epsilon)
                    break;

                maksymalnyPrzepływ += m;
                Wierzchołek v = ujście;

                while (v != źródło)
                {
                    Wierzchołek u = v.Rodzic;
                    Krawędź uv = u.Krawędzie.Single(k => k.Wierzchołek == v);
                    Krawędź vu = v.Krawędzie.Single(k => k.Wierzchołek == u);
                    uv.Przepływ += m;
                    vu.Przepływ -= m;
                    v = u;
                }
            }

            return maksymalnyPrzepływ;
        }

        static float Bfs(IEnumerable<Wierzchołek> graf, Wierzchołek źródło, Wierzchołek ujście)
        {
            int liczbaWierzchołków = graf.Count();
            Dictionary<int, float> m = new Dictionary<int, float>(liczbaWierzchołków);
            m[źródło.Nazwa] = Single.PositiveInfinity;
            Queue<Wierzchołek> q = new Queue<Wierzchołek>();

            q.Enqueue(źródło);

            while (q.Any())
            {
                Wierzchołek u = q.Dequeue();

                foreach (Krawędź krawędź in u.Krawędzie)
                {
                    Wierzchołek v = krawędź.Wierzchołek;

                    if (krawędź.Pojemność - krawędź.Przepływ > 0 && v.Rodzic == null && v != źródło)
                    {
                        v.Rodzic = u;
                        m[v.Nazwa] = Math.Min(m[u.Nazwa], krawędź.Pojemność - krawędź.Przepływ);

                        if (v != ujście)
                            q.Enqueue(v);
                        else
                        {
                            float przepływZnalezionejŚcieżki = m[ujście.Nazwa];

                            return przepływZnalezionejŚcieżki;
                        }
                    }
                }
            }

            return 0;
        }

        /*static void EdmondsKarp(float[,] C, int[][] E, int s, int t, out float f, out float[,] F)
{
    int rozmiar = C.GetLength(0);
    f = 0;
    F = new float[rozmiar, rozmiar];

    while (true)
    {
        float m;
        int[] P;

        BFS(C, E, s, t, F, out m, out P);

        if (m == 0)
            break;

        f += m;
        int v = t;

        while (v != s)
        {
            int u = P[v];
            F[u, v] += m;
            F[u, v] = -m;
            v = u;
        }
    }
}*/

        /*static void BFS(float[,] C, int[][] E, int s, int t, float[,] F, out float m, out int[] P)
        {
            int liczbaWierzchołków = C.GetLength(0);
            P = new int[liczbaWierzchołków];

            for (int u = 0; u < liczbaWierzchołków; u++)
                P[u] = -1;

            P[s] = -2;
            float[] M = new float[liczbaWierzchołków];
            M[s] = Single.PositiveInfinity;
            Queue<int> Q = new Queue<int>();

            Q.Enqueue(s);

            while (Q.Any())
            {
                int u = Q.Dequeue();

                foreach (int v in E[u])
                    if (C[u, v] - F[u, v] > 0 && P[v] == -1)
                    {
                        P[v] = u;
                        M[v] = Math.Min(M[u], C[u, v] - F[u, v]);

                        if (v != t)
                            Q.Enqueue(v);
                        else
                        {
                            m = M[t];

                            return;
                        }
                    }
            }

            m = 0;
        }*/

        static List<Wierzchołek> UtwórzGraf()
        {
            IEnumerable<string> linie = File.ReadAllLines("graf.txt").Where(l => !l.StartsWith("#"));
            List<Wierzchołek> graf = new List<Wierzchołek>();

            foreach (string linia in linie)
            {
                string[] zawartość = linia.Split(' ');
                char nazwaPierwszego = Convert.ToChar(zawartość[0]);
                float przepływ = Convert.ToSingle(zawartość[1]);
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

                pierwszy.Krawędzie.Add(new Krawędź(drugi, przepływ));
                drugi.Krawędzie.Add(new Krawędź(pierwszy, -przepływ));
            }

            graf.Sort((x, y) => x.Nazwa - y.Nazwa);

            foreach (Wierzchołek w in graf)
                w.Krawędzie.Sort((x, y) => x.Wierzchołek.Nazwa - y.Wierzchołek.Nazwa);

            return graf;
        }
    }
}