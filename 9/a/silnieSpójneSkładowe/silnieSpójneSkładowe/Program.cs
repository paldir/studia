using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace silnieSpójneSkładowe
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Wierzchołek> G;
            int czasOdwiedzenia = 1;
            int czasPrzetworzenia = 1;

            StwórzGrafNaPodstawieWyrażeniaZPliku(out G);
            TotalnyDfs(G, ref czasOdwiedzenia, ref czasPrzetworzenia);
            TransponujGraf(G);
            SortujWierzchołki(G);

            foreach (Wierzchołek v in G)
                v.CzasOdwiedzenia = 0;

            List<List<Wierzchołek>> silnieSpójneSkładowe = TotalnyDfs(G, ref czasOdwiedzenia, ref czasPrzetworzenia);
            bool rozwiązywalne = true;

            TransponujGraf(G);

            foreach (List<Wierzchołek> silnieSpójnaSkładowa in silnieSpójneSkładowe)
            {
                foreach (Wierzchołek wierzchołek in silnieSpójnaSkładowa)
                    Console.Write("{0} ", wierzchołek.NazwaLogiczna);

                foreach (Wierzchołek wierzchołek in silnieSpójnaSkładowa)
                {
                    Wierzchołek negacja = G.Find(w => w.Nazwa == wierzchołek.Nazwa && w.Negacja == !wierzchołek.Negacja);

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

                foreach (Wierzchołek wierzchołek in G.OrderBy(w => w.Nazwa))
                    if (!wierzchołek.Negacja)
                        Console.Write("{0} ", wierzchołek.WartośćLogiczna.Value ? 1 : 0);
            }
            else
                Console.WriteLine("brak rozwiązania");

            Console.ReadKey();
        }

        static List<List<Wierzchołek>> TotalnyDfs(List<Wierzchołek> G, ref int czasOdwiedzenia, ref int czasPrzetworzenia)
        {
            bool istniejąNieodwiedzoneWierzchołki;
            List<List<Wierzchołek>> silnieSpójneSkładowe = new List<List<Wierzchołek>>();

            do
            {
                Wierzchołek pierwszyNieodwiedzony = G.FirstOrDefault(w => !w.Odwiedzony);
                istniejąNieodwiedzoneWierzchołki = pierwszyNieodwiedzony != null;

                if (istniejąNieodwiedzoneWierzchołki)
                {
                    List<Wierzchołek> silnieSpójnaSkładowa = new List<Wierzchołek>();

                    silnieSpójneSkładowe.Add(silnieSpójnaSkładowa);
                    Dfs(G, pierwszyNieodwiedzony, ref czasOdwiedzenia, ref czasPrzetworzenia, ref silnieSpójnaSkładowa);
                }
            }
            while (istniejąNieodwiedzoneWierzchołki);

            return silnieSpójneSkładowe;
        }

        static void Dfs(List<Wierzchołek> G, Wierzchołek v, ref int czasOdwiedzenia, ref int czasPrzetworzenia, ref List<Wierzchołek> silnieSpójnaSkładowa)
        {
            v.CzasOdwiedzenia = czasOdwiedzenia;
            czasOdwiedzenia++;

            foreach (Wierzchołek sąsiad in v.Sąsiedzi)
                if (!sąsiad.Odwiedzony)
                    Dfs(G, sąsiad, ref czasOdwiedzenia, ref czasPrzetworzenia, ref silnieSpójnaSkładowa);

            v.CzasPrzetworzenia = czasPrzetworzenia;
            czasPrzetworzenia++;

            silnieSpójnaSkładowa.Add(v);
        }

        static void TransponujGraf(List<Wierzchołek> G)
        {
            List<Tuple<string, bool, List<Tuple<string, bool>>>> listySąsiedztwa = new List<Tuple<string, bool, List<Tuple<string, bool>>>>();

            foreach (Wierzchołek v in G)
            {
                List<Wierzchołek> sąsiedzi = v.Sąsiedzi;
                List<Tuple<string, bool>> nazwySąsiadów = new List<Tuple<string, bool>>();

                foreach (Wierzchołek sąsiad in sąsiedzi)
                    nazwySąsiadów.Add(new Tuple<string, bool>(sąsiad.Nazwa, sąsiad.Negacja));

                listySąsiedztwa.Add(new Tuple<string, bool, List<Tuple<string, bool>>>(v.Nazwa, v.Negacja, nazwySąsiadów));

                sąsiedzi.Clear();
            }

            foreach (Tuple<string, bool, List<Tuple<string, bool>>> pozycja in listySąsiedztwa)
            {
                Wierzchołek v = G.Find(w => w.Nazwa == pozycja.Item1 && w.Negacja == pozycja.Item2);

                foreach (Tuple<string, bool> nazwaSąsiada in pozycja.Item3)
                {
                    Wierzchołek sąsiad = G.Find(w => w.Nazwa == nazwaSąsiada.Item1 && w.Negacja == nazwaSąsiada.Item2);

                    sąsiad.Sąsiedzi.Add(v);
                }
            }
        }

        static void SortujWierzchołki(List<Wierzchołek> G)
        {
            Comparison<Wierzchołek> porównanie = (x, y) => -x.CzasPrzetworzenia.CompareTo(y.CzasPrzetworzenia);
            G.Sort(porównanie);

            foreach (Wierzchołek v in G)
                v.Sąsiedzi.Sort(porównanie);
        }

        static void StwórzGrafNaPodstawiePliku(out List<Wierzchołek> kolekcjaWierzchołków)
        {
            IEnumerable<string> linie = File.ReadAllLines("graf.txt").Where(l => !l.StartsWith("#"));
            kolekcjaWierzchołków = new List<Wierzchołek>(linie.Count() - 1);
            string[] przedziałNazw = linie.First().Split(' ');
            string napisPoczątkuPrzedziału = przedziałNazw[0];
            string napisKońcaPrzedziału = przedziałNazw[1];
            int początekPrzedziału;

            if (Int32.TryParse(napisPoczątkuPrzedziału, out początekPrzedziału))
                for (int i = początekPrzedziału; i <= Int32.Parse(napisKońcaPrzedziału); i++)
                    kolekcjaWierzchołków.Add(new Wierzchołek(i.ToString()));
            else
                for (char i = Char.Parse(napisPoczątkuPrzedziału); i <= Char.Parse(napisKońcaPrzedziału); i++)
                    kolekcjaWierzchołków.Add(new Wierzchołek(i.ToString()));

            for (int i = 1; i < linie.Count(); i++)
            {
                string[] elementyLinii = linie.ElementAt(i).Split(' ');

                Wierzchołek wierzchołek = kolekcjaWierzchołków.Find(w => w.Nazwa == elementyLinii[0]);
                List<Wierzchołek> sąsiedzi = wierzchołek.Sąsiedzi;

                for (int j = 1; j < elementyLinii.Length; j++)
                {
                    Wierzchołek sąsiad = kolekcjaWierzchołków.Find(w => w.Nazwa == elementyLinii[j]);

                    if (!sąsiedzi.Contains(sąsiad))
                        sąsiedzi.Add(sąsiad);
                }
            }
        }

        static void StwórzGrafNaPodstawieWyrażeniaZPliku(out List<Wierzchołek> G)
        {
            G = new List<Wierzchołek>();
            IEnumerable<string> linie = File.ReadAllLines("wyrażenie.txt").Where(l => !l.StartsWith("#"));
            List<Wierzchołek> połączenia = new List<Wierzchołek>();

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

                if (G.Find(w => w.Nazwa == nazwaPierwszego) == null)
                {
                    G.Add(new Wierzchołek(nazwaPierwszego, true));
                    G.Add(new Wierzchołek(nazwaPierwszego, false));
                }

                if (G.Find(w => w.Nazwa == nazwaDrugiego) == null)
                {
                    G.Add(new Wierzchołek(nazwaDrugiego, true));
                    G.Add(new Wierzchołek(nazwaDrugiego, false));
                }

                Wierzchołek początekPierwszejKrawędzi = G.Find(w => w.Nazwa == nazwaPierwszego && w.Negacja == !pierwszyZanegowany);
                Wierzchołek koniecPierwszejKrawędzi = G.Find(w => w.Nazwa == nazwaDrugiego && w.Negacja == drugiZanegowany);
                Wierzchołek początekDrugiejKrawędzi = G.Find(w => w.Nazwa == nazwaDrugiego && w.Negacja == !drugiZanegowany);
                Wierzchołek koniecDrugiejKrawędzi = G.Find(w => w.Nazwa == nazwaPierwszego && w.Negacja == pierwszyZanegowany);

                początekPierwszejKrawędzi.Sąsiedzi.Add(koniecPierwszejKrawędzi);
                początekDrugiejKrawędzi.Sąsiedzi.Add(koniecDrugiejKrawędzi);
            }

            foreach (Wierzchołek wierzchołek in G)
                wierzchołek.Sąsiedzi = wierzchołek.Sąsiedzi.Distinct().ToList();
        }
    }
}