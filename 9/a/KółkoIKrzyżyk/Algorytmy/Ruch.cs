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
        int _zwycięskaLiczbaPól;
        int _rozmiar;

        public Ruch(bool kółko, int głębokość, int zwycięskaLiczbaPól)
        {
            _perspektywaKółka = kółko;
            _głębokość = głębokość;
            _zwycięskaLiczbaPól = zwycięskaLiczbaPól;
        }

        public bool AlfaBetaObcięcie(Pole[,] stanGry, out WynikGry wynik, out int a, out int b, out KierunekZwycięskiejLinii kierunek)
        {
            double punktyKółka;
            double punktyKrzyżyka;
            _rozmiar = stanGry.GetLength(0);

            NastępnyAlfaBetaObcięcie(stanGry, true, _perspektywaKółka, _głębokość, Double.NegativeInfinity, Double.PositiveInfinity, out a, out b);

            return GraZakończona(stanGry, out punktyKółka, out punktyKrzyżyka, out wynik, out kierunek);
        }

        double NastępnyAlfaBetaObcięcie(Pole[,] stanGry, bool maksymalizacja, bool ruchKółka, int głębokość, double alfa, double beta, out int a, out int b)
        {
            double punktyKółka;
            double punktyKrzyżyka;
            WynikGry wynik;
            KierunekZwycięskiejLinii kierunek;
            a = b = -1;

            if (GraZakończona(stanGry, out punktyKółka, out punktyKrzyżyka, out wynik, out kierunek) || głębokość == 0)
                return WartośćWyniku(punktyKółka, punktyKrzyżyka);

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

                for (int i = 0; i < _rozmiar; i++)
                {
                    for (int j = 0; j < _rozmiar; j++)
                        if (stanGry[i, j] == Pole.Puste)
                        {
                            Pole[,] możliwyStan = KopiujStanGry(stanGry);
                            możliwyStan[i, j] = znakAktualnegoGracza;
                            double opłacalnośćRozwiązania = NastępnyAlfaBetaObcięcie(możliwyStan, false, !ruchKółka, głębokość - 1, alfa, beta, out a, out b);
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

                for (int i = 0; i < _rozmiar; i++)
                {
                    for (int j = 0; j < _rozmiar; j++)
                        if (stanGry[i, j] == Pole.Puste)
                        {
                            Pole[,] możliwyStan = KopiujStanGry(stanGry);
                            możliwyStan[i, j] = znakAktualnegoGracza;
                            double opłacalnośćRozwiązania = NastępnyAlfaBetaObcięcie(możliwyStan, true, !ruchKółka, głębokość - 1, alfa, beta, out a, out b);
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
            a = ekstremalneI;
            b = ekstremalneJ;

            return ekstremum;
        }

        double WartośćWyniku(double punktyKółka, double punktyKrzyżyka)
        {
            if (_perspektywaKółka)
                return punktyKółka - punktyKrzyżyka;
            else
                return punktyKrzyżyka - punktyKółka;
        }

        public bool GraZakończona(Pole[,] stanGry, out double punktyKółka, out double punktyKrzyżyka, out WynikGry wynik, out KierunekZwycięskiejLinii kierunek)
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
                    {
                        kierunek = KierunekZwycięskiejLinii.Poziomy;

                        return true;
                    }
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
                    {
                        kierunek = KierunekZwycięskiejLinii.Pionowy;

                        return true;
                    }
                }

            {
                int ostatniSprawdzanyIndeks = rozmiar - _zwycięskaLiczbaPól;

                for (int i = 0; i <= ostatniSprawdzanyIndeks; i++)
                    for (int j = 0; j <= ostatniSprawdzanyIndeks; j++)
                    {
                        int liczbaKółek = 0;
                        int liczbaKrzyżyków = 0;
                        int liczbaPustych = 0;

                        for (int k = 0; k < _zwycięskaLiczbaPól; k++)
                            SprawdźZawartośćPola(stanGry[i + k, j + k], ref liczbaKółek, ref liczbaKrzyżyków, ref liczbaPustych);

                        if (OkreślZwycięzcę(liczbaKółek, liczbaKrzyżyków, liczbaPustych, ref punktyKółka, ref punktyKrzyżyka, out wynik))
                        {
                            kierunek = KierunekZwycięskiejLinii.UkośnieOdLewejDoPrawej;

                            return true;
                        }
                    }
            }

            {
                int ostatniSprawdzanyIndeksWierszy = rozmiar - _zwycięskaLiczbaPól;
                int pierwszySprawdzanyIndeksKolumny = rozmiar - 1;
                int ostatniSprawdzanyIndeksKolumn = _zwycięskaLiczbaPól - 1;

                for (int i = 0; i <= ostatniSprawdzanyIndeksWierszy; i++)
                    for (int j = pierwszySprawdzanyIndeksKolumny; j >= ostatniSprawdzanyIndeksKolumn; j--)
                    {
                        int liczbaKółek = 0;
                        int liczbaKrzyżyków = 0;
                        int liczbaPustych = 0;

                        for (int k = 0; k < _zwycięskaLiczbaPól; k++)
                            SprawdźZawartośćPola(stanGry[i + k, j - k], ref liczbaKółek, ref liczbaKrzyżyków, ref liczbaPustych);

                        if (OkreślZwycięzcę(liczbaKółek, liczbaKrzyżyków, liczbaPustych, ref punktyKółka, ref punktyKrzyżyka, out wynik))
                        {
                            kierunek = KierunekZwycięskiejLinii.UkośnieOdPrawejDoLewej;
                            
                            return true;
                        }
                    }
            }

            bool istniejąPustePola = false;

            foreach (Pole pole in stanGry)
                if (pole == Pole.Puste)
                {
                    istniejąPustePola = true;

                    break;
                }

            kierunek = KierunekZwycięskiejLinii.BrakWygranej;

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
            int potęga = 3;
            
            if (liczbaKółekWLinii > 0 && liczbaKrzyżykówWLinii == 0 && liczbaPustych == _zwycięskaLiczbaPól - liczbaKółekWLinii)
            {
                punktyKółka += Math.Pow(Convert.ToDouble(liczbaKółekWLinii) / _zwycięskaLiczbaPól * 100, potęga);

                if (liczbaKółekWLinii == _zwycięskaLiczbaPól)
                {
                    punktyKrzyżyka = 0;
                    wynik = WynikGry.Kółko;

                    return true;
                }
            }

            if (liczbaKółekWLinii == 0 && liczbaKrzyżykówWLinii > 0 && liczbaPustych == _zwycięskaLiczbaPól - liczbaKrzyżykówWLinii)
            {
                punktyKrzyżyka += Math.Pow(Convert.ToDouble(liczbaKrzyżykówWLinii) / _zwycięskaLiczbaPól * 100, potęga);

                if (liczbaKrzyżykówWLinii == _zwycięskaLiczbaPól)
                {
                    punktyKółka = 0;
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