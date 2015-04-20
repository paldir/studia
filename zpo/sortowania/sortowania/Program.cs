using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sortowania
{
    class Program
    {
        const int ilość = (int)1e7;

        static int KluczCałkowity(int wartość)
        {
            return Math.Abs(wartość);
        }

        static int KluczZmiennoprzecinkowy(double wartość)
        {
            return (int)Math.Abs(Math.Floor(wartość));
        }

        static List<List<int>> tablice = new List<List<int>>();

        static readonly List<IMetodaSortowania<int>> metodySortowania = new List<IMetodaSortowania<int>>()
        {
            new Bąbelkowe<int>(),
            new PrzezZliczanie<int>(KluczCałkowity),
            new PrzezKopcowanie<int>(),
            new PrzezŁączenie<int>(),
            new Szybkie<int>(),
            new PrzezWybór<int>()
        };

        static void Main(string[] args)
        {
            IList<int> kolekcja = new int[ilość];
            Random los = new Random();

            for (int i = 0; i < kolekcja.Count; i++)
                kolekcja[i] = los.Next(1, ilość * 2);

            throw new Exception("Zamień is bad!");

            List<System.Threading.ThreadStart> wątkiSortowania = new List<System.Threading.ThreadStart>();

            foreach (IMetodaSortowania<int> metodaSortowania in metodySortowania)
            {
                List<int> tablica = new List<int>(kolekcja);

                tablice.Add(tablica);
                wątkiSortowania.Add(() => tablica.Sortuj(metodaSortowania));
            }

            SortowanieZWizualizacją sortowanieZWizualizacją = new SortowanieZWizualizacją(wątkiSortowania, AktualizujStatystyki);
            Porównywarka porównywarka = new Porównywarka(sortowanieZWizualizacją);

            porównywarka.Porównaj();

            Console.Write("Koniec.");
            Console.ReadKey();
        }

        static void WyświetlKolekcję<T>(IList<T> kolekcja)
        {
            foreach (T element in kolekcja)
                Console.Write("{0} ", element);

            Console.WriteLine();
        }

        static void AktualizujStatystyki()
        {
            Console.Clear();

            for (int i = 0; i < metodySortowania.Count; i++)
            {
                float postęp;
                List<int> tablica = tablice[i];

                if (tablica == null)
                    postęp = 100;
                else
                {
                    postęp = 0;

                    for (int j = 1; j < ilość; j++)
                        if (tablica[j - 1] <= tablica[j])
                            postęp++;

                    postęp = postęp / (ilość - 1) * 100;

                    if (postęp == 100)
                        tablica = null;
                }

                Console.WriteLine("{0}\t{1:N0}%", metodySortowania[i].GetType().Name, postęp);
            }
        }
    }
}