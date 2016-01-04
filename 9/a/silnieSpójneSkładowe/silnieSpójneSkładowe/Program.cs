using System;
using System.Collections.Generic;
using System.Linq;

using System.IO;

namespace silnieSpójneSkładowe
{
    class Program
    {
        static void Main()
        {
            List<Wierzchołek> g;
            int czasOdwiedzenia = 1;
            int czasPrzetworzenia = 1;

            StwórzGrafNaPodstawieWyrażeniaZPliku(out g);
            TotalnyDfs(g, ref czasOdwiedzenia, ref czasPrzetworzenia);
            TransponujGraf(g);
            SortujWierzchołki(g);

            foreach (Wierzchołek v in g)
                v.CzasOdwiedzenia = 0;

            List<List<Wierzchołek>> silnieSpójneSkładowe = TotalnyDfs(g, ref czasOdwiedzenia, ref czasPrzetworzenia);
            bool rozwiązywalne = true;

            TransponujGraf(g);

            foreach (List<Wierzchołek> silnieSpójnaSkładowa in silnieSpójneSkładowe)
            {
                foreach (Wierzchołek wierzchołek in silnieSpójnaSkładowa)
                    Console.Write("{0} ", wierzchołek.NazwaLogiczna);

                foreach (Wierzchołek wierzchołek in silnieSpójnaSkładowa)
                {
                    Wierzchołek negacja = g.Find(w => w.Nazwa == wierzchołek.Nazwa && w.Negacja == !wierzchołek.Negacja);

                    if (!wierzchołek.WartośćLogiczna.HasValue)
                    {
                        wierzchołek.WartośćLogiczna = false;

                        if (!negacja.WartośćLogiczna.HasValue)
                            negacja.WartośćLogiczna = true;
                    }

                    if (silnieSpójnaSkładowa.Contains(negacja))
                    {
                        rozwiązywalne = false;

                        Console.Write("\tx");

                        break;
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine();

            if (rozwiązywalne)
            {
                foreach (List<Wierzchołek> silnieSpójnaSkładowa in silnieSpójneSkładowe)
                    foreach (Wierzchołek wierzchołek in silnieSpójnaSkładowa)
                        if (wierzchołek.WartośćLogiczna.HasValue)
                        {
                            if (wierzchołek.WartośćLogiczna.Value)
                                foreach (Wierzchołek sąsiad in wierzchołek.Sąsiedzi)
                                    sąsiad.WartośćLogiczna = true;
                        }
                        else
                            throw new Exception();

                foreach (Wierzchołek wierzchołek in g.OrderBy(w => w.Nazwa))
                    if (!wierzchołek.Negacja)
                        Console.Write("{0} ", wierzchołek.WartośćLogiczna != null && wierzchołek.WartośćLogiczna.Value ? 1 : 0);
            }
            else
                Console.WriteLine("brak rozwiązania");

            Console.ReadKey();
        }

        static List<List<Wierzchołek>> TotalnyDfs(List<Wierzchołek> g, ref int czasOdwiedzenia, ref int czasPrzetworzenia)
        {
            bool istniejąNieodwiedzoneWierzchołki;
            List<List<Wierzchołek>> silnieSpójneSkładowe = new List<List<Wierzchołek>>();

            do
            {
                Wierzchołek pierwszyNieodwiedzony = g.FirstOrDefault(w => !w.Odwiedzony);
                istniejąNieodwiedzoneWierzchołki = pierwszyNieodwiedzony != null;

                if (istniejąNieodwiedzoneWierzchołki)
                {
                    List<Wierzchołek> silnieSpójnaSkładowa = new List<Wierzchołek>();

                    silnieSpójneSkładowe.Add(silnieSpójnaSkładowa);
                    Dfs(pierwszyNieodwiedzony, ref czasOdwiedzenia, ref czasPrzetworzenia, ref silnieSpójnaSkładowa);
                }
            }
            while (istniejąNieodwiedzoneWierzchołki);

            return silnieSpójneSkładowe;
        }

        static void Dfs(Wierzchołek v, ref int czasOdwiedzenia, ref int czasPrzetworzenia, ref List<Wierzchołek> silnieSpójnaSkładowa)
        {
            v.CzasOdwiedzenia = czasOdwiedzenia;
            czasOdwiedzenia++;

            foreach (Wierzchołek sąsiad in v.Sąsiedzi)
                if (!sąsiad.Odwiedzony)
                    Dfs(sąsiad, ref czasOdwiedzenia, ref czasPrzetworzenia, ref silnieSpójnaSkładowa);

            v.CzasPrzetworzenia = czasPrzetworzenia;
            czasPrzetworzenia++;

            silnieSpójnaSkładowa.Add(v);
        }

        static void TransponujGraf(List<Wierzchołek> g)
        {
            List<Tuple<string, bool, List<Tuple<string, bool>>>> listySąsiedztwa = new List<Tuple<string, bool, List<Tuple<string, bool>>>>();

            foreach (Wierzchołek v in g)
            {
                List<Wierzchołek> sąsiedzi = v.Sąsiedzi;
                List<Tuple<string, bool>> nazwySąsiadów = sąsiedzi.Select(sąsiad => new Tuple<string, bool>(sąsiad.Nazwa, sąsiad.Negacja)).ToList();

                listySąsiedztwa.Add(new Tuple<string, bool, List<Tuple<string, bool>>>(v.Nazwa, v.Negacja, nazwySąsiadów));

                sąsiedzi.Clear();
            }

            foreach (Tuple<string, bool, List<Tuple<string, bool>>> pozycja in listySąsiedztwa)
            {
                Wierzchołek v = g.Find(w => w.Nazwa == pozycja.Item1 && w.Negacja == pozycja.Item2);

                foreach (Tuple<string, bool> nazwaSąsiada in pozycja.Item3)
                {
                    Wierzchołek sąsiad = g.Find(w => w.Nazwa == nazwaSąsiada.Item1 && w.Negacja == nazwaSąsiada.Item2);

                    sąsiad.Sąsiedzi.Add(v);
                }
            }
        }

        static void SortujWierzchołki(List<Wierzchołek> g)
        {
            Comparison<Wierzchołek> porównanie = (x, y) => -x.CzasPrzetworzenia.CompareTo(y.CzasPrzetworzenia);
            g.Sort(porównanie);

            foreach (Wierzchołek v in g)
                v.Sąsiedzi.Sort(porównanie);
        }

        static void StwórzGrafNaPodstawieWyrażeniaZPliku(out List<Wierzchołek> g)
        {
            g = new List<Wierzchołek>();
            IEnumerable<string> linie = File.ReadAllLines("wyrażenie.txt").Where(l => !l.StartsWith("#"));

            foreach (string linia in linie)
            {
                string[] elementyLinii = linia.Split(' ');
                string nazwaPierwszego = elementyLinii[0];
                string nazwaDrugiego = elementyLinii[1];
                bool pierwszyZanegowany = nazwaPierwszego.StartsWith("-");
                bool drugiZanegowany = nazwaDrugiego.StartsWith("-");

                if (pierwszyZanegowany)
                    nazwaPierwszego = nazwaPierwszego.Remove(0, 1);

                if (drugiZanegowany)
                    nazwaDrugiego = nazwaDrugiego.Remove(0, 1);

                if (g.Find(w => w.Nazwa == nazwaPierwszego) == null)
                {
                    g.Add(new Wierzchołek(nazwaPierwszego, true));
                    g.Add(new Wierzchołek(nazwaPierwszego, false));
                }

                if (g.Find(w => w.Nazwa == nazwaDrugiego) == null)
                {
                    g.Add(new Wierzchołek(nazwaDrugiego, true));
                    g.Add(new Wierzchołek(nazwaDrugiego, false));
                }

                Wierzchołek początekPierwszejKrawędzi = g.Find(w => w.Nazwa == nazwaPierwszego && w.Negacja == !pierwszyZanegowany);
                Wierzchołek koniecPierwszejKrawędzi = g.Find(w => w.Nazwa == nazwaDrugiego && w.Negacja == drugiZanegowany);
                Wierzchołek początekDrugiejKrawędzi = g.Find(w => w.Nazwa == nazwaDrugiego && w.Negacja == !drugiZanegowany);
                Wierzchołek koniecDrugiejKrawędzi = g.Find(w => w.Nazwa == nazwaPierwszego && w.Negacja == pierwszyZanegowany);

                początekPierwszejKrawędzi.Sąsiedzi.Add(koniecPierwszejKrawędzi);
                początekDrugiejKrawędzi.Sąsiedzi.Add(koniecDrugiejKrawędzi);
            }

            foreach (Wierzchołek wierzchołek in g)
                wierzchołek.Sąsiedzi = wierzchołek.Sąsiedzi.Distinct().ToList();
        }
    }
}