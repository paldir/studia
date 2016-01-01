using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using System.Windows.Threading;
using KółkoIKrzyżyk.Properties;

namespace KółkoIKrzyżyk.ModelWidoku
{
    public class GłównyModelWidoku : ObiektModelWidoku
    {
        bool _ruchKółka;
        DispatcherTimer _minutnik;
        Algorytmy.KierunekZwycięskiejLinii _kierunek;

        public Komenda WykonanieRuchu { get; private set; }
        public Komenda RozpoczęcieGry { get; private set; }
        public Pole OstatnioWypełnionePole { get; private set; }

        TrybGry _tryb;
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

        bool _brakGry;
        public bool BrakGry
        {
            get { return _brakGry; }

            private set
            {
                _brakGry = value;

                OnPropertyChanged("BrakGry");
            }
        }

        Algorytmy.WynikGry _wynik;
        public Algorytmy.WynikGry Wynik
        {
            get { return _wynik; }

            private set
            {
                _wynik = value;

                OnPropertyChanged("Wynik");
            }
        }

        int _długośćBokuPlanszy;
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

        Plansza _plansza;
        public Plansza Plansza
        {
            get { return _plansza; }

            private set
            {
                _plansza = value;

                OnPropertyChanged("Plansza");
            }
        }

        bool _ruchGracza;
        public bool RuchGracza
        {
            get { return _ruchGracza; }

            set
            {
                _ruchGracza = value;

                OnPropertyChanged("RuchGracza");
            }
        }

        int _zwycięskaLiczbaPól;
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

        int _głębokośćRekurencji;
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

        Zaczynający _ktoZaczyna;
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
        

        public bool ŻywyGracz
        {
            get { return Tryb == TrybGry.GraczVsSi; }
        }

        public GłównyModelWidoku()
        {
            _minutnik = new DispatcherTimer();
            _minutnik.Interval = TimeSpan.FromSeconds(1);
            _minutnik.Tick += minutnik_Tick;

            Settings ustawienia = Settings.Default;
            RozpoczęcieGry = new Komenda(RozpocznijGrę);
            WykonanieRuchu = new Komenda(WykonajRuch);
            DługośćBokuPlanszy = ustawienia.DługośćBokuPlanszy;
            ZwycięskaLiczbaPól = ustawienia.ZwycięskaLiczbaPól;
            GłębokośćRekurencji = ustawienia.GłębokośćRekurencji;
            Tryb = ustawienia.TrybGry;
            KtoZaczyna = ustawienia.Zaczynający;
            BrakGry = true;
        }

        void RozpocznijGrę()
        {
            BrakGry = false;
            _ruchKółka = true;
            Wynik = Algorytmy.WynikGry.Trwająca;

            Plansza.Resetuj();

            if (Tryb == TrybGry.GraczVsSi && KtoZaczyna == Zaczynający.Gracz)
                RuchGracza = true;
            else
            {
                RuchGracza = false;

                _minutnik.Start();
            }
        }

        void WykonajRuch(object parametr)
        {
            if (RuchGracza)
            {
                ModelWidoku.Pole pole = parametr as ModelWidoku.Pole;

                if (pole.Zawartość == Algorytmy.Pole.Puste)
                {
                    Algorytmy.Pole znak;

                    if (_ruchKółka)
                        znak = Algorytmy.Pole.Kółko;
                    else
                        znak = Algorytmy.Pole.Krzyżyk;

                    OstatnioWypełnionePole = pole;
                    pole.Zawartość = znak;
                    _ruchKółka = !_ruchKółka;
                    RuchGracza = false;

                    _minutnik.Start();
                }
            }
        }

        void minutnik_Tick(object sender, EventArgs e)
        {
            Algorytmy.Ruch algorytm = new Algorytmy.Ruch(_ruchKółka, GłębokośćRekurencji, ZwycięskaLiczbaPól);
            Algorytmy.Pole[,] gra = new Algorytmy.Pole[DługośćBokuPlanszy, DługośćBokuPlanszy];
            Algorytmy.WynikGry wynik;
            Algorytmy.KierunekZwycięskiejLinii kierunek;
            int a;
            int b;

            for (int i = 0; i < DługośćBokuPlanszy; i++)
                for (int j = 0; j < DługośćBokuPlanszy; j++)
                    gra[i, j] = Plansza[i][j].Zawartość;

            if (algorytm.AlfaBetaObcięcie(gra, out wynik, out a, out b, out kierunek))
            {
                BrakGry = true;

                _minutnik.Stop();
            }

            Wynik = wynik;
            _kierunek = kierunek;

            if (wynik == Algorytmy.WynikGry.Trwająca)
                OstatnioWypełnionePole = Plansza[a][b];
            else
            {
                OstatnioWypełnionePole = null;

                if (kierunek != Algorytmy.KierunekZwycięskiejLinii.BrakWygranej)
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

                    switch (kierunek)
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

                                if (i >= 0 && j >= 0)
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

                                if (i < DługośćBokuPlanszy && j < DługośćBokuPlanszy)
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

                                if (i >= 0 && j < DługośćBokuPlanszy)
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

                                if (i < DługośćBokuPlanszy && j >= 0)
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

            for (int i = 0; i < DługośćBokuPlanszy; i++)
                for (int j = 0; j < DługośćBokuPlanszy; j++)
                    Plansza[i][j].Zawartość = gra[i, j];

            _ruchKółka = !_ruchKółka;

            if (_tryb == TrybGry.GraczVsSi && Wynik == Algorytmy.WynikGry.Trwająca)
            {
                _minutnik.Stop();

                RuchGracza = true;
            }
        }
    }
}