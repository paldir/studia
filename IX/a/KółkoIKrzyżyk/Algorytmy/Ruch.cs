using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorytmy
{
    public class Ruch
    {
        bool _perspektywaKółka;
        int _głębokość;

        public Ruch(bool kółko, int głębokość)
        {
            _perspektywaKółka = kółko;
            _głębokość = głębokość;
        }

        public bool Minimax(Pole[,] stanGry, out WynikGry wynik)
        {
            double punktyKółka;
            double punktyKrzyżyka;

            NastępnyMinimax(stanGry, true, _perspektywaKółka, _głębokość);

            return GraZakończona(stanGry, out punktyKółka, out punktyKrzyżyka, out wynik);
        }

        public bool AlfaBetaObcięcie(Pole[,] stanGry, out WynikGry wynik)
        {
            double punktyKółka;
            double punktyKrzyżyka;

            NastępnyAlfaBetaObcięcie(stanGry, true, _perspektywaKółka, _głębokość, Double.NegativeInfinity, Double.PositiveInfinity);

            return GraZakończona(stanGry, out punktyKółka, out punktyKrzyżyka, out wynik);
        }

        double NastępnyMinimax(Pole[,] stanGry, bool maksymalizacja, bool ruchKółka, int głębokość)
        {
            double punktyKółka;
            double punktyKrzyżyka;
            WynikGry wynik;

            if (GraZakończona(stanGry, out punktyKółka, out punktyKrzyżyka, out wynik) || głębokość == 0)
                return WartośćWyniku(punktyKółka, punktyKrzyżyka);

            int rozmiar = stanGry.GetLength(0);
            Pole znakAktualnegoGracza;

            if (ruchKółka)
                znakAktualnegoGracza = Pole.Kółko;
            else
                znakAktualnegoGracza = Pole.Krzyżyk;

            Func<double, double, bool> funkcjaPorównująca;
            double ekstremum;
            int ekstremalneI = -1;
            int ekstremalneJ = -1;

            if (maksymalizacja)
            {
                ekstremum = Single.NegativeInfinity;
                funkcjaPorównująca = (x, y) => x > y;
            }
            else
            {
                ekstremum = Single.PositiveInfinity;
                funkcjaPorównująca = (x, y) => x < y;
            }

            for (int i = 0; i < rozmiar; i++)
                for (int j = 0; j < rozmiar; j++)
                    if (stanGry[i, j] == Pole.Puste)
                    {
                        Pole[,] możliwyStan = KopiujStanGry(stanGry);
                        możliwyStan[i, j] = znakAktualnegoGracza;
                        double opłacalnośćRozwiązania = NastępnyMinimax(możliwyStan, !maksymalizacja, !ruchKółka, głębokość - 1);

                        if (funkcjaPorównująca(opłacalnośćRozwiązania, ekstremum))
                        {
                            ekstremum = opłacalnośćRozwiązania;
                            ekstremalneI = i;
                            ekstremalneJ = j;
                        }
                    }

            stanGry[ekstremalneI, ekstremalneJ] = znakAktualnegoGracza;

            return ekstremum;
        }

        double NastępnyAlfaBetaObcięcie(Pole[,] stanGry, bool maksymalizacja, bool ruchKółka, int głębokość, double alfa, double beta)
        {
            double punktyKółka;
            double punktyKrzyżyka;
            WynikGry wynik;

            if (GraZakończona(stanGry, out punktyKółka, out punktyKrzyżyka, out wynik) || głębokość == 0)
                return WartośćWyniku(punktyKółka, punktyKrzyżyka);

            int rozmiar = stanGry.GetLength(0);
            Pole znakAktualnegoGracza;

            if (ruchKółka)
                znakAktualnegoGracza = Pole.Kółko;
            else
                znakAktualnegoGracza = Pole.Krzyżyk;

            int ekstremalneI = -1;
            int ekstremalneJ = -1;
            double ekstremum = 0;

            if (maksymalizacja)
            {
                ekstremum = Single.NegativeInfinity;

                for (int i = 0; i < rozmiar; i++)
                {
                    for (int j = 0; j < rozmiar; j++)
                        if (stanGry[i, j] == Pole.Puste)
                        {
                            Pole[,] możliwyStan = KopiujStanGry(stanGry);
                            możliwyStan[i, j] = znakAktualnegoGracza;
                            double opłacalnośćRozwiązania = NastępnyAlfaBetaObcięcie(możliwyStan, false, !ruchKółka, głębokość - 1, alfa, beta);
                            alfa = Math.Max(alfa, ekstremum);

                            if (opłacalnośćRozwiązania > ekstremum)
                            {
                                ekstremum = opłacalnośćRozwiązania;
                                ekstremalneI = i;
                                ekstremalneJ = j;
                            }

                            if (beta <= alfa)
                                break;
                        }

                    if (beta <= alfa)
                        break;
                }
            }
            else
            {
                ekstremum = Single.PositiveInfinity;

                for (int i = 0; i < rozmiar; i++)
                {
                    for (int j = 0; j < rozmiar; j++)
                        if (stanGry[i, j] == Pole.Puste)
                        {
                            Pole[,] możliwyStan = KopiujStanGry(stanGry);
                            możliwyStan[i, j] = znakAktualnegoGracza;
                            double opłacalnośćRozwiązania = NastępnyAlfaBetaObcięcie(możliwyStan, true, !ruchKółka, głębokość - 1, alfa, beta);
                            beta = Math.Min(beta, ekstremum);

                            if (opłacalnośćRozwiązania < ekstremum)
                            {
                                ekstremum = opłacalnośćRozwiązania;
                                ekstremalneI = i;
                                ekstremalneJ = j;
                            }

                            if (beta <= alfa)
                                break;
                        }

                    if (beta <= alfa)
                        break;
                }
            }

            stanGry[ekstremalneI, ekstremalneJ] = znakAktualnegoGracza;

            return ekstremum;
        }

        double WartośćWyniku(double punktyKółka, double punktyKrzyżyka)
        {
            if (_perspektywaKółka)
                return punktyKółka - punktyKrzyżyka;
            else
                return punktyKrzyżyka - punktyKółka;
        }

        static bool GraZakończona(Pole[,] stanGry, out double punktyKółka, out double punktyKrzyżyka, out WynikGry wynik)
        {
            int rozmiar = stanGry.GetLength(0);
            punktyKółka = punktyKrzyżyka = 0;

            for (int i = 0; i < rozmiar; i++)
            {
                int liczbaKółek = 0;
                int liczbaKrzyżyków = 0;

                for (int j = 0; j < rozmiar; j++)
                    SprawdźZawartośćPola(stanGry[i, j], ref liczbaKółek, ref liczbaKrzyżyków);

                if (OkreślZwycięzcę(liczbaKółek, liczbaKrzyżyków, rozmiar, ref punktyKółka, ref punktyKrzyżyka, out wynik))
                    return true;
            }

            for (int j = 0; j < rozmiar; j++)
            {
                int liczbaKółek = 0;
                int liczbaKrzyżyków = 0;

                for (int i = 0; i < rozmiar; i++)
                    SprawdźZawartośćPola(stanGry[i, j], ref liczbaKółek, ref liczbaKrzyżyków);

                if (OkreślZwycięzcę(liczbaKółek, liczbaKrzyżyków, rozmiar, ref punktyKółka, ref punktyKrzyżyka, out wynik))
                    return true;
            }

            {
                int liczbaKółek = 0;
                int liczbaKrzyżyków = 0;

                for (int i = 0; i < rozmiar; i++)
                    SprawdźZawartośćPola(stanGry[i, i], ref liczbaKółek, ref liczbaKrzyżyków);

                if (OkreślZwycięzcę(liczbaKółek, liczbaKrzyżyków, rozmiar, ref punktyKółka, ref punktyKrzyżyka, out wynik))
                    return true;
            }

            {
                int liczbaKółek = 0;
                int liczbaKrzyżyków = 0;

                for (int i = 0; i < rozmiar; i++)
                    SprawdźZawartośćPola(stanGry[i, rozmiar - 1 - i], ref liczbaKółek, ref liczbaKrzyżyków);

                if (OkreślZwycięzcę(liczbaKółek, liczbaKrzyżyków, rozmiar, ref punktyKółka, ref punktyKrzyżyka, out wynik))
                    return true;
            }

            bool istniejąPustePola = false;

            foreach (Pole pole in stanGry)
                if (pole == Pole.Puste)
                {
                    istniejąPustePola = true;

                    break;
                }

            if (!istniejąPustePola)
            {
                wynik = WynikGry.Remis;

                return true;
            }

            wynik = WynikGry.Trwająca;

            return false;
        }

        static void SprawdźZawartośćPola(Pole pole, ref int liczbaKółek, ref int liczbaKrzyżyków)
        {
            if (pole == Pole.Kółko)
                liczbaKółek++;
            else if (pole == Pole.Krzyżyk)
                liczbaKrzyżyków++;
        }

        static bool OkreślZwycięzcę(int liczbaKółekWLinii, int liczbaKrzyżykówWLinii, int długośćLinii, ref double punktyKółka, ref double punktyKrzyżyka, out WynikGry wynik)
        {
            if (liczbaKółekWLinii > 0 && liczbaKrzyżykówWLinii == 0)
            {
                punktyKółka += Convert.ToDouble(liczbaKółekWLinii) / długośćLinii * 100;

                if (liczbaKółekWLinii == długośćLinii)
                {
                    punktyKółka = Double.MaxValue;
                    punktyKrzyżyka = 0;
                    wynik = WynikGry.Kółko;

                    return true;
                }
            }

            if (liczbaKółekWLinii == 0 && liczbaKrzyżykówWLinii > 0)
            {
                punktyKrzyżyka += Convert.ToDouble(liczbaKrzyżykówWLinii) / długośćLinii * 100;

                if (liczbaKrzyżykówWLinii == długośćLinii)
                {
                    punktyKółka = 0;
                    punktyKrzyżyka = Double.MaxValue;
                    wynik = WynikGry.Krzyżyk;

                    return true;
                }
            }

            wynik = WynikGry.Remis | WynikGry.Trwająca;

            return false;
        }

        static Pole[,] KopiujStanGry(Pole[,] stanGry)
        {
            int rozmiar = stanGry.GetLength(0);
            Pole[,] kopia = new Pole[rozmiar, rozmiar];

            Array.Copy(stanGry, 0, kopia, 0, stanGry.Length);

            return kopia;
        }
    }
}