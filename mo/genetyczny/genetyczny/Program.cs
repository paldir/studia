﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace genetyczny
{
    class Program
    {
        const double epsilon = Double.Epsilon * 2;
        
        static List<Osobnik> LosujPopulację(int liczbaOsobników)
        {
            List<Osobnik> populacja = new List<Osobnik>();
            int długośćChromosomu = miasta.Length;
            Random los = new Random();

            for (int i = 0; i < liczbaOsobników; i++)
            {
                int[] geny = new int[długośćChromosomu];
                List<int> dostępneGeny = new List<int>();

                for (int j = 0; j < długośćChromosomu; j++)
                    dostępneGeny.Add(j);

                while (dostępneGeny.Any())
                {
                    int liczbaDostępnychGenów = dostępneGeny.Count;
                    int wylosowanyGen = los.Next(0, liczbaDostępnychGenów);

                    geny[liczbaDostępnychGenów - 1] = dostępneGeny[wylosowanyGen];

                    dostępneGeny.RemoveAt(wylosowanyGen);
                }

                populacja.Add(new Osobnik(geny, DługośćTrasy(geny)));
            }

            return populacja;
        }

        static double DługośćTrasy(int[] geny)
        {
            double odległość = 0;
            int liczbaGenów = geny.Length;

            for (int i = 0; i < liczbaGenów - 1; i++)
                odległość += odległości[geny[i], geny[i + 1]];

            odległość += odległości[geny[liczbaGenów - 1], geny[0]];

            return odległość;
        }

        static void Krzyżuj(Osobnik mama, Osobnik tata, out Osobnik nowaMama, out Osobnik nowyTata)
        {
            int[] genyMamy = mama.Geny;
            int[] genyTaty = tata.Geny;
            int ilośćGenów = genyMamy.Length;
            Random los = new Random();
            int liczbaStałychGenów = los.Next(2, 4);
            int indeks1 = los.Next(0, ilośćGenów - liczbaStałychGenów);
            int indeks2 = indeks1 + liczbaStałychGenów;
            int[] noweGenyMamy = new int[ilośćGenów];
            int[] noweGenyTaty = new int[ilośćGenów];

            for (int i = 0; i < ilośćGenów; i++)
                noweGenyMamy[i] = noweGenyTaty[i] = -1;

            while (indeks2 == indeks1)
                indeks2 = los.Next(0, ilośćGenów);

            if (indeks1 > indeks2)
            {
                int tmp = indeks1;
                indeks1 = indeks2;
                indeks2 = tmp;
            }

            for (int i = indeks1; i < indeks2; i++)
            {
                noweGenyMamy[i] = genyMamy[i];
                noweGenyTaty[i] = genyTaty[i];
            }

            for (int i = 0; i < indeks1; i++)
            {
                int tmp = genyMamy.First(m => !noweGenyTaty.Contains(m));
                noweGenyMamy[i] = genyTaty.First(t => !noweGenyMamy.Contains(t));
                noweGenyTaty[i] = tmp;
            }

            for (int i = indeks2; i < ilośćGenów; i++)
            {
                int tmp = genyMamy.First(m => !noweGenyTaty.Contains(m));
                noweGenyMamy[i] = genyTaty.First(t => !noweGenyMamy.Contains(t));
                noweGenyTaty[i] = tmp;
            }

            nowaMama = new Osobnik(noweGenyMamy, DługośćTrasy(noweGenyMamy));
            nowyTata = new Osobnik(noweGenyTaty, DługośćTrasy(noweGenyTaty));
        }

        static void Main(string[] args)
        {
            odległości = PołożenieGeograficzne.WyznaczOdległościPomiędzyMiastami(miasta, "miasta.txt");
            List<Osobnik> populacja = LosujPopulację(miasta.Length * 10);
            Random los = new Random();
            bool jedziemy = true;

            for (int ź = 0; jedziemy; ź++)
            {
                double sumaPrzystosowania = populacja.Sum(o => o.Przystosowanie);

                foreach (Osobnik osobnik in populacja)
                    osobnik.PrzystosowanieProcentowo = osobnik.Przystosowanie / sumaPrzystosowania * 100;

                populacja.Sort();

                List<KawałekRuletki> kawałkiRuletki = new List<KawałekRuletki>() { new KawałekRuletki(0, populacja.First().PrzystosowanieProcentowo) };

                for (int i = 1; i < populacja.Count - 1; i++)
                {
                    double początek = kawałkiRuletki.Last().Koniec;

                    kawałkiRuletki.Add(new KawałekRuletki(początek, początek + populacja[i].PrzystosowanieProcentowo));
                }

                kawałkiRuletki.Add(new KawałekRuletki(kawałkiRuletki.Last().Koniec, 100));

                List<Osobnik> nowePokolenie = new List<Osobnik>();
                int liczebnośćPopulacji = populacja.Count;

                for (int i = 0; i < Math.Ceiling(liczebnośćPopulacji / 2.0); i++)
                {
                    double fartMamy = los.NextDouble() * 100;
                    double fartTaty = los.NextDouble() * 100;
                    KawałekRuletki ruletkaMamy = kawałkiRuletki.Single(k => k._Początek <= fartMamy && k.Koniec > fartMamy);
                    KawałekRuletki ruletkaTaty = kawałkiRuletki.Single(k => k._Początek <= fartTaty && k.Koniec > fartTaty);
                    Osobnik mama = populacja[kawałkiRuletki.IndexOf(ruletkaMamy)];
                    Osobnik tata = populacja[kawałkiRuletki.IndexOf(ruletkaTaty)];
                    Osobnik nowaMama;
                    Osobnik nowyTata;

                    Krzyżuj(mama, tata, out nowaMama, out nowyTata);
                    nowePokolenie.Add(nowaMama);
                    nowePokolenie.Add(nowyTata);
                }

                double długośćTrasyStara = populacja.Sum(p => p.DługośćTrasy);
                double długośćTrasyNowa = nowePokolenie.Sum(p => p.DługośćTrasy);

                if (Math.Abs(długośćTrasyStara - długośćTrasyNowa) <= epsilon)
                    jedziemy = false;

                populacja = nowePokolenie;

                foreach (Osobnik osobnik in populacja)
                {
                    int[] geny = osobnik.Geny;

                    for (int i = 0; i < geny.Length - 1; i++)
                        if (los.Next(1000 * liczebnośćPopulacji) == 1)
                        {
                            int tmp = geny[i];
                            geny[i] = geny[i + 1];
                            geny[i + 1] = tmp;
                        }
                }
            }

            double trasa = populacja.Min(p => p.DługośćTrasy);

            foreach (int numer in populacja.First(p => p.DługośćTrasy == trasa).Geny)
                Console.WriteLine(miasta[numer]);

            Console.WriteLine("\nDługość trasy: {0}", trasa);
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
            "Szczecin", 
            "Bydgoszcz",
            "Lublin",
            "Katowice"/*,
            "Białystok",
            "Gdynia", 
            "Częstochowa",
            "Radom",
            "Sosnowiec", 
            "Toruń"*/
        };
    }
}
