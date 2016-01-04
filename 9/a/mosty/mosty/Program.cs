using System;
using System.Collections.Generic;
using System.Linq;

namespace mosty
{
    enum Cel { Mosty, PunktyArtykulacji }

    class Program
    {
        static void Main()
        {
            List<Wierzchołek> g;

            StwórzGrafNaPodstawiePliku(out g);
            Console.WriteLine("Mosty: ");
            Wypisz(g.First(), 0, Cel.Mosty);
            Console.WriteLine();

            foreach (Wierzchołek w in g)
            {
                w.F = w.L = 0;
                w.Odwiedzony = false;
                w.Rodzic = null;
            }

            Console.WriteLine("Punkty artykulacji: ");
            Wypisz(g.First(), 0, Cel.PunktyArtykulacji);
            Console.ReadKey();
        }

        static void Wypisz(Wierzchołek i, int d, Cel cel)
        {
            i.Odwiedzony = true;
            i.F = i.L = d;
            int liczbaDzieci = 0;
            bool punktArtykulacji = false;

            foreach (Wierzchołek ni in i.Sąsiedzi)
            {
                if (!ni.Odwiedzony)
                {
                    ni.Rodzic = i;

                    Wypisz(ni, d + 1, cel);

                    liczbaDzieci++;

                    if (ni.L >= i.F)
                        punktArtykulacji = true;

                    i.L = Math.Min(i.L, ni.L);

                    if (cel == Cel.Mosty && ni.L == ni.F)
                        Console.WriteLine("{0} {1}", i.Nazwa, ni.Nazwa);
                }
                else if (ni != i.Rodzic)
                    i.L = Math.Min(i.L, ni.F);
            }

            if (cel == Cel.PunktyArtykulacji && ((i.Rodzic != null && punktArtykulacji) || (i.Rodzic == null && liczbaDzieci > 1)))
                Console.WriteLine(i.Nazwa);
        }

        static void StwórzGrafNaPodstawiePliku(out List<Wierzchołek> kolekcjaWierzchołków)
        {
            string[] linie = System.IO.File.ReadAllLines("graf.txt").Where(l => !l.StartsWith("#")).ToArray();
            kolekcjaWierzchołków = new List<Wierzchołek>(linie.Length - 1);
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

            for (int i = 1; i < linie.Length; i++)
            {
                string[] elementyLinii = linie.ElementAt(i).Split(' ');

                Wierzchołek wierzchołek = kolekcjaWierzchołków.Find(w => w.Nazwa == elementyLinii[0]);
                List<Wierzchołek> sąsiedzi = wierzchołek.Sąsiedzi;

                for (int j = 1; j < elementyLinii.Length; j++)
                {
                    Wierzchołek sąsiad = kolekcjaWierzchołków.Find(w => w.Nazwa == elementyLinii[j]);
                    List<Wierzchołek> sąsiedziSąsiada = sąsiad.Sąsiedzi;

                    if (!sąsiedzi.Contains(sąsiad))
                        sąsiedzi.Add(sąsiad);

                    if (!sąsiedziSąsiada.Contains(wierzchołek))
                        sąsiedziSąsiada.Add(wierzchołek);
                }
            }
        }
    }
}