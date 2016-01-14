using System;
using System.Linq;

namespace Algorytmy
{
    public class Ruch
    {
        private readonly bool _perspektywaKółka;
        private readonly int _głębokość;
        private readonly int _zwycięskaLiczbaPól;
        private int _rozmiar;
        private readonly Action _inkrementacjaLiczbyPrzeanalizowanychRuchów;

        public Ruch(bool kółko, int głębokość, int zwycięskaLiczbaPól, Action inkrementacjaLiczbyPrzeanalizowanychRuchów)
        {
            _perspektywaKółka = kółko;
            _głębokość = głębokość;
            _zwycięskaLiczbaPól = zwycięskaLiczbaPól;
            _inkrementacjaLiczbyPrzeanalizowanychRuchów = inkrementacjaLiczbyPrzeanalizowanychRuchów;
        }

        public bool AlfaBetaObcięcie(Pole[,] stanGry, out WynikGry wynik, out int a, out int b, out KierunekZwycięskiejLinii kierunek)
        {
            double punktyKółka;
            double punktyKrzyżyka;
            _rozmiar = stanGry.GetLength(0);

            NastępnyAlfaBetaObcięcie(stanGry, true, _perspektywaKółka, _głębokość, double.NegativeInfinity, double.PositiveInfinity, out a, out b);

            return GraZakończona(stanGry, out punktyKółka, out punktyKrzyżyka, out wynik, out kierunek);
        }

        private double NastępnyAlfaBetaObcięcie(Pole[,] stanGry, bool maksymalizacja, bool ruchKółka, int głębokość, double alfa, double beta, out int a, out int b)
        {
            double punktyKółka;
            double punktyKrzyżyka;
            WynikGry wynik;
            KierunekZwycięskiejLinii kierunek;
            a = b = -1;

            if (GraZakończona(stanGry, out punktyKółka, out punktyKrzyżyka, out wynik, out kierunek) || (głębokość == 0))
                return WartośćWyniku(punktyKółka, punktyKrzyżyka);

            Pole znakAktualnegoGracza = ruchKółka ? Pole.Kółko : Pole.Krzyżyk;
            int ekstremalneI = -1;
            int ekstremalneJ = -1;
            double ekstremum;

            if (maksymalizacja)
            {
                ekstremum = float.NegativeInfinity;

                for (int i = 0; i < _rozmiar; i++)
                {
                    for (int j = 0; j < _rozmiar; j++)
                        if (stanGry[i, j] == Pole.Puste)
                        {
                            Pole[,] możliwyStan = KopiujStanGry(stanGry);
                            możliwyStan[i, j] = znakAktualnegoGracza;
                            double opłacalnośćRozwiązania = NastępnyAlfaBetaObcięcie(możliwyStan, false, !ruchKółka, głębokość - 1, alfa, beta, out a, out b);
                            alfa = Math.Max(alfa, ekstremum);

                            if (głębokość == _głębokość)
                                _inkrementacjaLiczbyPrzeanalizowanychRuchów();

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
                ekstremum = float.PositiveInfinity;

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

        private double WartośćWyniku(double punktyKółka, double punktyKrzyżyka)
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

                    for (int k = j; (k < początekKolejnejTrójki) && (k < rozmiar); k++)
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

                    for (int k = i; (k < początekKolejnejTrójki) && (k < rozmiar); k++)
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

            bool istniejąPustePola = stanGry.Cast<Pole>().Any(pole => pole == Pole.Puste);
            kierunek = KierunekZwycięskiejLinii.BrakWygranej;

            if (!istniejąPustePola)
            {
                wynik = WynikGry.Remis;

                return true;
            }

            wynik = WynikGry.Trwająca;

            return false;
        }

        private static void SprawdźZawartośćPola(Pole pole, ref int liczbaKółek, ref int liczbaKrzyżyków, ref int liczbaPustych)
        {
            if (pole == Pole.Kółko)
                liczbaKółek++;
            else if (pole == Pole.Krzyżyk)
                liczbaKrzyżyków++;
            else
                liczbaPustych++;
        }

        private bool OkreślZwycięzcę(int liczbaKółekWLinii, int liczbaKrzyżykówWLinii, int liczbaPustych, ref double punktyKółka, ref double punktyKrzyżyka, out WynikGry wynik)
        {
            const int potęga = 2;

            if ((liczbaKółekWLinii > 0) && (liczbaKrzyżykówWLinii == 0) && (liczbaPustych == _zwycięskaLiczbaPól - liczbaKółekWLinii))
            {
                punktyKółka += Math.Pow(Convert.ToDouble(liczbaKółekWLinii) / _zwycięskaLiczbaPól * 100, potęga);

                if (liczbaKółekWLinii == _zwycięskaLiczbaPól)
                {
                    punktyKrzyżyka = -punktyKółka;
                    wynik = WynikGry.Kółko;

                    return true;
                }
            }

            if ((liczbaKółekWLinii == 0) && (liczbaKrzyżykówWLinii > 0) && (liczbaPustych == _zwycięskaLiczbaPól - liczbaKrzyżykówWLinii))
            {
                punktyKrzyżyka += Math.Pow(Convert.ToDouble(liczbaKrzyżykówWLinii) / _zwycięskaLiczbaPól * 100, potęga);

                if (liczbaKrzyżykówWLinii == _zwycięskaLiczbaPól)
                {
                    punktyKółka = -punktyKrzyżyka;
                    wynik = WynikGry.Krzyżyk;

                    return true;
                }
            }

            wynik = WynikGry.Remis | WynikGry.Trwająca;

            return false;
        }

        private static Pole[,] KopiujStanGry(Pole[,] stanGry)
        {
            int rozmiar = stanGry.GetLength(0);
            Pole[,] kopia = new Pole[rozmiar, rozmiar];

            Array.Copy(stanGry, 0, kopia, 0, stanGry.Length);

            return kopia;
        }
    }
}