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
        int _zwycięskaLiczbaPól = 3;

        public Ruch(bool kółko, int głębokość, int zwycięskaLiczbaPól)
        {
            _perspektywaKółka = kółko;
            _głębokość = głębokość;
            _zwycięskaLiczbaPól = zwycięskaLiczbaPól;
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

        bool GraZakończona(Pole[,] stanGry, out double punktyKółka, out double punktyKrzyżyka, out WynikGry wynik)
        {
            int rozmiar = stanGry.GetLength(0);
            punktyKółka = punktyKrzyżyka = 0;

            for (int i = 0; i < rozmiar; i++)
                for (int j = 0; j < rozmiar; j++)
                {
                    int liczbaKółek = 0;
                    int liczbaKrzyżyków = 0;
                    int liczbaPustych = 0;
                    int początekKolejnejTrójki = j + _zwycięskaLiczbaPól;

                    for (int k = j; k < początekKolejnejTrójki && k < rozmiar; k++)
                        SprawdźZawartośćPola(stanGry[i, k], ref liczbaKółek, ref liczbaKrzyżyków, ref liczbaPustych);

                    if (OkreślZwycięzcę(liczbaKółek, liczbaKrzyżyków, liczbaPustych, ref punktyKółka, ref punktyKrzyżyka, out wynik))
                        return true;
                }

            for (int j = 0; j < rozmiar; j++)
                for (int i = 0; i < rozmiar; i++)
                {
                    int liczbaKółek = 0;
                    int liczbaKrzyżyków = 0;
                    int liczbaPustych = 0;
                    int początekKolejnejTrójki = i + _zwycięskaLiczbaPól;

                    for (int k = i; k < początekKolejnejTrójki && k < rozmiar; k++)
                        SprawdźZawartośćPola(stanGry[k, j], ref liczbaKółek, ref liczbaKrzyżyków, ref liczbaPustych);

                    if (OkreślZwycięzcę(liczbaKółek, liczbaKrzyżyków, liczbaPustych, ref punktyKółka, ref punktyKrzyżyka, out wynik))
                        return true;
                }

            /*for (int i = 0; i < rozmiar; i++)
                for (int j = 0; j < rozmiar - ZwycięskaLiczbaPól; j++)
                {
                    int liczbaKółek = 0;
                    int liczbaKrzyżyków = 0;
                    int liczbaPustych = 0;
                    int początekKolejnejTrójki = i + ZwycięskaLiczbaPól;

                    for (int k = i; k < początekKolejnejTrójki && k < rozmiar; k++)
                        SprawdźZawartośćPola(stanGry[k, k], ref liczbaKółek, ref liczbaKrzyżyków, ref liczbaPustych);

                    if (OkreślZwycięzcę(liczbaKółek, liczbaKrzyżyków, liczbaPustych, ref punktyKółka, ref punktyKrzyżyka, out wynik))
                        return true;
                }

            for (int i = 0; i < rozmiar; i++)
            {
                int liczbaKółek = 0;
                int liczbaKrzyżyków = 0;
                int liczbaPustych = 0;
                int początekKolejnejTrójki = i + ZwycięskaLiczbaPól;

                for (int k = i; k < początekKolejnejTrójki && k < rozmiar; k++)
                    SprawdźZawartośćPola(stanGry[k, rozmiar - k - 1], ref liczbaKółek, ref liczbaKrzyżyków, ref liczbaPustych);

                if (OkreślZwycięzcę(liczbaKółek, liczbaKrzyżyków, liczbaPustych, ref punktyKółka, ref punktyKrzyżyka, out wynik))
                    return true;
            }*/

            /*for (int i = 0; i < rozmiar; i++)
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
            }*/

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

        static void SprawdźZawartośćPola(Pole pole, ref int liczbaKółek, ref int liczbaKrzyżyków, ref int liczbaPustych)
        {
            if (pole == Pole.Kółko)
                liczbaKółek++;
            else if (pole == Pole.Krzyżyk)
                liczbaKrzyżyków++;
            else
                liczbaPustych++;
        }

        bool OkreślZwycięzcę(int liczbaKółekWLinii, int liczbaKrzyżykówWLinii, int liczbaPustych, ref double punktyKółka, ref double punktyKrzyżyka, out WynikGry wynik)
        {
            if (liczbaKółekWLinii > 0 && liczbaKrzyżykówWLinii == 0 && liczbaPustych == _zwycięskaLiczbaPól - liczbaKółekWLinii)
            {
                if (liczbaKółekWLinii == _zwycięskaLiczbaPól)
                {
                    punktyKółka = Double.MaxValue;
                    punktyKrzyżyka = 0;
                    wynik = WynikGry.Kółko;

                    return true;
                }
                
                punktyKółka += Math.Pow(Convert.ToDouble(liczbaKółekWLinii) / _zwycięskaLiczbaPól * 100, 2);
            }

            if (liczbaKółekWLinii == 0 && liczbaKrzyżykówWLinii > 0 && liczbaPustych == _zwycięskaLiczbaPól - liczbaKrzyżykówWLinii)
            {
                if (liczbaKrzyżykówWLinii == _zwycięskaLiczbaPól)
                {
                    punktyKółka = 0;
                    punktyKrzyżyka = Double.MaxValue;
                    wynik = WynikGry.Krzyżyk;

                    return true;
                }

                punktyKrzyżyka += Math.Pow(Convert.ToDouble(liczbaKrzyżykówWLinii) / _zwycięskaLiczbaPól * 100, 2);
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