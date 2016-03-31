using System;
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
            string[] linie = File.ReadAllLines("seq.fasta").Where(x => !x.StartsWith(">")).ToArray();
            string sekwencja1 = linie[0];
            string sekwencja2 = linie[1];
            int długośćSekwencji1 = sekwencja1.Length;
            int długośćSekwencji2 = sekwencja2.Length;
            int liczbaWierszy = długośćSekwencji1 + 1;
            int liczbaKolumn = długośćSekwencji2 + 1;
            NameValueCollection konfiguracja = ConfigurationManager.AppSettings;
            int d = int.Parse(konfiguracja["d"]);
            int nagroda = int.Parse(konfiguracja["nagroda"]);
            int kara = int.Parse(konfiguracja["kara"]);
            Komórka[,] macierz = new Komórka[liczbaWierszy, liczbaKolumn];

            for (int i = 0; i < liczbaWierszy; i++)
                macierz[i, 0] = new Komórka {Liczba = i*d};

            for (int j = 0; j < liczbaKolumn; j++)
                macierz[0, j] = new Komórka {Liczba = j*d};

            for (int i = 1; i < liczbaWierszy; i++)
                for (int j = 1; j < liczbaKolumn; j++)
                {
                    int s = sekwencja1[i - 1] == sekwencja2[j - 1] ? nagroda : kara;
                    int skos = macierz[i - 1, j - 1].Liczba + s;
                    int góra = macierz[i - 1, j].Liczba + d;
                    int lewo = macierz[i, j - 1].Liczba + d;
                    int max = Math.Max(skos, Math.Max(góra, lewo));
                    Strzałka strzałka;

                    if (max == skos)
                        strzałka = Strzałka.Skos;
                    else if (max == góra)
                        strzałka = Strzałka.Góra;
                    else
                        strzałka = Strzałka.Lewo;

                    macierz[i, j] = new Komórka {Liczba = max, Strzałka = strzałka};
                }

            int k = długośćSekwencji1;
            int l = długośćSekwencji2;
            Komórka komórka = macierz[k, l];
            bool koniec = false;
            string nowaSekwencja1 = sekwencja1;
            string nowaSekwencja2 = sekwencja2;

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

            Console.WriteLine(nowaSekwencja1);
            Console.WriteLine(nowaSekwencja2);
            Console.ReadKey();
        }
    }
}