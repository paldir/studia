using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorytmWęgierski
{
    class Program
    {
        static readonly int[,] krawędzie = new int[,]
            {
                {1, 6, 1},
                {1, 4, 4},
                {2, 6, 6},
                {2, 5, 8},
                {3, 5, 6},
                {3, 4, 1}
            };

        static void OdbudujKrawędzie(List<Wierzchołek> graf)
        {
            for (int i = 0; i < krawędzie.GetLength(0); i++)
            {
                Wierzchołek wierzchołek1 = graf.Find(w => w.Nazwa == krawędzie[i, 0]);
                Wierzchołek wierzchołek2 = graf.Find(w => w.Nazwa == krawędzie[i, 1]);

                wierzchołek1.Sąsiedzi.Add(new Sąsiad(wierzchołek2, krawędzie[i, 2]));
                wierzchołek2.Sąsiedzi.Add(new Sąsiad(wierzchołek1, krawędzie[i, 2]));
            }
        }

        static void UtwórzGrafRówny(List<Wierzchołek> graf)
        {
            foreach (Wierzchołek wierzchołek in graf.Where(w => w.ZbiórWGrafieDwudzielnym == 1))
            {
                List<Sąsiad> sąsiedziDoUsunięcia = wierzchołek.Sąsiedzi.FindAll(s => s.Waga != wierzchołek.Etykieta);

                foreach (Sąsiad sąsiad in sąsiedziDoUsunięcia)
                {
                    List<Sąsiad> sąsiedziSąsiada = sąsiad.Wierzchołek.Sąsiedzi;

                    wierzchołek.Sąsiedzi.Remove(sąsiad);
                    sąsiedziSąsiada.Remove(sąsiedziSąsiada.Find(s => s.Wierzchołek == wierzchołek));
                }
            }
        }
        
        static void Main(string[] args)
        {
            List<Wierzchołek> graf = new List<Wierzchołek>();

            for (int i = 1; i <= 3; i++)
                graf.Add(new Wierzchołek(i, 1));

            for (int i = 4; i <= 6; i++)
                graf.Add(new Wierzchołek(i, 2));

            bool pierwszyKrok = true;
            List<Wierzchołek> v1 = graf.FindAll(w => w.ZbiórWGrafieDwudzielnym == 1);
            List<Wierzchołek> v2 = graf.FindAll(w => w.ZbiórWGrafieDwudzielnym == 2);

            while (true)
            {
                if (pierwszyKrok)
                {
                    OdbudujKrawędzie(graf);

                    foreach (Wierzchołek wierzchołek in v1)
                        wierzchołek.Etykieta = wierzchołek.Sąsiedzi.Max(s => s.Waga);

                    foreach (Wierzchołek wierzchołek in v2)
                        wierzchołek.Etykieta = 0;

                    UtwórzGrafRówny(graf);
                    SkojarzeniePoczątkowe(graf);
                }

                if (!graf.Exists(w => w.SkojarzonyZ == null))
                    break;

                Wierzchołek u = v1.Where(w => w.Sąsiedzi.Any()).First(w => w.SkojarzonyZ == null);
                List<Wierzchołek> S = new List<Wierzchołek>() { u };
                List<Wierzchołek> T = new List<Wierzchołek>();
                List<Wierzchołek> sąsiedziS = new List<Wierzchołek>();

                foreach (Wierzchołek wierzchołek in S)
                    sąsiedziS.AddRange(wierzchołek.Sąsiedzi.Select(s => s.Wierzchołek));

                sąsiedziS = sąsiedziS.Distinct().OrderBy(w => w.Nazwa).ToList();
                T = T.Distinct().OrderBy(w => w.Nazwa).ToList();

                if (sąsiedziS.Count == T.Count && Enumerable.SequenceEqual(sąsiedziS, T))
                {
                    List<int> alfy = new List<int>();

                    foreach (Wierzchołek x in S)
                        foreach (Sąsiad sąsiadY in x.Sąsiedzi.Where(s => !T.Contains(s.Wierzchołek)))
                            alfy.Add(x.Etykieta + sąsiadY.Wierzchołek.Etykieta - sąsiadY.Waga);

                    int alfa = alfy.Min();

                    foreach (Wierzchołek wierzchołek in graf)
                        if (S.Contains(wierzchołek))
                            wierzchołek.Etykieta -= alfa;
                        else
                            if (T.Contains(wierzchołek))
                                wierzchołek.Etykieta += alfa;

                    OdbudujKrawędzie(graf);
                    UtwórzGrafRówny(graf);
                }

                Wierzchołek y = sąsiedziS.Except(T).First();

                pierwszyKrok = y.SkojarzonyZ != null;
                
                if (!pierwszyKrok)
                {

                }
                else
                {
                    Wierzchołek z = y.SkojarzonyZ;

                    S.Add(z);
                    T.Add(y);
                }
            }
        }

        static void SkojarzeniePoczątkowe(List<Wierzchołek> graf)
        {
            foreach (Wierzchołek wierzchołek in graf)
                wierzchołek.SkojarzonyZ = null;

            foreach (Wierzchołek wierzchołek in graf)
                if (wierzchołek.SkojarzonyZ == null)
                {
                    Wierzchołek nieskojarzonySąsiad = wierzchołek.Sąsiedzi.Select(s => s.Wierzchołek).FirstOrDefault(s => s.SkojarzonyZ == null);

                    if (nieskojarzonySąsiad != null)
                    {
                        wierzchołek.SkojarzonyZ = nieskojarzonySąsiad;
                        nieskojarzonySąsiad.SkojarzonyZ = wierzchołek;
                    }
                }
        }
    }
}