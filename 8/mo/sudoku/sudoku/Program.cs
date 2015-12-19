
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace sudoku
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] pliki = Directory.GetFiles("easy");
            Random random = new Random();
            Wierzchołek[,] sudoku = new Wierzchołek[9, 9];
            char[] bufor = new char[1];

            using (StreamReader strumień = new StreamReader(pliki[random.Next(pliki.Length)]))
                for (int i = 0; i < 9; i++)
                    for (int j = 0; j < 9; j++)
                    {
                        do
                            strumień.ReadBlock(bufor, 0, 1);
                        while (!Char.IsNumber(bufor[0]) && bufor[0] != '.');

                        if (bufor[0] == '.')
                            bufor[0] = '0';

                        int kwadrat = j / 3 + i / 3 * 3 + 1;

                        sudoku[i, j] = new Wierzchołek(Convert.ToInt32(bufor[0].ToString()), kwadrat, i, j);
                    }

            Wyświetl(sudoku);

            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                {
                    Wierzchołek wierzchołek = sudoku[i, j];

                    for (int k = 0; k < 9; k++)
                        for (int l = 0; l < 9; l++)
                        {
                            Wierzchołek potencjalnySąsiad = sudoku[k, l];
                            List<Wierzchołek> sąsiedzi = wierzchołek.Sąsiedzi;

                            if ((k == i || l == j || potencjalnySąsiad.Kwadrat == wierzchołek.Kwadrat) && wierzchołek != potencjalnySąsiad)
                                sąsiedzi.Add(potencjalnySąsiad);
                        }
                }

            bool zmiana;

            do
            {
                zmiana = false;
                List<PustaKomórka> pusteWierzchołki = new List<PustaKomórka>();

                for (int i = 0; i < 9; i++)
                    for (int j = 0; j < 9; j++)
                        if (sudoku[i, j].Kolor == 0)
                            pusteWierzchołki.Add(new PustaKomórka(i, j));

                for (int i = 0; i < 9; i++)
                    for (int j = 0; j < 9; j++)
                    {
                        Wierzchołek wierzchołek = sudoku[i, j];
                        int kolor = wierzchołek.Kolor;

                        if (kolor != 0)
                            foreach (PustaKomórka pustyWierzchołek in pusteWierzchołki)
                                foreach (Wierzchołek sąsiad in wierzchołek.Sąsiedzi)
                                    if (pustyWierzchołek.I == sąsiad.I && pustyWierzchołek.J == sąsiad.J)
                                        pustyWierzchołek.Możliwości.Remove(kolor);
                    }

                foreach (PustaKomórka pustaKomórka in pusteWierzchołki.Where(w => w.Możliwości.Count == 1))
                {
                    sudoku[pustaKomórka.I, pustaKomórka.J].Kolor = pustaKomórka.Możliwości.Single();
                    zmiana = true;

                    Wyświetl(sudoku);
                }
            }
            while (zmiana);

            SprawdźPoprawność(sudoku);
            Wyświetl(sudoku);
        }

        /*static bool WpiszJedyneMożliwości(Wierzchołek[,] sudoku)
        {
            bool zmiana = false;

            for (int i = 0; i < 9; i++)
            {
                List<int> liczbyWWierszu = new List<int>();
                Wierzchołek pustaKomórka = null;

                for (int j = 0; j < 9; j++)
                {
                    Wierzchołek komórka = sudoku[i, j];
                    int kolorKomórki = komórka.Kolor;

                    if (kolorKomórki == 0)
                        pustaKomórka = komórka;
                    else
                        liczbyWWierszu.Add(kolorKomórki);
                }

                if (liczbyWWierszu.Count == 8)
                {
                    pustaKomórka.Kolor = Liczby.Except(liczbyWWierszu).Single();
                    zmiana = true;
                    SprawdźPoprawność(sudoku);
                }
            }

            for (int j = 0; j < 9; j++)
            {
                List<int> liczbyWKolumnie = new List<int>();
                Wierzchołek pustaKomórka = null;

                for (int i = 0; i < 9; i++)
                {
                    Wierzchołek komórka = sudoku[i, j];
                    int kolorKomórki = komórka.Kolor;

                    if (kolorKomórki == 0)
                        pustaKomórka = komórka;
                    else
                        liczbyWKolumnie.Add(kolorKomórki);
                }

                if (liczbyWKolumnie.Count == 8)
                {
                    pustaKomórka.Kolor = Liczby.Except(liczbyWKolumnie).Single();
                    zmiana = true;
                    SprawdźPoprawność(sudoku);
                }
            }

            return zmiana;
        }*/

        public static readonly List<int> Liczby = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        static void Wyświetl(Wierzchołek[,] sudoku)
        {
            Console.Clear();

            for (int i = 0; i < 9; i++)
            {
                if (i % 3 == 0)
                    Console.WriteLine();

                for (int j = 0; j < 9; j++)
                {
                    if (j % 3 == 0)
                        Console.Write('\t');

                    int kolor = sudoku[i, j].Kolor;

                    Console.Write("{0} ", kolor == 0 ? "-" : kolor.ToString());
                }

                Console.WriteLine();
            }

            Console.ReadKey();
        }

        static void SprawdźPoprawność(Wierzchołek[,] sudoku)
        {
            for (int i = 0; i < 9; i++)
            {
                List<int> liczbyWWierszu = new List<int>();

                for (int j = 0; j < 9; j++)
                {
                    int kolorKomórki = sudoku[i, j].Kolor;

                    if (kolorKomórki != 0)
                        liczbyWWierszu.Add(kolorKomórki);
                }

                if (liczbyWWierszu.Distinct().Count() != liczbyWWierszu.Count)
                    throw new Exception();
            }

            for (int j = 0; j < 9; j++)
            {
                List<int> liczbyWKolumnie = new List<int>();

                for (int i = 0; i < 9; i++)
                {
                    int kolorKomórki = sudoku[i, j].Kolor;

                    if (kolorKomórki != 0)
                        liczbyWKolumnie.Add(kolorKomórki);
                }

                if (liczbyWKolumnie.Distinct().Count() != liczbyWKolumnie.Count)
                    throw new Exception();
            }
        }
    }
}