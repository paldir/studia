using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace maksymalneSkojarzenie
{
    class Program
    {
        static void SkojarzeniePoczątkowe(List<Wierzchołek> graf)
        {
            foreach (Wierzchołek wierzchołek in graf)
                if (wierzchołek.SkojarzonyZ == null)
                {
                    Wierzchołek nieskojarzonySąsiad = wierzchołek.Sąsiedzi.FirstOrDefault(s => s.SkojarzonyZ == null);

                    if (nieskojarzonySąsiad != null)
                    {
                        wierzchołek.SkojarzonyZ = nieskojarzonySąsiad;
                        nieskojarzonySąsiad.SkojarzonyZ = wierzchołek;
                    }
                }
        }

        static List<Wierzchołek> ŚcieżkaRozszerzająca(List<Wierzchołek> graf)
        {
            Dictionary<int, int[]> słownikSąsiedztwa = new Dictionary<int, int[]>();

            foreach (Wierzchołek wierzchołek in graf)
                słownikSąsiedztwa.Add(wierzchołek.Nazwa, wierzchołek.Sąsiedzi.Select(w => w.Nazwa).ToArray());

            foreach (Wierzchołek wierzchołek in graf.FindAll(w => w.ZbiórWGrafieDwudzielnym == 1))
                wierzchołek.Sąsiedzi.Remove(wierzchołek.SkojarzonyZ);

            foreach (Wierzchołek wierzchołek in graf.FindAll(w => w.ZbiórWGrafieDwudzielnym == 2))
                wierzchołek.Sąsiedzi.RemoveAll(w => w != wierzchołek.SkojarzonyZ);

            Queue<Wierzchołek> kolejka = new Queue<Wierzchołek>();
            List<Wierzchołek> ścieżkaRozszerzająca = new List<Wierzchołek>();
            Wierzchołek korzeń = graf.FirstOrDefault(w => w.SkojarzonyZ == null && w.Sąsiedzi.Any());

            if (korzeń != null)
            {
                korzeń.Rodzic = null;

                kolejka.Enqueue(korzeń);

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
                        foreach (Wierzchołek sąsiad in wierzchołek.Sąsiedzi)
                            if (!sąsiad.Odwiedzony)
                            {
                                sąsiad.Rodzic = wierzchołek;

                                kolejka.Enqueue(sąsiad);
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

                foreach (int nazwaSąsiada in słownikSąsiedztwa[nazwaWierzchołka])
                {
                    Wierzchołek sąsiad = graf.Find(w => w.Nazwa == nazwaSąsiada);

                    if (!wierzchołek.Sąsiedzi.Contains(sąsiad))
                        wierzchołek.Sąsiedzi.Add(sąsiad);
                }

                wierzchołek.Sąsiedzi = wierzchołek.Sąsiedzi.OrderBy(w => w.Nazwa).ToList();
            }

            return ścieżkaRozszerzająca;
        }

        static void MaksymalneSkojarzenie(List<Wierzchołek> graf)
        {
            List<Wierzchołek> p;

            SkojarzeniePoczątkowe(graf);

            do
            {
                p = ŚcieżkaRozszerzająca(graf);

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

        static void Main(string[] args)
        {
            List<Wierzchołek> graf = new List<Wierzchołek>();

            for (int i = 1; i <= 4; i++)
                graf.Add(new Wierzchołek(i, 1));

            for (int i = 5; i <= 8; i++)
                graf.Add(new Wierzchołek(i, 2));

            List<Krawędź> krawędzie = new List<Krawędź>()
            {
                new Krawędź(1, 5), 
                new Krawędź(1, 6),
                new Krawędź(1, 7), 
                new Krawędź(2, 5),
                new Krawędź(2, 6), 
                new Krawędź(2, 8), 
                new Krawędź(3, 5),
                new Krawędź(4, 7), 
                new Krawędź(4, 8)
            };

            krawędzie = krawędzie.Distinct().ToList();

            foreach (Krawędź krawędź in krawędzie)
            {
                Wierzchołek wierzchołek1 = graf.Find(w => w.Nazwa == krawędź.Wierzchołek1);
                Wierzchołek wierzchołek2 = graf.Find(w => w.Nazwa == krawędź.Wierzchołek2);

                if (!wierzchołek1.Sąsiedzi.Contains(wierzchołek2))
                    wierzchołek1.Sąsiedzi.Add(wierzchołek2);

                if (!wierzchołek2.Sąsiedzi.Contains(wierzchołek1))
                    wierzchołek2.Sąsiedzi.Add(wierzchołek1);
            }

            MaksymalneSkojarzenie(graf);
        }
    }
}