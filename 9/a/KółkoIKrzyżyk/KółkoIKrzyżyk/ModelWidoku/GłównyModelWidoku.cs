using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace KółkoIKrzyżyk.ModelWidoku
{
    public class GłównyModelWidoku : ObiektModelWidoku
    {
        bool _ruchKółka;
        bool _ruchGracza;
        DispatcherTimer _minutnik;

        public int ZwycięskaLiczbaPól { get; set; }
        public Komenda WykonanieRuchu { get; private set; }
        public Komenda RozpoczęcieGry { get; private set; }
        public int GłębokośćRekurencji { get; set; }
        public Zaczynający KtoZaczyna { get; set; }
        public Pole OstatnioWypełnionePole { get; set; }

        TrybGry _tryb;
        public TrybGry Tryb
        {
            get { return _tryb; }

            set
            {
                _tryb = value;

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
                _długośćBokuPlanszy = value;
                Plansza = new Plansza(value);

                OnPropertyChanged("DługośćBokuPlanszy");
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


        public bool ŻywyGracz
        {
            get { return Tryb == KółkoIKrzyżyk.TrybGry.GraczVsSi; }
        }

        public GłównyModelWidoku()
        {
            _minutnik = new DispatcherTimer();
            _minutnik.Interval = TimeSpan.FromSeconds(1);
            _minutnik.Tick += minutnik_Tick;

            RozpoczęcieGry = new Komenda(RozpocznijGrę);
            WykonanieRuchu = new Komenda(WykonajRuch);
            DługośćBokuPlanszy = 10;
            ZwycięskaLiczbaPól = 5;
            GłębokośćRekurencji = 3;
            BrakGry = true;
        }

        void RozpocznijGrę()
        {
            BrakGry = false;
            _ruchKółka = true;

            Plansza.Resetuj();

            if (Tryb == TrybGry.GraczVsSi && KtoZaczyna == Zaczynający.Gracz)
                _ruchGracza = true;
            else
            {
                _ruchGracza = false;

                _minutnik.Start();
            }
        }

        void WykonajRuch(object parametr)
        {
            if (_ruchGracza)
            {
                ModelWidoku.Pole pole = parametr as ModelWidoku.Pole;
                Algorytmy.Pole znak;

                if (_ruchKółka)
                    znak = Algorytmy.Pole.Kółko;
                else
                    znak = Algorytmy.Pole.Krzyżyk;

                pole.Zawartość = znak;
                _ruchKółka = !_ruchKółka;
                _ruchGracza = false;

                _minutnik.Start();
            }
        }

        void minutnik_Tick(object sender, EventArgs e)
        {
            Algorytmy.Ruch algorytm = new Algorytmy.Ruch(_ruchKółka, GłębokośćRekurencji, ZwycięskaLiczbaPól);
            Algorytmy.Pole[,] gra = new Algorytmy.Pole[DługośćBokuPlanszy, DługośćBokuPlanszy];
            Algorytmy.WynikGry wynik;

            for (int i = 0; i < DługośćBokuPlanszy; i++)
                for (int j = 0; j < DługośćBokuPlanszy; j++)
                    gra[i, j] = Plansza[i][j].Zawartość;

            if (algorytm.AlfaBetaObcięcie(gra, out wynik))
            {
                BrakGry = true;

                _minutnik.Stop();
            }

            Wynik = wynik;

            for (int i = 0; i < DługośćBokuPlanszy; i++)
                for (int j = 0; j < DługośćBokuPlanszy; j++)
                    Plansza[i][j].Zawartość = gra[i, j];

            _ruchKółka = !_ruchKółka;

            if (_tryb == KółkoIKrzyżyk.TrybGry.GraczVsSi)
            {
                _minutnik.Stop();

                _ruchGracza = true;
            }
        }
    }
}