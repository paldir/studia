using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorytmWęgierski
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Wierzchołek> graf = new List<Wierzchołek>();

            for (int i = 1; i <= 3; i++)
                graf.Add(new Wierzchołek(i, 1));

            for (int i = 4; i <= 6; i++)
                graf.Add(new Wierzchołek(i, 2));

            List<Wierzchołek> v1 = graf.FindAll(w => w.ZbiórWGrafieDwudzielnym == 1);
            List<Wierzchołek> v2 = graf.FindAll(w => w.ZbiórWGrafieDwudzielnym == 2);
            int[,] krawędzie = new int[,]
            {
                {1, 6, 1},
                {1, 4, 4},
                {2, 6, 6},
                {2, 5, 8},
                {3, 5, 6},
                {3, 4, 1}
            };

            for (int i = 0; i < krawędzie.GetLength(0); i++)
            {
                Wierzchołek wierzchołek1 = graf.Find(w => w.Nazwa == krawędzie[i, 0]);
                Wierzchołek wierzchołek2 = graf.Find(w => w.Nazwa == krawędzie[i, 1]);

                wierzchołek1.Sąsiedzi.Add(new Sąsiad(wierzchołek2, krawędzie[i, 2]));
                wierzchołek2.Sąsiedzi.Add(new Sąsiad(wierzchołek1, krawędzie[i, 2]));
            }

            foreach (Wierzchołek wierzchołek in v1)
                wierzchołek.Etykieta = wierzchołek.Sąsiedzi.Max(s => s.Waga);

            foreach (Wierzchołek wierzchołek in v2)
                wierzchołek.Etykieta = 0;

            foreach (Wierzchołek wierzchołek in v1)
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
    }
}