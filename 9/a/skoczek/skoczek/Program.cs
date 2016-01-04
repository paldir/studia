using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace skoczek
{
    class Program
    {
        const int M = 30;
        const int N = 30;
        const int LiczbaPól = M * N;
        const int NumerPrzedostatniegoKroku = LiczbaPól - 1;
        static readonly int[] X = new int[] { 1, 2, 2, 1, -1, -2, -2, -1 };
        static readonly int[] Y = new int[] { -2, -1, 1, 2, 2, 1, -1, -2 };
        static Pole[,] _szachownica;
        static readonly PorównywarkaPól Porównywarka = new PorównywarkaPól();

        static void Main()
        {
            bool sukces;
            _szachownica = new Pole[M, N];
            Stopwatch stoper = new Stopwatch();

            for (int p = 0; p < M; p++)
                for (int q = 0; q < N; q++)
                    _szachownica[p, q] = new Pole(p, q);

            for (int p = 0; p < M; p++)
                for (int q = 0; q < N; q++)
                {
                    Pole pole = _szachownica[p, q];
                    List<Pole> sąsiedzi = new List<Pole>();
                    int liczbaSąsiadów = 0;

                    for (int i = 0; i < 8; i++)
                    {
                        int np = p + X[i];
                        int nq = q + Y[i];

                        if ((np >= 0 && np < M) && (nq >= 0 && nq < N))
                        {
                            liczbaSąsiadów++;

                            sąsiedzi.Add(_szachownica[np, nq]);
                        }
                    }

                    pole.IlośćWolnychSąsiadów = liczbaSąsiadów;
                    pole.Sąsiedzi = sąsiedzi;
                }


            stoper.Start();
            //PróbujOtwarte(_szachownica[0, 0], 1, out sukces);
            PróbujZamknięte(_szachownica[M / 2, N / 2], 1, out sukces);
            stoper.Stop();
            WyświetlSzachownicę();
            Console.WriteLine(stoper.Elapsed);
            Console.ReadKey();
        }

        static void PróbujOtwarte(Pole pole, int k, out bool sukces)
        {
            pole.NumerKroku = k;
            List<Pole> sąsiedzi = pole.Sąsiedzi;
            sukces = k == LiczbaPól;

            foreach (Pole sąsiad in sąsiedzi)
                sąsiad.IlośćWolnychSąsiadów--;

            if (!sukces)
            {
                bool sukcesLokalny = false;

                sąsiedzi.Sort(Porównywarka);

                foreach (Pole sąsiad in sąsiedzi)
                {
                    if (sąsiad.NumerKroku == 0 && (sąsiad.IlośćWolnychSąsiadów > 0 || k == NumerPrzedostatniegoKroku))
                    {
                        PróbujOtwarte(sąsiad, k + 1, out sukcesLokalny);

                        if (sukcesLokalny)
                            break;
                    }
                }

                if (!sukcesLokalny)
                {
                    pole.NumerKroku = 0;

                    foreach (Pole sąsiad in sąsiedzi)
                        sąsiad.IlośćWolnychSąsiadów++;
                }

                sukces = sukcesLokalny;
            }
        }

        static void PróbujZamknięte(Pole pole, int k, out bool sukces)
        {
            pole.NumerKroku = k;
            List<Pole> sąsiedzi = pole.Sąsiedzi;
            sukces = k == LiczbaPól && sąsiedzi.Exists(s => s.NumerKroku == 1);

            foreach (Pole sąsiad in sąsiedzi)
                sąsiad.IlośćWolnychSąsiadów--;

            if (!sukces)
            {
                bool sukcesLokalny = false;
                
                sąsiedzi.Sort(Porównywarka);

                foreach (Pole sąsiad in sąsiedzi)
                {
                    if (sąsiad.NumerKroku == 0 && (sąsiad.IlośćWolnychSąsiadów > 0 || k == NumerPrzedostatniegoKroku))
                    {
                        PróbujZamknięte(sąsiad, k + 1, out sukcesLokalny);

                        if (sukcesLokalny)
                            break;
                    }
                }

                if (!sukcesLokalny)
                {
                    pole.NumerKroku = 0;

                    foreach (Pole sąsiad in sąsiedzi)
                        sąsiad.IlośćWolnychSąsiadów++;
                }

                sukces = sukcesLokalny;
            }
        }

        static void WyświetlSzachownicę()
        {
            for (int p = 0; p < M; p++)
            {
                for (int q = 0; q < N; q++)
                    Console.Write("{0,4}", _szachownica[p, q].NumerKroku);

                Console.Write(Environment.NewLine);
            }
        }
    }
}