﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komiwojażer
{
    class Program
    {
        static readonly string[] miasta = new string[]
        {
            "Warszawa", 
            "Kraków", 
            "Łódź", 
            "Wrocław",
            "Poznań",
            "Gdańsk", 
            "Szczecin", 
            "Bydgoszcz",
            "Lublin",
            "Katowice",
            "Białystok",
            "Gdynia", 
            "Częstochowa",
            "Radom",
            "Sosnowiec", 
            "Toruń"
        };

        static double ObliczOdległość(PołożenieGeograficzne położenie1, PołożenieGeograficzne położenie2)
        {
            const int R = 6371;
            double fi1 = położenie1.Szerokość.ToRadians();
            double fi2 = położenie2.Szerokość.ToRadians();
            double deltaFi = (położenie2.Szerokość - położenie1.Szerokość).ToRadians();
            double deltaLambda = (położenie2.Długość - położenie1.Długość).ToRadians();
            double a = Math.Sin(deltaFi / 2) * Math.Sin(deltaFi / 2) + Math.Cos(fi1) * Math.Cos(fi2) * Math.Sin(deltaLambda / 2) * Math.Sin(deltaLambda / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c;
        }

        static void Main(string[] args)
        {
            double[,] odległości;

            {
                int ilośćMiast = miasta.Length;
                odległości = new double[ilośćMiast, ilośćMiast];
                List<string> linie = new List<string>();
                Dictionary<string, PołożenieGeograficzne> położeniaMiast = new Dictionary<string, PołożenieGeograficzne>();

                using (System.IO.StreamReader strumień = new System.IO.StreamReader("miasta.txt"))
                    while (!strumień.EndOfStream)
                        linie.Add(strumień.ReadLine());

                foreach (string miasto in miasta)
                {
                    string linia = linie.Find(l => l.StartsWith(miasto));
                    int stopnieDługości = Int32.Parse(linia.Substring(24, 2));
                    int minutyDługości = Int32.Parse(linia.Substring(27, 2));
                    int stopnieSzerokości = Int32.Parse(linia.Substring(39, 2));
                    int minutySzerokości = Int32.Parse(linia.Substring(42, 2));

                    położeniaMiast.Add(miasto, new PołożenieGeograficzne(stopnieSzerokości + minutySzerokości / 60.0, stopnieDługości + minutyDługości / 60.0));
                }

                for (int i = 0; i < ilośćMiast; i++)
                    for (int j = 0; j < ilośćMiast; j++)
                        if (i == j)
                            odległości[i, j] = Double.MaxValue;
                        else
                            odległości[i, j] = ObliczOdległość(położeniaMiast[miasta[i]], położeniaMiast[miasta[j]]);
            }
        }
    }
}