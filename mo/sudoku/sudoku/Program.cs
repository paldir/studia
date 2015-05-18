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

                        sudoku[i, j] = new Wierzchołek(Convert.ToInt32(bufor[0].ToString()), kwadrat);
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

                            if ((k == i || l == j || potencjalnySąsiad.Kwadrat == wierzchołek.Kwadrat) && !sąsiedzi.Contains(potencjalnySąsiad))
                            {
                                sąsiedzi.Add(potencjalnySąsiad);
                                potencjalnySąsiad.Sąsiedzi.Add(wierzchołek);
                            }
                        }
                }

            bool zmiana;

            do
            {
                zmiana = false;
                
                for (int h = 1; h <= 9; h++)
                {
                    List<Wierzchołek> pusteWierzchołki = new List<Wierzchołek>();

                    for (int i = 0; i < 9; i++)
                        for (int j = 0; j < 9; j++)
                        {
                            Wierzchołek wierzchołek = sudoku[i, j];

                            if (wierzchołek.Kolor == 0)
                                pusteWierzchołki.Add(wierzchołek);
                        }

                    for (int i = 0; i < 9; i++)
                        for (int j = 0; j < 9; j++)
                        {
                            Wierzchołek wierzchołek = sudoku[i, j];

                            if (wierzchołek.Kolor == h)
                                foreach (Wierzchołek sąsiad in wierzchołek.Sąsiedzi)
                                    pusteWierzchołki.Remove(sąsiad);
                        }

                    foreach (Wierzchołek wierzchołek in pusteWierzchołki)
                    {
                        zmiana = !wierzchołek.Sąsiedzi.Exists(s => s.Kolor == h);

                        if (zmiana)
                            wierzchołek.Kolor = h;
                    }

                    Console.Clear();
                    Wyświetl(sudoku);
                }
            }
            while (zmiana);
        }

        static void Wyświetl(Wierzchołek[,] sudoku)
        {
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
    }
}