using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace genetyczny
{
    class Program
    {
        static List<int[]> LosujPopulację(int liczbaOsobników)
        {
            List<int[]> populacja = new List<int[]>();
            int długośćChromosomu = miasta.Length;
            Random los = new Random();

            for (int i = 0; i < liczbaOsobników; i++)
            {
                int[] osobnik = new int[długośćChromosomu];
                List<int> dostępneGeny = new List<int>();

                for (int j = 0; j < długośćChromosomu; j++)
                    dostępneGeny.Add(j);

                while (dostępneGeny.Any())
                {
                    int liczbaDostępnychGenów = dostępneGeny.Count;
                    int wylosowanyGen = los.Next(0, liczbaDostępnychGenów);

                    osobnik[liczbaDostępnychGenów - 1] = dostępneGeny[wylosowanyGen];

                    dostępneGeny.RemoveAt(wylosowanyGen);
                }

                populacja.Add(osobnik);
            }

            return populacja;
        }

        static double DługośćTrasy(int[] osobnik)
        {
            double odległość = 0;
            int liczbaGenów = osobnik.Length;

            for (int i = 0; i < liczbaGenów - 1; i++)
                odległość += odległości[osobnik[i], osobnik[i + 1]];

            odległość += odległości[osobnik[liczbaGenów - 1], osobnik[0]];

            return odległość;
        }

        static void Krzyżuj(int[] mama, int[] tata)
        {
            int ilośćGenów = mama.Length;
            Random los = new Random();
            int indeks1 = los.Next(0, ilośćGenów);
            int indeks2 = indeks1;
            int[] nowaMama = new int[ilośćGenów];
            int[] nowyTata = new int[ilośćGenów];

            for (int i = 0; i < ilośćGenów; i++)
                nowaMama[i] = nowyTata[i] = -1;

            while (indeks2 == indeks1)
                indeks2 = los.Next(0, ilośćGenów);

            if (indeks1 > indeks2)
            {
                int tmp = indeks1;
                indeks1 = indeks2;
                indeks2 = tmp;
            }

            for (int i = indeks1; i <= indeks2; i++)
            {
                int tmp = mama[i];
                nowaMama[i] = tata[i];
                nowyTata[i] = tmp;
            }

            for (int i = 0; i < indeks1; i++)
            {
                int tmp = mama.First(m => !nowyTata.Contains(m));
                nowaMama[i] = tata.First(t => !nowaMama.Contains(t));
                nowyTata[i] = tmp;
            }

            for (int i = indeks2 + 1; i < ilośćGenów; i++)
            {
                int tmp = mama.First(m => !nowyTata.Contains(m));
                nowaMama[i] = tata.First(t => !nowaMama.Contains(t));
                nowyTata[i] = tmp;
            }
        }

        static void Main(string[] args)
        {
            odległości = PołożenieGeograficzne.WyznaczOdległościPomiędzyMiastami(miasta, "miasta.txt");

            List<int[]> populacja = LosujPopulację(5);
            IEnumerable<double> długościTras = populacja.Select(o => DługośćTrasy(o));
            IEnumerable<double> przystosowanie = długościTras.Select(d => 1 / d);
            double sumaPrzystosowania = przystosowanie.Sum();
            przystosowanie = przystosowanie.Select(p => p / sumaPrzystosowania * 100);

            Krzyżuj(populacja[0], populacja[1]);
        }

        static double[,] odległości;

        static readonly string[] miasta = new string[]
        {
            "Warszawa", 
            "Kraków", 
            "Łódź", 
            "Wrocław",
            "Poznań",
            "Gdańsk", 
            "Szczecin"/*, 
            "Bydgoszcz",
            "Lublin",
            "Katowice",
            "Białystok",
            "Gdynia", 
            "Częstochowa",
            "Radom",
            "Sosnowiec", 
            "Toruń"*/
        };
    }
}
