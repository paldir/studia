using System;
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

        static List<ElementZerowy> Redukcja(double[,] odległości)
        {
            int wiersze = odległości.GetLength(0);
            int kolumny = odległości.GetLength(1);
            List<ElementZerowy> elementyZerowe = new List<ElementZerowy>();

            for (int i = 0; i < wiersze; i++)
            {
                double minimumWWierszu = Double.MaxValue;
                int indeksMinimum = -1;

                for (int j = 0; j < kolumny; j++)
                {
                    double wartość = odległości[i, j];

                    if (wartość < minimumWWierszu)
                    {
                        minimumWWierszu = wartość;
                        indeksMinimum = j;
                    }
                }

                for (int j = 0; j < kolumny; j++)
                    odległości[i, j] -= minimumWWierszu;

                elementyZerowe.Add(new ElementZerowy(i, indeksMinimum, minimumWWierszu));
            }

            for (int j = 0; j < kolumny; j++)
            {
                double minimumWKolumnie = Double.MaxValue;
                int indeksMinimum = -1;

                for (int i = 0; i < wiersze; i++)
                {
                    double wartość = odległości[i, j];

                    if (wartość < minimumWKolumnie)
                    {
                        minimumWKolumnie = wartość;
                        indeksMinimum = -1;
                    }
                }

                for (int i = 0; i < wiersze; i++)
                    odległości[i, j] -= minimumWKolumnie;

                elementyZerowe.Add(new ElementZerowy(indeksMinimum, j, minimumWKolumnie));
            }

            return elementyZerowe;
        }

        static double[,] WybierzRozwiązanie(double[,] oryginalneOdległości, ElementZerowy rozwiązanie)
        {
            int oryginalneWiersze = oryginalneOdległości.GetLength(0);
            int oryginalneKolumny = oryginalneOdległości.GetLength(1);
            double[,] odległości = new double[oryginalneWiersze - 1, oryginalneKolumny - 1];
            int k = 0;
            int l = 0;

            for (int i = 0; i < oryginalneWiersze; i++)
            {
                if (i == rozwiązanie.I)
                    continue;

                for (int j = 0; j < oryginalneKolumny; j++)
                {
                    if (j == rozwiązanie.J)
                        continue;

                    odległości[k, l] = oryginalneOdległości[i, j];
                    l++;
                }

                k++;
            }

            return odległości;
        }

        static void Main(string[] args)
        {
            double[,] odległościPoczątkowe = new double[,]
            {
                {1, 3, 93, 13, 33, 9},
                {4, 1, 77, 42, 21, 16},
                {45, 17, 1, 36, 16, 28},
                {39, 90, 80, 1, 56, 7},
                {28, 46, 88, 33, 1, 25},
                {3, 88, 18, 46, 92, 1}
            };

            for (int i = 0; i < 6; i++)
                odległościPoczątkowe[i, i] = Double.PositiveInfinity;

            /*{
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
                            odległości[i, j] = Double.PositiveInfinity;
                        else
                            odległości[i, j] = ObliczOdległość(położeniaMiast[miasta[i]], położeniaMiast[miasta[j]]);
            }*/

            SortedDictionary<double, Węzeł> dolneOgraniczenieNaLiść = new SortedDictionary<double, Węzeł>();
            double[,] odległości = odległościPoczątkowe;

            do
            {
                List<ElementZerowy> elementyZerowe = Redukcja(odległości);
                
                Węzeł węzeł = new Węzeł(odległości);
                double dolneOgraniczenie=elementyZerowe.Sum(e => e.PoprzedniaWartość);

                ///

                dolneOgraniczenieNaLiść.Add(dolneOgraniczenie, węzeł);

                odległości = dolneOgraniczenieNaLiść.First().Value.Odległości;
            }
            while (odległości.GetLength(0) != 2 && odległości.GetLength(1) != 2);
        }

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
    }
}