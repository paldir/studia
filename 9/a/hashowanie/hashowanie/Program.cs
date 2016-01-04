using System;
using System.Linq;

namespace hashowanie
{
    class Program
    {
        const int M = 1500;

        static void Main()
        {
            string[] nazwiska = System.IO.File.ReadAllLines("hash-dane.txt");
            TablicaHaszowanaZamknięcie<string> mod = new TablicaHaszowanaZamknięcie<string>(M, KonwertujNapisNaInt);
            TablicaHaszowanaOtwarcie<string> otwarta = new TablicaHaszowanaOtwarcie<string>(M, KonwertujNapisNaInt);

            foreach (string nazwisko in nazwiska)
            {
                mod.Umieść(nazwisko);
                otwarta.Umieść(nazwisko);
            }

            Console.WriteLine("Początkowe rozmiary tablic: {0}", M);
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("--- zamknięta ---");
            Console.WriteLine();
            Console.WriteLine("Ilość elementów w tablicy: {0}", mod.LiczbaElementów);
            Console.WriteLine("Rozmiar tablicy: {0}", mod.Tablica.Length);
            //Console.WriteLine("Średnia ilość elementów na liście: {0}", mod.Tablica.Average(l => l.Count));
            //Console.WriteLine("Ilość pustych list: {0}", mod.Tablica.Count(l => l.Count == 0));
            Console.WriteLine("Maksymalna ilość elementów na liście: {0}", mod.Tablica.Max(l => l.Count));
            Console.WriteLine("Liczba konfliktów przy dodawaniu: {0}", mod.LiczbaKonfliktówPrzyDodawaniu);
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("--- otwarta ---");
            Console.WriteLine();
            Console.WriteLine("Ilość elementów w tablicy: {0}", otwarta.LiczbaElementów);
            Console.WriteLine("Rozmiar tablicy: {0}", otwarta.Tablica.Length);
            Console.WriteLine("Liczba konfliktów przy dodawaniu: {0}", otwarta.LiczbaKonfliktówPrzyDodawaniu);

            Console.ReadKey();
        }

        static int KonwertujNapisNaInt(string napis)
        {
            //return napis.Sum(z => Convert.ToInt32(z));
            return Math.Abs(napis.GetHashCode());
        }
    }
}