using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;

namespace alignment
{
    public class Program
    {
        private static void Main()
        {
            //Console.OutputEncoding = Encoding.UTF8;
            NameValueCollection konfiguracja = ConfigurationManager.AppSettings;
            Sekwencja sekwencja = OdczytajSekwencję(konfiguracja["nieznana sekwencja"]);
            Algorytm algorytm = (Algorytm) Enum.Parse(typeof (Algorytm), konfiguracja["algorytm"].ToUpper());
            int d = int.Parse(konfiguracja["d"]);
            Dictionary<char, Dictionary<char, int>> macierzSubstytucji = StwórzMacierzSubstytucji(konfiguracja["macierz substytucji"]);
            IEnumerable<Sekwencja> bazaDanych = StwórzBazęDanych();

            foreach (Sekwencja sekwencjaZBazy in bazaDanych)
            {
                int długośćLiczb;
                Komórka[,] macierz = StwórzMacierz(algorytm, macierzSubstytucji, sekwencja.Struktura, sekwencjaZBazy.Struktura, d, out długośćLiczb);
            }

            //WyświetlWyniki(macierz, sekwencja1, sekwencja2, algorytm, długośćLiczb);

            Console.Write("Naciśnij dowolny klawisz, aby kontynuować...");
            Console.ReadKey();
        }

        private static Sekwencja OdczytajSekwencję(string nazwaPliku)
        {
            using (StreamReader strumień = new StreamReader(nazwaPliku))
            {
                Sekwencja sekwencja = new Sekwencja {Nazwa = strumień.ReadLine()};

                while (!strumień.EndOfStream)
                    sekwencja.Struktura = string.Concat(sekwencja.Struktura, strumień.ReadLine());

                return sekwencja;
            }
        }

        private static IEnumerable<Sekwencja> StwórzBazęDanych()
        {
            List<Sekwencja> sekwencje = new List<Sekwencja>();

            using (StreamReader strumień = new StreamReader("DataBase.fasta"))
                while (!strumień.EndOfStream)
                {
                    string linia = strumień.ReadLine();

                    if (linia != null)
                        if (linia.StartsWith(">"))
                        {
                            Sekwencja sekwencja = new Sekwencja { Nazwa = linia };

                            sekwencje.Add(sekwencja);
                        }
                        else
                        {
                            Sekwencja ostatnia = sekwencje.Last();
                            ostatnia.Struktura = string.Concat(ostatnia.Struktura, linia);
                        }
                }

            return sekwencje;
        }

        private static Dictionary<char, Dictionary<char, int>> StwórzMacierzSubstytucji(string nazwaPliku)
        {
            string[] linie = File.ReadAllLines(nazwaPliku).Where(l => !l.StartsWith("#")).ToArray();
            string pierwszaLinia = linie[0];
            Dictionary<char, Dictionary<char, int>> macierz = pierwszaLinia.Where(znak => znak != ' ').ToDictionary(znak => znak, znak => new Dictionary<char, int>());
            int liczbaElementów = macierz.Count;

            for (int i = 0; i < liczbaElementów; i++)
                for (int j = 0; j < liczbaElementów; j++)
                {
                    int liczba = int.Parse(linie[i + 1].Substring(1 + j*3, 3));
                    Dictionary<char, Dictionary<char, int>>.KeyCollection klucze = macierz.Keys;

                    macierz[klucze.ElementAt(i)].Add(klucze.ElementAt(j), liczba);
                }

            return macierz;
        }

        private static Komórka[,] StwórzMacierz(Algorytm algorytm, IReadOnlyDictionary<char, Dictionary<char, int>> macierzSubstytucji, string sekwencja1, string sekwencja2, int d, out int długośćLiczb)
        {
            int przelicznik = -1;
            int długośćSekwencji1 = sekwencja1.Length;
            int długośćSekwencji2 = sekwencja2.Length;
            int liczbaWierszy = długośćSekwencji1 + 1;
            int liczbaKolumn = długośćSekwencji2 + 1;
            Komórka[,] macierz = new Komórka[liczbaWierszy, liczbaKolumn];
            długośćLiczb = 0;

            switch (algorytm)
            {
                case Algorytm.NW:
                    przelicznik = d;

                    break;

                case Algorytm.SW:
                    przelicznik = 0;

                    break;
            }

            for (int i = 0; i < liczbaWierszy; i++)
                macierz[i, 0] = new Komórka {Liczba = i*przelicznik};

            for (int j = 0; j < liczbaKolumn; j++)
                macierz[0, j] = new Komórka {Liczba = j*przelicznik};

            for (int i = 1; i < liczbaWierszy; i++)
                for (int j = 1; j < liczbaKolumn; j++)
                {
                    char znak1 = sekwencja1[i - 1];
                    char znak2 = sekwencja2[j - 1];
                    int s = macierzSubstytucji[znak1][znak2];
                    int skos = macierz[i - 1, j - 1].Liczba + s;
                    int góra = macierz[i - 1, j].Liczba + d;
                    int lewo = macierz[i, j - 1].Liczba + d;
                    int maksimum = Math.Max(skos, Math.Max(góra, lewo));
                    Strzałka strzałka;

                    if (algorytm == Algorytm.SW)
                        maksimum = Math.Max(maksimum, 0);

                    if (maksimum == skos)
                        strzałka = Strzałka.Skos;
                    else if (maksimum == góra)
                        strzałka = Strzałka.Góra;
                    else
                        strzałka = Strzałka.Lewo;

                    macierz[i, j] = new Komórka {Liczba = maksimum, Strzałka = strzałka};
                    długośćLiczb = Math.Max(długośćLiczb, maksimum.ToString().Length);
                }

            return macierz;
        }

        private static void WyświetlWyniki(Komórka[,] macierz, string sekwencja1, string sekwencja2, Algorytm algorytm, int długośćLiczb)
        {
            int k = -1;
            int l = -1;
            Komórka komórka = null;
            bool koniec = false;
            string nowaSekwencja1 = sekwencja1;
            string nowaSekwencja2 = sekwencja2;
            int długośćSekwencji1 = sekwencja1.Length;
            int długośćSekwencji2 = sekwencja2.Length;
            int liczbaWierszy = długośćSekwencji1 + 1;
            int liczbaKolumn = długośćSekwencji2 + 1;

            switch (algorytm)
            {
                case Algorytm.NW:
                    k = długośćSekwencji1;
                    l = długośćSekwencji2;
                    komórka = macierz[k, l];

                    break;

                case Algorytm.SW:
                    komórka = macierz[0, 0];

                    for (int i = 0; i < liczbaWierszy; i++)
                        for (int j = 0; j < liczbaKolumn; j++)
                        {
                            Komórka aktualnaKomórka = macierz[i, j];

                            if (aktualnaKomórka.Liczba > komórka.Liczba)
                            {
                                komórka = aktualnaKomórka;
                                k = i;
                                l = j;
                            }
                        }

                    break;
            }

            if (komórka != null)
                do
                {
                    switch (komórka.Strzałka)
                    {
                        case Strzałka.Brak:
                            koniec = true;

                            break;

                        case Strzałka.Góra:
                            k--;
                            nowaSekwencja2 = nowaSekwencja2.Insert(l, "-");

                            break;

                        case Strzałka.Lewo:
                            l--;
                            nowaSekwencja1 = nowaSekwencja1.Insert(k, "-");

                            break;

                        case Strzałka.Skos:
                            k--;
                            l--;

                            break;
                    }

                    if (koniec)
                        break;

                    komórka = macierz[k, l];
                } while (true);

            długośćLiczb += 2;

            Console.Write(new string(' ', długośćLiczb * 2));

            for (int i = 0; i < długośćSekwencji2; i++)
                Console.Write(sekwencja2[i].ToString().PadLeft(długośćLiczb));

            Console.WriteLine();
            Console.Write(new string(' ', długośćLiczb));

            for (int j = 0; j < liczbaKolumn; j++)
                Console.Write(macierz[0, j].Liczba.ToString().PadLeft(długośćLiczb));

            Console.WriteLine();

            for (int i = 1; i < liczbaWierszy; i++)
            {
                Console.Write(string.Concat(sekwencja1[i - 1].ToString().PadLeft(długośćLiczb), macierz[i, 0].Liczba.ToString().PadLeft(długośćLiczb)));

                for (int j = 1; j < liczbaKolumn; j++)
                {
                    komórka = macierz[i, j];
                    char strzałka = ' ';

                    switch (komórka.Strzałka)
                    {
                        case Strzałka.Góra:
                            strzałka = '↑';

                            break;

                        case Strzałka.Lewo:
                            strzałka = '←';

                            break;

                        case Strzałka.Skos:
                            strzałka = '\\';

                            break;
                    }

                    Console.Write(string.Concat(komórka.Liczba, strzałka).PadLeft(długośćLiczb));
                }

                Console.WriteLine();
            }

            Console.WriteLine(nowaSekwencja1);
            Console.WriteLine(nowaSekwencja2);
        }
    }
}