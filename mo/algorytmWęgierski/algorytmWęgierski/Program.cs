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
                {1, 11, 1},
                {1, 12, 6},
                {2, 12, 8},
                {2, 13, 6},
                {3, 11, 4},
                {3, 13, 1}
            };

        /*static readonly int[,] krawędzie = new int[,]
            {
                {1, 4, 4},
                {1, 6, 1},
                {2, 5, 8},
                {2, 6, 6},
                {3, 4, 1},
                {3, 5, 6}
            };*/

        static void OdbudujKrawędzie(List<Wierzchołek> graf)
        {
            for (int i = 0; i < krawędzie.GetLength(0); i++)
            {
                Wierzchołek wierzchołek1 = graf.Find(w => w.Nazwa == krawędzie[i, 0]);
                Wierzchołek wierzchołek2 = graf.Find(w => w.Nazwa == krawędzie[i, 1]);
                List<Sąsiad> sąsiedzi1 = wierzchołek1.Sąsiedzi;
                List<Sąsiad> sąsiedzi2 = wierzchołek2.Sąsiedzi;

                if (!sąsiedzi1.Exists(s => s.Wierzchołek == wierzchołek2))
                    sąsiedzi1.Add(new Sąsiad(wierzchołek2, krawędzie[i, 2]));

                if (!sąsiedzi2.Exists(s => s.Wierzchołek == wierzchołek1))
                    sąsiedzi2.Add(new Sąsiad(wierzchołek1, krawędzie[i, 2]));
            }
        }

        static void UtwórzGrafRówny(List<Wierzchołek> graf)
        {
            foreach (Wierzchołek wierzchołek in graf.Where(w => w.ZbiórWGrafieDwudzielnym == 1))
            {
                List<Sąsiad> sąsiedziDoUsunięcia = wierzchołek.Sąsiedzi.FindAll(s => s.Waga != wierzchołek.Etykieta + s.Wierzchołek.Etykieta);

                foreach (Sąsiad sąsiad in sąsiedziDoUsunięcia)
                {
                    List<Sąsiad> sąsiedziSąsiada = sąsiad.Wierzchołek.Sąsiedzi;

                    wierzchołek.Sąsiedzi.Remove(sąsiad);
                    sąsiedziSąsiada.Remove(sąsiedziSąsiada.Find(s => s.Wierzchołek == wierzchołek));
                }
            }
        }

        static List<Wierzchołek> N_l(List<Wierzchołek> graf)
        {
            List<Wierzchołek> sąsiedzi = new List<Wierzchołek>();

            foreach (Wierzchołek wierzchołek in graf)
                sąsiedzi.AddRange(wierzchołek.Sąsiedzi.Select(s => s.Wierzchołek));

            return sąsiedzi.Distinct().OrderBy(s => s.Nazwa).ToList();
        }

        static void Main(string[] args)
        {
            List<Wierzchołek> graf = new List<Wierzchołek>();

            for (int i = 1; i <= 3; i++)
                graf.Add(new Wierzchołek(i, 1));

            for (int i = 11; i <= 13; i++)
                graf.Add(new Wierzchołek(i, 2));

            bool drugiKrok = true;
            List<Wierzchołek> v1 = graf.FindAll(w => w.ZbiórWGrafieDwudzielnym == 1);
            List<Wierzchołek> v2 = graf.FindAll(w => w.ZbiórWGrafieDwudzielnym == 2);
            List<Wierzchołek> S = null;
            List<Wierzchołek> T = null;
            Wierzchołek u = null;

            OdbudujKrawędzie(graf);

            foreach (Wierzchołek wierzchołek in v1)
                wierzchołek.Etykieta = wierzchołek.Sąsiedzi.Max(s => s.Waga);

            foreach (Wierzchołek wierzchołek in v2)
                wierzchołek.Etykieta = 0;

            UtwórzGrafRówny(graf);
            SkojarzeniePoczątkowe(graf);

            while (graf.Any(w => w.SkojarzonyZ == null))
            {
                if (drugiKrok)
                {
                    u = v1.Where(w => w.Sąsiedzi.Any()).First(w => w.SkojarzonyZ == null);
                    S = new List<Wierzchołek>() { u };
                    T = new List<Wierzchołek>();
                }

                List<Wierzchołek> sąsiedziS = N_l(S);
                T = T.Distinct().OrderBy(w => w.Nazwa).ToList();

                if (sąsiedziS.Count == T.Count && Enumerable.SequenceEqual(sąsiedziS, T))
                {
                    List<int> alfy = new List<int>();

                    OdbudujKrawędzie(graf);

                    foreach (Wierzchołek x in S)
                        foreach (Sąsiad sąsiadY in x.Sąsiedzi.Where(s => !T.Contains(s.Wierzchołek)))
                            alfy.Add(x.Etykieta + sąsiadY.Wierzchołek.Etykieta - sąsiadY.Waga);

                    int alfa = alfy.Min();

                    foreach (Wierzchołek wierzchołek in S)
                        wierzchołek.Etykieta -= alfa;

                    foreach (Wierzchołek wierzchołek in T)
                        wierzchołek.Etykieta += alfa;

                    UtwórzGrafRówny(graf);
                }

                sąsiedziS = N_l(S);
                Wierzchołek y = sąsiedziS.Except(T).First();
                drugiKrok = y.SkojarzonyZ == null;

                if (drugiKrok)
                    MaksymalneSkojarzenie(graf, u);
                else
                {
                    S.Add(y.SkojarzonyZ);
                    T.Add(y);
                }
            }

            foreach (Wierzchołek wierzchołek in graf)
                wierzchołek.Sąsiedzi.RemoveAll(s => s.Wierzchołek != wierzchołek.SkojarzonyZ);

            int suma = 0;

            foreach (Wierzchołek wierzchołek in v1)
                suma += wierzchołek.Sąsiedzi.First().Waga;
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

        static List<Wierzchołek> ŚcieżkaRozszerzająca(List<Wierzchołek> graf, Wierzchołek początek)
        {
            Dictionary<int, List<Sąsiad>> słownikSąsiedztwa = new Dictionary<int, List<Sąsiad>>();

            foreach (Wierzchołek wierzchołek in graf)
                słownikSąsiedztwa.Add(wierzchołek.Nazwa, new List<Sąsiad>(wierzchołek.Sąsiedzi));

            foreach (Wierzchołek wierzchołek in graf.FindAll(w => w.ZbiórWGrafieDwudzielnym == 1))
            {
                Sąsiad skojarzonySąsiad = wierzchołek.Sąsiedzi.Find(s => s.Wierzchołek == wierzchołek.SkojarzonyZ);

                wierzchołek.Sąsiedzi.Remove(skojarzonySąsiad);
            }

            foreach (Wierzchołek wierzchołek in graf.FindAll(w => w.ZbiórWGrafieDwudzielnym == 2))
                wierzchołek.Sąsiedzi.RemoveAll(s => s.Wierzchołek != wierzchołek.SkojarzonyZ);

            Queue<Wierzchołek> kolejka = new Queue<Wierzchołek>();
            List<Wierzchołek> ścieżkaRozszerzająca = new List<Wierzchołek>();

            if (początek != null)
            {
                początek.Rodzic = null;

                kolejka.Enqueue(początek);

                while (kolejka.Any())
                {
                    Wierzchołek wierzchołek = kolejka.Dequeue();

                    if (wierzchołek.SkojarzonyZ == null && wierzchołek.Rodzic != null)
                    {
                        ścieżkaRozszerzająca.Add(wierzchołek);

                        while (wierzchołek.Rodzic != null)
                        {
                            wierzchołek = wierzchołek.Rodzic;

                            ścieżkaRozszerzająca.Add(wierzchołek);
                        }

                        break;
                    }
                    else
                        foreach (Sąsiad sąsiad in wierzchołek.Sąsiedzi)
                            if (!sąsiad.Wierzchołek.Odwiedzony)
                            {
                                sąsiad.Wierzchołek.Rodzic = wierzchołek;

                                kolejka.Enqueue(sąsiad.Wierzchołek);
                            }

                    wierzchołek.Odwiedzony = true;
                }

                ścieżkaRozszerzająca.Reverse();
            }

            foreach (int nazwaWierzchołka in słownikSąsiedztwa.Keys)
            {
                Wierzchołek wierzchołek = graf.Find(w => w.Nazwa == nazwaWierzchołka);
                wierzchołek.Odwiedzony = false;
                wierzchołek.Rodzic = null;
                wierzchołek.Sąsiedzi = słownikSąsiedztwa[nazwaWierzchołka];
                wierzchołek.Sąsiedzi = wierzchołek.Sąsiedzi.OrderBy(w => w.Wierzchołek.Nazwa).ToList();
            }

            return ścieżkaRozszerzająca;
        }

        static void MaksymalneSkojarzenie(List<Wierzchołek> graf, Wierzchołek początek)
        {
            List<Wierzchołek> p;

            do
            {
                p = ŚcieżkaRozszerzająca(graf, początek);

                if (p.Any())
                {
                    List<Krawędź> krawędzieSkojarzone = new List<Krawędź>();
                    List<Krawędź> krawędzieŚcieżkiRozszerzającej = new List<Krawędź>();

                    foreach (Wierzchołek wierzchołek in graf.Where(w => w.SkojarzonyZ != null))
                    {
                        Krawędź krawędź = new Krawędź(wierzchołek.Nazwa, wierzchołek.SkojarzonyZ.Nazwa);

                        if (!krawędzieSkojarzone.Contains(krawędź))
                            krawędzieSkojarzone.Add(krawędź);
                    }

                    for (int i = 0; i < p.Count - 1; i++)
                    {
                        Krawędź krawędź = new Krawędź(p[i].Nazwa, p[i + 1].Nazwa);

                        if (!krawędzieŚcieżkiRozszerzającej.Contains(krawędź))
                            krawędzieŚcieżkiRozszerzającej.Add(krawędź);
                    }

                    List<Krawędź> noweSkojarzenie = Enumerable.Union(krawędzieSkojarzone, krawędzieŚcieżkiRozszerzającej).ToList();

                    foreach (Krawędź krawędź in Enumerable.Intersect(krawędzieSkojarzone, krawędzieŚcieżkiRozszerzającej))
                        noweSkojarzenie.Remove(krawędź);

                    foreach (Wierzchołek wierzchołek in graf)
                        wierzchołek.SkojarzonyZ = null;

                    foreach (Krawędź krawędź in noweSkojarzenie)
                    {
                        Wierzchołek wierzchołek1 = graf.Find(w => w.Nazwa == krawędź.Wierzchołek1);
                        Wierzchołek wierzchołek2 = graf.Find(w => w.Nazwa == krawędź.Wierzchołek2);
                        wierzchołek1.SkojarzonyZ = wierzchołek2;
                        wierzchołek2.SkojarzonyZ = wierzchołek1;
                    }
                }
            }
            while (p.Any());
        }
    }
}