using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kieszonka
{
    class Program
    {
        const string formatNazwyPliku = "iris_2vs3_{0}_{1}.txt";
        delegate int DelegatFunkcjiF(double a);

        static void Trening(List<Pozycja> pozycjeTreningowe, DelegatFunkcjiF funkcjaF, out double[] w, out double b)
        {
            int długośćWektora = pozycjeTreningowe.First().X.Length;
            Random los = new Random();

            w = new double[długośćWektora];

            for (int i = 0; i < długośćWektora; i++)
                w[i] = los.NextDouble() * 0.4 - 0.2;

            b = los.NextDouble() * 0.4 - 0.2;
            int wiek = 0;
            int liczbaIteracji = 0;
            Kieszonka kieszonka = new Kieszonka(wiek, w, b);

            while (kieszonka.Wiek < pozycjeTreningowe.Count)
            {
                pozycjeTreningowe = pozycjeTreningowe.Permutacja();

                foreach (Pozycja pozycja in pozycjeTreningowe)
                {
                    double[] x = pozycja.X;
                    int c = pozycja.Klasa;

                    if (funkcjaF(A(w, x, b)) == c)
                        wiek++;
                    else
                    {
                        if (wiek > kieszonka.Wiek)
                            kieszonka = new Kieszonka(wiek, w, b);

                        for (int i = 0; i < długośćWektora; i++)
                            w[i] += c * x[i];

                        b += c;
                        wiek = 0;
                    }
                }

                if (liczbaIteracji > 100)
                    break;
                else
                    liczbaIteracji++;
            }

            w = kieszonka.W;
            b = kieszonka.B;
        }

        static float Test(List<Pozycja> pozycjeTestowe, DelegatFunkcjiF funkcjaF, double[] w, double b)
        {
            float testyZakończoneSukcesem = 0;

            foreach (Pozycja pozycja in pozycjeTestowe)
                if (funkcjaF(A(w, pozycja.X, b)) == pozycja.Klasa)
                    testyZakończoneSukcesem++;

            return testyZakończoneSukcesem / pozycjeTestowe.Count * 100;
        }

        static int F(double a)
        {
            return a >= 0 ? 1 : -1;
        }

        static double A(double[] w, double[] x, double b)
        {
            double a = 0;

            for (int i = 0; i < w.Length; i++)
                a += w[i] * x[i];

            return a + b;
        }

        static List<Pozycja> WczytajPozycjeZPliku(string nazwaPliku)
        {
            List<Pozycja> pozycje = new List<Pozycja>();

            using (System.IO.StreamReader strumień = new System.IO.StreamReader(nazwaPliku))
                while (!strumień.EndOfStream)
                {
                    List<string> linia = strumień.ReadLine().Split('\t').ToList();

                    pozycje.Add(new Pozycja(linia.GetRange(0, linia.Count - 1).Select(x => Double.Parse(x)), Int32.Parse(linia.Last())));
                }

            return pozycje;
        }

        static void Main(string[] args)
        {
            System.Globalization.CultureInfo infoOKulturze = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            infoOKulturze.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = infoOKulturze;
            string[] klasy = { "A", "B", "C", "D", "E" };

            foreach (string klasa in klasy)
            {
                List<Pozycja> pozycjeTreningowe = WczytajPozycjeZPliku(String.Format(formatNazwyPliku, klasa, "tr"));
                List<Pozycja> pozycjeTestowe = WczytajPozycjeZPliku(String.Format(formatNazwyPliku, klasa, "te"));
                double[] w;
                double b;

                Trening(pozycjeTreningowe, F, out w, out b);

                float procentSukcesów = Test(pozycjeTestowe, F, w, b);

                Console.WriteLine(String.Format("{0}: {1}%", klasa, procentSukcesów));
            }

            Console.ReadKey();
        }
    }
}