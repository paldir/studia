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
                double minimumWWierszu = odległości[i, 0];
                int indeksMinimum = 0;

                for (int j = 1; j < kolumny; j++)
                {
                    double wartość = odległości[i, j];

                    if (wartość < minimumWWierszu)
                    {
                        minimumWWierszu = wartość;
                        indeksMinimum = j;
                    }
                }

                if (minimumWWierszu != 0 && minimumWWierszu != Double.PositiveInfinity)
                {
                    for (int j = 0; j < kolumny; j++)
                        odległości[i, j] -= minimumWWierszu;

                    elementyZerowe.Add(new ElementZerowy(i, indeksMinimum, minimumWWierszu));
                }
            }

            for (int j = 0; j < kolumny; j++)
            {
                double minimumWKolumnie = odległości[0, j];
                int indeksMinimum = 0;

                for (int i = 1; i < wiersze; i++)
                {
                    double wartość = odległości[i, j];

                    if (wartość < minimumWKolumnie)
                    {
                        minimumWKolumnie = wartość;
                        indeksMinimum = i;
                    }
                }

                if (minimumWKolumnie != 0 && minimumWKolumnie != Double.PositiveInfinity)
                {
                    for (int i = 0; i < wiersze; i++)
                        odległości[i, j] -= minimumWKolumnie;

                    elementyZerowe.Add(new ElementZerowy(indeksMinimum, j, minimumWKolumnie));
                }
            }

            return elementyZerowe.OrderByDescending(e => e.PoprzedniaWartość).ToList();
        }

        static double[,] WybierzRozwiązanie(double[,] oryginalneOdległości, ElementZerowy rozwiązanie, List<ElementZerowy> dotychczasoweRozwiązania)
        {
            int oryginalneWiersze = oryginalneOdległości.GetLength(0);
            int oryginalneKolumny = oryginalneOdległości.GetLength(1);
            double[,] odległości = new double[oryginalneWiersze, oryginalneKolumny];
            int indeksIRozwiązania = rozwiązanie.I;
            int indeksJRozwiązania = rozwiązanie.J;

            for (int i = 0; i < oryginalneWiersze; i++)
                for (int j = 0; j < oryginalneKolumny; j++)
                    odległości[i, j] = oryginalneOdległości[i, j];

            for (int j = 0; j < oryginalneKolumny; j++)
                odległości[indeksIRozwiązania, j] = Double.PositiveInfinity;

            for (int i = 0; i < oryginalneWiersze; i++)
                odległości[i, indeksJRozwiązania] = Double.PositiveInfinity;

            if (odległości[indeksJRozwiązania, indeksIRozwiązania] == Double.PositiveInfinity)
            {
                ElementZerowy dotychczasoweRozwiązanie = rozwiązanie;

                for (int i = 0; i < dotychczasoweRozwiązania.Count - 1; i++)
                    dotychczasoweRozwiązanie = dotychczasoweRozwiązania.First(r => r.J == dotychczasoweRozwiązanie.I);

                odległości[rozwiązanie.J, dotychczasoweRozwiązanie.I] = Double.PositiveInfinity;
            }
            else
                odległości[indeksJRozwiązania, indeksIRozwiązania] = Double.PositiveInfinity;

                return odległości;
        }

        static double[,] NieWybierajRozwiązania(double[,] oryginalneOdległości, ElementZerowy rozwiązanie)
        {
            int oryginalneWiersze = oryginalneOdległości.GetLength(0);
            int oryginalneKolumny = oryginalneOdległości.GetLength(1);
            double[,] odległości = new double[oryginalneWiersze, oryginalneKolumny];

            for (int i = 0; i < oryginalneWiersze; i++)
                for (int j = 0; j < oryginalneKolumny; j++)
                    odległości[i, j] = oryginalneOdległości[i, j];

            odległości[rozwiązanie.I, rozwiązanie.J] = Double.PositiveInfinity;

            return odległości;
        }

        static ElementZerowy ZnajdźNajbardziejOpłacalneRozwiązanie(double[,] odległości, List<ElementZerowy> rozwiązania, out double strataPrzyNiewykorzystaniuRozwiązania)
        {
            ElementZerowy najbardziejOpłacalneRozwiązanie = null;
            strataPrzyNiewykorzystaniuRozwiązania = -1;

            foreach (ElementZerowy rozwiązanie in rozwiązania)
            {
                int indeksIRozwiązania = rozwiązanie.I;
                int indeksJRozwiązania = rozwiązanie.J;
                double minimumWWierszu = Double.MaxValue;

                for (int j = 0; j < odległości.GetLength(1); j++)
                    if (j != indeksJRozwiązania)
                    {
                        double element = odległości[indeksIRozwiązania, j];

                        if (element < minimumWWierszu)
                            minimumWWierszu = element;
                    }

                double minimumWKolumnie = Double.MaxValue;

                for (int i = 0; i < odległości.GetLength(0); i++)
                    if (i != indeksIRozwiązania)
                    {
                        double element = odległości[i, indeksJRozwiązania];

                        if (element < minimumWKolumnie)
                            minimumWKolumnie = element;
                    }

                if (minimumWWierszu == Double.MaxValue)
                    minimumWWierszu = 0;

                if (minimumWKolumnie == Double.MaxValue)
                    minimumWKolumnie = 0;

                double strata = minimumWWierszu + minimumWKolumnie;

                if (strata > strataPrzyNiewykorzystaniuRozwiązania)
                {
                    strataPrzyNiewykorzystaniuRozwiązania = strata;
                    najbardziejOpłacalneRozwiązanie = rozwiązanie;
                }
            }

            return najbardziejOpłacalneRozwiązanie;
        }

        static void Main(string[] args)
        {
            double[,] odległościPoczątkowe;
            int miastoStartu = 0;

            {
                odległościPoczątkowe = new double[,]
                {
                    {1, 10, 8, 9, 7},
                    {10, 1, 10, 5, 6},
                    {8, 10, 1, 8, 9},
                    {9, 5,  8, 1, 6},
                    {7, 6, 9, 6, 1}
                };
            }

            /*{
                odległościPoczątkowe = new double[,]
                {
                    {1, 3, 93, 13, 33, 9},
                    {4, 1, 77, 42, 21, 16},
                    {45, 17, 1, 36, 16, 28},
                    {39, 90, 80, 1, 56, 7},
                    {28, 46, 88, 33, 1, 25},
                    {3, 88, 18, 46, 92, 1}
                };
            }*/

            /*{
                int ilośćMiast = miasta.Length;
                odległościPoczątkowe = new double[ilośćMiast, ilośćMiast];
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
                            odległościPoczątkowe[i, j] = ObliczOdległość(położeniaMiast[miasta[i]], położeniaMiast[miasta[j]]);
            }*/

            int rozmiar = odległościPoczątkowe.GetLength(0);
            double[,] kopiaOdległości = new double[rozmiar, rozmiar];

            for (int i = 0; i < rozmiar; i++)
                odległościPoczątkowe[i, i] = Double.PositiveInfinity;

            for (int i = 0; i < rozmiar; i++)
                for (int j = 0; j < rozmiar; j++)
                    kopiaOdległości[i, j] = odległościPoczątkowe[i, j];

            List<Węzeł> węzłyDrzewa = new List<Węzeł>();
            List<ElementZerowy> elementyZerowe = Redukcja(kopiaOdległości);
            double strata = 1;
            Węzeł węzeł = null;

            węzłyDrzewa.Add(new Węzeł(kopiaOdległości, elementyZerowe.Sum(e => e.PoprzedniaWartość)));

            while (strata > 0)
            {
                węzłyDrzewa = węzłyDrzewa.OrderBy(w => w.LB).ThenByDescending(w => w.WybraneRozwiązania.Count).ToList();
                węzeł = węzłyDrzewa.First();

                węzłyDrzewa.Remove(węzeł);

                double[,] odległości = węzeł.Odległości;
                ElementZerowy najlepszeRozwiązanie = ZnajdźNajbardziejOpłacalneRozwiązanie(odległości, węzeł.ElementyZerowe, out strata);
                double[,] odległościPoWybraniuRozwiązania = WybierzRozwiązanie(odległości, najlepszeRozwiązanie, węzeł.WybraneRozwiązania);
                double[,] odległościPoNiewybraniuRozwiązania = NieWybierajRozwiązania(odległości, najlepszeRozwiązanie);
                List<ElementZerowy> elementyZerowePoWybraniuRozwiązania = Redukcja(odległościPoWybraniuRozwiązania);
                List<ElementZerowy> elementyZerowePoNiewybraniuRozwiązania = Redukcja(odległościPoNiewybraniuRozwiązania);

                if (elementyZerowePoNiewybraniuRozwiązania.Sum(e => e.PoprzedniaWartość) != strata)
                    throw new Exception();

                double lB = węzeł.LB;
                Węzeł węzełPoWybraniuRozwiązania = new Węzeł(odległościPoWybraniuRozwiązania, lB + elementyZerowePoWybraniuRozwiązania.Sum(e => e.PoprzedniaWartość), najlepszeRozwiązanie, węzeł);
                Węzeł węzełPoNiewybraniuRozwiązania = new Węzeł(odległościPoNiewybraniuRozwiązania, lB + strata, węzeł);

                węzeł.Dzieci.Add(węzełPoWybraniuRozwiązania);
                węzeł.Dzieci.Add(węzełPoNiewybraniuRozwiązania);
                węzłyDrzewa.Add(węzełPoWybraniuRozwiązania);
                węzłyDrzewa.Add(węzełPoNiewybraniuRozwiązania);
            }

            Dictionary<int, List<int>> połączenia = new Dictionary<int, List<int>>();

            for (int i = 0; i < rozmiar; i++)
                połączenia.Add(i, new List<int>());

            foreach (ElementZerowy rozwiązanie in węzeł.WybraneRozwiązania)
            {
                int i = rozwiązanie.I;
                int j = rozwiązanie.J;

                połączenia[i].Add(j);
                połączenia[j].Add(i);
            }

            if (węzeł.ElementyZerowe.Count > 2)
            {
                ElementZerowy elementDoUsunięcia = null;

                foreach (ElementZerowy element1 in węzeł.ElementyZerowe)
                {
                    List<List<int>> drogi = new List<List<int>>();
                    IEnumerable<ElementZerowy> połączeniaBezObecnego = węzeł.ElementyZerowe.Except(new List<ElementZerowy>() { element1 });

                    foreach (ElementZerowy element2 in połączeniaBezObecnego)
                        drogi.Add(new List<int>() { element2.I, element2.J });

                    IEnumerable<List<int>> brakującePołączenia = Enumerable.Concat(połączenia.Values, drogi);
                    bool dodaćBrakującePołączenia = true;

                    for (int i = 0; i < rozmiar; i++)
                        if (brakującePołączenia.Count(p => p.Contains(i)) != 2)
                        {
                            dodaćBrakującePołączenia = false;

                            break;
                        }

                    if (dodaćBrakującePołączenia)
                    {
                        elementDoUsunięcia = element1;

                        break;
                    }
                }

                węzeł.ElementyZerowe.Remove(elementDoUsunięcia);
            }

            foreach (ElementZerowy rozwiązanie in węzeł.ElementyZerowe)
            {
                int i = rozwiązanie.I;
                int j = rozwiązanie.J;

                połączenia[i].Add(j);
                połączenia[j].Add(i);
            }

            List<int> ścieżka = new List<int> { miastoStartu };

            for (int i = 0; i < rozmiar - 1; i++)
            {
                int start = ścieżka.Last();
                int stop = połączenia[start].First(m => !ścieżka.Contains(m));

                ścieżka.Add(stop);
            }

            if (połączenia[ścieżka.Last()].Contains(miastoStartu))
                ścieżka.Add(miastoStartu);
            else
                throw new Exception();

            double koszt = 0;

            for (int i = 1; i < ścieżka.Count; i++)
                koszt += odległościPoczątkowe[ścieżka[i - 1], ścieżka[i]];
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