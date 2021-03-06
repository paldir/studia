﻿using System.Windows;
using System.Threading;
using KółkoIKrzyżyk.Properties;

namespace KółkoIKrzyżyk.ModelWidoku
{
    public class GłównyModelWidoku : ObiektModelWidoku
    {
        private bool _ruchKółka;
        private Thread _wątekGry;
        private Algorytmy.KierunekZwycięskiejLinii _kierunek;
        private readonly object _lock;

        public Komenda WykonanieRuchu { get; private set; }
        public Komenda RozpoczęcieGry { get; private set; }
        public Komenda ZakończenieGry { get; private set; }
        public Pole OstatnioWypełnionePole { get; private set; }

        private TrybGry _tryb;
        public TrybGry Tryb
        {
            get { return _tryb; }

            set
            {
                Settings ustawienia = Settings.Default;
                _tryb = ustawienia.TrybGry = value;

                ustawienia.Save();
                OnPropertyChanged("ŻywyGracz");
            }
        }

        private bool _brakGry;
        public bool BrakGry
        {
            get { return _brakGry; }

            private set
            {
                _brakGry = value;

                OnPropertyChanged("BrakGry");
            }
        }

        private Algorytmy.WynikGry _wynik;
        public Algorytmy.WynikGry Wynik
        {
            get { return _wynik; }

            private set
            {
                _wynik = value;

                OnPropertyChanged("Wynik");
            }
        }

        private int _długośćBokuPlanszy;
        public int DługośćBokuPlanszy
        {
            get { return _długośćBokuPlanszy; }

            set
            {
                Settings ustawienia = Settings.Default;
                _długośćBokuPlanszy = ustawienia.DługośćBokuPlanszy = value;
                Plansza = new Plansza(value);

                if (ZwycięskaLiczbaPól > DługośćBokuPlanszy)
                    ZwycięskaLiczbaPól = DługośćBokuPlanszy;

                ustawienia.Save();
            }
        }

        private Plansza _plansza;
        public Plansza Plansza
        {
            get { return _plansza; }

            private set
            {
                _plansza = value;

                OnPropertyChanged("Plansza");
            }
        }

        private bool _ruchGracza;
        public bool RuchGracza
        {
            get { return _ruchGracza; }

            set
            {
                _ruchGracza = value;

                OnPropertyChanged("RuchGracza");
            }
        }

        private int _zwycięskaLiczbaPól;
        public int ZwycięskaLiczbaPól
        {
            get { return _zwycięskaLiczbaPól; }

            set
            {
                if (value > DługośćBokuPlanszy)
                    value = DługośćBokuPlanszy;

                Settings ustawienia = Settings.Default;
                _zwycięskaLiczbaPól = ustawienia.ZwycięskaLiczbaPól = value;

                OnPropertyChanged("ZwycięskaLiczbaPól");
                ustawienia.Save();
            }
        }

        private int _głębokośćRekurencji;
        public int GłębokośćRekurencji
        {
            get { return _głębokośćRekurencji; }

            set
            {
                Settings ustawienia = Settings.Default;
                _głębokośćRekurencji = ustawienia.GłębokośćRekurencji = value;

                ustawienia.Save();
            }
        }

        private Zaczynający _ktoZaczyna;
        public Zaczynający KtoZaczyna
        {
            get { return _ktoZaczyna; }

            set
            {
                Settings ustawienia = Settings.Default;
                _ktoZaczyna = ustawienia.Zaczynający = value;

                ustawienia.Save();
            }
        }

        private int _liczbaMożliwychRuchów;
        public int LiczbaMożliwychRuchów
        {
            get { return _liczbaMożliwychRuchów; }

            set
            {
                _liczbaMożliwychRuchów = value;

                OnPropertyChanged("LiczbaMożliwychRuchów");
            }
        }

        private int _liczbaPrzeanalizowanychRuchów;
        public int LiczbaPrzeanalizowanychRuchów
        {
            get { return _liczbaPrzeanalizowanychRuchów; }

            set
            {
                _liczbaPrzeanalizowanychRuchów = value;

                OnPropertyChanged("LiczbaPrzeanalizowanychRuchów");
            }
        }

        public bool ŻywyGracz
        {
            get { return Tryb == TrybGry.GraczVsSi; }
        }

        public GłównyModelWidoku()
        {
            _lock = new object();
            Settings ustawienia = Settings.Default;
            RozpoczęcieGry = new Komenda(RozpocznijGrę);
            WykonanieRuchu = new Komenda(WykonajRuchJakoGracz);
            ZakończenieGry = new Komenda(() => Current_Exit(null, null));
            DługośćBokuPlanszy = ustawienia.DługośćBokuPlanszy;
            ZwycięskaLiczbaPól = ustawienia.ZwycięskaLiczbaPól;
            GłębokośćRekurencji = ustawienia.GłębokośćRekurencji;
            Tryb = ustawienia.TrybGry;
            KtoZaczyna = ustawienia.Zaczynający;
            BrakGry = true;
            LiczbaMożliwychRuchów = 1;

            Application.Current.Exit += Current_Exit;
        }

        private void RozpocznijGrę()
        {
            BrakGry = false;
            _ruchKółka = true;
            Wynik = Algorytmy.WynikGry.Trwająca;
            _wątekGry = new Thread(Gra);

            Plansza.Resetuj();

            if ((Tryb == TrybGry.GraczVsSi) && (KtoZaczyna == Zaczynający.Gracz))
                RuchGracza = true;
            else
                RuchGracza = false;

            _wątekGry.Start();
        }

        private void Gra()
        {
            do
            {
                if (RuchGracza)
                {
                    lock (_lock)
                        Monitor.Wait(_lock);

                    RuchGracza = false;
                    _ruchKółka = !_ruchKółka;
                }

                if (Wynik == Algorytmy.WynikGry.Trwająca)
                {
                    LiczbaPrzeanalizowanychRuchów = 0;

                    Thread.Sleep(1000);
                    WykonajRuchJakoKomputer();

                    _ruchKółka = !_ruchKółka;

                    if (ŻywyGracz)
                        RuchGracza = true;
                }
            }
            while (Wynik == Algorytmy.WynikGry.Trwająca);

            RuchGracza = false;
        }

        private void WykonajRuchJakoGracz(object parametr)
        {
            if (RuchGracza)
            {
                Pole pole = parametr as Pole;

                if ((pole != null) && (pole.Zawartość == Algorytmy.Pole.Puste))
                {
                    Algorytmy.Pole znak = _ruchKółka ? Algorytmy.Pole.Kółko : Algorytmy.Pole.Krzyżyk;

                    OstatnioWypełnionePole = pole;
                    pole.Zawartość = znak;
                    Algorytmy.Ruch algorytm = new Algorytmy.Ruch(_ruchKółka, GłębokośćRekurencji, ZwycięskaLiczbaPól, InkrementujLiczbęPrzeanalizowanychRuchów);
                    Algorytmy.Pole[,] stanGry = KonwertujNaTypZAlgorytmu();
                    double punktyKółka;
                    double punktyKrzyżyka;
                    Algorytmy.WynikGry wynik;

                    if (algorytm.GraZakończona(stanGry, out punktyKółka, out punktyKrzyżyka, out wynik, out _kierunek))
                    {
                        BrakGry = true;
                        Wynik = wynik;

                        PrzedstawWynikGry(stanGry, pole.I, pole.J);
                        ZapiszNowyStanGry(stanGry);
                    }

                    lock (_lock)
                        Monitor.Pulse(_lock);
                }
            }
        }

        private void WykonajRuchJakoKomputer()
        {
            Algorytmy.Ruch algorytm = new Algorytmy.Ruch(_ruchKółka, GłębokośćRekurencji, ZwycięskaLiczbaPól, InkrementujLiczbęPrzeanalizowanychRuchów);
            Algorytmy.Pole[,] gra = KonwertujNaTypZAlgorytmu();
            Algorytmy.WynikGry wynik;
            int a;
            int b;
            LiczbaMożliwychRuchów = 0;

            foreach (Algorytmy.Pole pole in gra)
                if (pole == Algorytmy.Pole.Puste)
                    LiczbaMożliwychRuchów++;

            if (algorytm.AlfaBetaObcięcie(gra, out wynik, out a, out b, out _kierunek))
                BrakGry = true;

            Wynik = wynik;
            Application aplikacja = Application.Current;

            if (wynik == Algorytmy.WynikGry.Trwająca)
                OstatnioWypełnionePole = Plansza[a][b];
            else
                PrzedstawWynikGry(gra, a, b);

            if (aplikacja != null)
                aplikacja.Dispatcher.Invoke(() => ZapiszNowyStanGry(gra));
        }

        private void ZapiszNowyStanGry(Algorytmy.Pole[,] gra)
        {
            for (int i = 0; i < DługośćBokuPlanszy; i++)
                for (int j = 0; j < DługośćBokuPlanszy; j++)
                    Plansza[i][j].Zawartość = gra[i, j];
        }

        private Algorytmy.Pole[,] KonwertujNaTypZAlgorytmu()
        {
            Algorytmy.Pole[,] gra = new Algorytmy.Pole[DługośćBokuPlanszy, DługośćBokuPlanszy];

            for (int i = 0; i < DługośćBokuPlanszy; i++)
                for (int j = 0; j < DługośćBokuPlanszy; j++)
                    gra[i, j] = Plansza[i][j].Zawartość;

            return gra;
        }

        private void PrzedstawWynikGry(Algorytmy.Pole[,] gra, int a, int b)
        {
            OstatnioWypełnionePole = null;

            if (_kierunek != Algorytmy.KierunekZwycięskiejLinii.BrakWygranej)
            {
                Algorytmy.Pole poszukiwane;
                Algorytmy.Pole zamiennik;

                if (Wynik == Algorytmy.WynikGry.Kółko)
                {
                    poszukiwane = Algorytmy.Pole.Kółko;
                    zamiennik = Algorytmy.Pole.ZwycięskieKółko;
                }
                else
                {
                    poszukiwane = Algorytmy.Pole.Krzyżyk;
                    zamiennik = Algorytmy.Pole.ZwycięskiKrzyżyk;
                }

                switch (_kierunek)
                {
                    case Algorytmy.KierunekZwycięskiejLinii.Poziomy:
                        for (int j = b; j >= 0; j--)
                            if (gra[a, j] == poszukiwane)
                                gra[a, j] = zamiennik;
                            else
                                break;

                        for (int j = b + 1; j < DługośćBokuPlanszy; j++)
                            if (gra[a, j] == poszukiwane)
                                gra[a, j] = zamiennik;
                            else
                                break;

                        break;

                    case Algorytmy.KierunekZwycięskiejLinii.Pionowy:
                        for (int i = a; i >= 0; i--)
                            if (gra[i, b] == poszukiwane)
                                gra[i, b] = zamiennik;
                            else
                                break;

                        for (int i = a + 1; i < DługośćBokuPlanszy; i++)
                            if (gra[i, b] == poszukiwane)
                                gra[i, b] = zamiennik;
                            else
                                break;

                        break;

                    case Algorytmy.KierunekZwycięskiejLinii.UkośnieOdLewejDoPrawej:
                        for (int k = 0; k < DługośćBokuPlanszy; k++)
                        {
                            int i = a - k;
                            int j = b - k;

                            if ((i >= 0) && (j >= 0))
                            {
                                if (gra[i, j] == poszukiwane)
                                    gra[i, j] = zamiennik;
                                else
                                    break;
                            }
                            else
                                break;
                        }

                        for (int k = 1; k < DługośćBokuPlanszy; k++)
                        {
                            int i = a + k;
                            int j = b + k;

                            if ((i < DługośćBokuPlanszy) && (j < DługośćBokuPlanszy))
                            {
                                if (gra[i, j] == poszukiwane)
                                    gra[i, j] = zamiennik;
                                else
                                    break;
                            }
                            else
                                break;
                        }

                        break;

                    case Algorytmy.KierunekZwycięskiejLinii.UkośnieOdPrawejDoLewej:
                        for (int k = 0; k < DługośćBokuPlanszy; k++)
                        {
                            int i = a - k;
                            int j = b + k;

                            if ((i >= 0) && (j < DługośćBokuPlanszy))
                            {
                                if (gra[i, j] == poszukiwane)
                                    gra[i, j] = zamiennik;
                                else
                                    break;
                            }
                            else
                                break;
                        }

                        for (int k = 1; k < DługośćBokuPlanszy; k++)
                        {
                            int i = a + k;
                            int j = b - k;

                            if ((i < DługośćBokuPlanszy) && (j >= 0))
                            {
                                if (gra[i, j] == poszukiwane)
                                    gra[i, j] = zamiennik;
                                else
                                    break;
                            }
                            else
                                break;
                        }

                        break;
                }
            }
        }

        private void InkrementujLiczbęPrzeanalizowanychRuchów()
        {
            LiczbaPrzeanalizowanychRuchów++;
        }

        private void Current_Exit(object sender, ExitEventArgs e)
        {
            if (_wątekGry != null)
                _wątekGry.Abort();

            BrakGry = true;
            Wynik = Algorytmy.WynikGry.Remis;
            OstatnioWypełnionePole = null;
        }
    }
}