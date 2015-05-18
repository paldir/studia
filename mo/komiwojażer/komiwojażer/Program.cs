using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komiwojażer
{
    class Program
    {
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

            ElementZerowy początekŚcieżki;
            ElementZerowy koniecŚcieżki;
            ElementZerowy następnyPoczątekŚcieżki = rozwiązanie;
            ElementZerowy następnyKoniecŚcieżki = rozwiązanie;

            do
            {
                początekŚcieżki = następnyPoczątekŚcieżki;
                następnyPoczątekŚcieżki = dotychczasoweRozwiązania.Find(r => r.J == początekŚcieżki.I);
            }
            while (następnyPoczątekŚcieżki != null);

            do
            {
                koniecŚcieżki = następnyKoniecŚcieżki;
                następnyKoniecŚcieżki = dotychczasoweRozwiązania.Find(r => r.I == koniecŚcieżki.J);
            }
            while (następnyKoniecŚcieżki != null);

            odległości[koniecŚcieżki.J, początekŚcieżki.I] = Double.PositiveInfinity;

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
            int miastoStartu = 15;
            //odległościPoczątkowe = OdległościZYouTube();
            //odległościPoczątkowe = OdległościZPrezentacji();
            odległościPoczątkowe = PołożenieGeograficzne.WyznaczOdległościPomiędzyMiastami(miasta, "miasta.txt");
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
                węzłyDrzewa = węzłyDrzewa.OrderBy(w => w.LB).ToList();
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

            IEnumerable<ElementZerowy> rozwiązania = węzeł.WybraneRozwiązania.Concat(węzeł.ElementyZerowe);
            List<int> ścieżka = new List<int>() { miastoStartu };

            while (ścieżka.Count != rozmiar + 1)
            {
                ElementZerowy element = rozwiązania.First(r => r.I == ścieżka.Last());

                ścieżka.Add(element.J);
            }

            foreach (int miasto in ścieżka)
                Console.WriteLine(miasta[miasto]);

            double koszt = 0;

            for (int i = 1; i < ścieżka.Count; i++)
                koszt += odległościPoczątkowe[ścieżka[i - 1], ścieżka[i]];

            Console.WriteLine();
            Console.WriteLine("Długość trasy to: {0}", koszt);
            Console.ReadKey();
        }

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

        static double[,] OdległościZYouTube()
        {
            return new double[,]
                {
                    {1, 10, 8, 9, 7},
                    {10, 1, 10, 5, 6},
                    {8, 10, 1, 8, 9},
                    {9, 5,  8, 1, 6},
                    {7, 6, 9, 6, 1}
                };
        }

        static double[,] OdległościZPrezentacji()
        {
            return new double[,]
                {
                    {1, 3, 93, 13, 33, 9},
                    {4, 1, 77, 42, 21, 16},
                    {45, 17, 1, 36, 16, 28},
                    {39, 90, 80, 1, 56, 7},
                    {28, 46, 88, 33, 1, 25},
                    {3, 88, 18, 46, 92, 1}
                };
        }
    }
}