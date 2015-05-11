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

                        int kwadrat = i % 3 + j % 3 + 1;

                    http://stackoverflow.com/questions/5269064/sudoku-find-current-square-based-on-row-column

                        sudoku[i, j] = new Wierzchołek(Convert.ToInt32(bufor[0].ToString()), kwadrat);
                    }

            for(int i=0; i<9; i++)
                for (int j = 0; j < 9; j++)
                {
                    Wierzchołek wierzchołek = sudoku[i, j];

                    if(wierzchołek.Kolor!=0)
                    {

                    }
                }
        }
    }
}