using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace alignment
{
    public class Program
    {
        private static void Main()
        {
            //Console.OutputEncoding = Encoding.UTF8;
            string[] linie = File.ReadAllLines("seq.fasta").Where(x => !x.StartsWith(">")).ToArray();
            string sekwencja1 = linie[0];
            string sekwencja2 = linie[1];
            int długośćSekwencji1 = sekwencja1.Length;
            int długośćSekwencji2 = sekwencja2.Length;
            int liczbaWierszy = długośćSekwencji1 + 1;
            int liczbaKolumn = długośćSekwencji2 + 1;
            NameValueCollection konfiguracja = ConfigurationManager.AppSettings;
            Algorytm algorytm = (Algorytm)Enum.Parse(typeof(Algorytm), konfiguracja["algorytm"].ToUpper());
            int d = int.Parse(konfiguracja["d"]);
            int nagroda = int.Parse(konfiguracja["nagroda"]);
            int kara = int.Parse(konfiguracja["kara"]);
            Komórka[,] macierz = new Komórka[liczbaWierszy, liczbaKolumn];
            int przelicznik = -1;
            int długośćLiczb = 0;
            Dictionary<char, Dictionary<char, int>> macierzSubstytucji = MacierzSubstytucji(konfiguracja["macierz substytucji"]);

            #region przygotowanie macierzy
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
                macierz[i, 0] = new Komórka { Liczba = i * przelicznik };

            for (int j = 0; j < liczbaKolumn; j++)
                macierz[0, j] = new Komórka { Liczba = j * przelicznik };

            for (int i = 1; i < liczbaWierszy; i++)
                for (int j = 1; j < liczbaKolumn; j++)
                {
                    int s = sekwencja1[i - 1] == sekwencja2[j - 1] ? nagroda : kara;
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

                    macierz[i, j] = new Komórka { Liczba = maksimum, Strzałka = strzałka };
                    długośćLiczb = Math.Max(długośćLiczb, maksimum.ToString().Length);
                }
            #endregion

            #region wyświetlanie wyników
            int k = -1;
            int l = -1;
            Komórka komórka = null;
            bool koniec = false;
            string nowaSekwencja1 = sekwencja1;
            string nowaSekwencja2 = sekwencja2;

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
            #endregion

            Console.ReadKey();
        }

        private static IEnumerable<Sekwencja> BazaDanych()
        {
            List<Sekwencja> sekwencje = new List<Sekwencja>();

            using (StreamReader strumień = new StreamReader("DataBase.fasta"))
                while (!strumień.EndOfStream)
                {
                    Sekwencja sekwencja = new Sekwencja() { Nazwa = strumień.ReadLine(), Struktura = strumień.ReadLine() };

                    sekwencje.Add(sekwencja);
                }

            return sekwencje;
        }

        private static Dictionary<char, Dictionary<char, int>> MacierzSubstytucji(string nazwaPliku)
        {
            Dictionary<char, Dictionary<char, int>> macierz = new Dictionary<char, Dictionary<char, int>>();
            string[] linie = File.ReadAllLines(nazwaPliku).Where(l => !l.StartsWith("#")).ToArray();
            string pierwszaLinia=linie[0];

            foreach(char znak in pierwszaLinia)
                if (znak != ' ')
                    macierz.Add(znak, new Dictionary<char, int>());

            foreach (Dictionary<char, int> słownik in macierz.Values)
                foreach (char znak in pierwszaLinia)
                    if (znak != ' ')
                        słownik.Add(znak, 0);

            for (int i = 1; i < linie.Length; i++)
            {
                string linia=linie[i];
                int j=1;

                for (int k = 1; k < linia.Length; k += 3)
                {
                    char znak = Char.Parse(linia.Substring(j, 3));
                }
            }

            return macierz;
        }
    }
}