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

        public Plansza Plansza { get; private set; }
        public int DługośćBokuPlanszy { get; private set; }
        public Komenda WykonanieRuchu { get; private set; }
        public Komenda RozpoczęcieGry { get; private set; }

        TrybGry _tryb;
        public TrybGry Tryb
        {
            get { return _tryb; }

            set
            {
                _tryb = value;

                OnPropertyChanged("TrybGry");
                OnPropertyChanged("ŻywyGracz");
            }
        }

        Zaczynający _ktoZaczyna;
        public Zaczynający KtoZaczyna
        {
            get { return _ktoZaczyna; }

            set
            {
                _ktoZaczyna = value;

                OnPropertyChanged("Zaczynający");
            }
        }

        bool _brakGry;
        public bool BrakGry
        {
            get { return _brakGry; }

            set
            {
                _brakGry = value;

                OnPropertyChanged("BrakGry");
            }
        }

        Algorytmy.WynikGry _wynik;
        public Algorytmy.WynikGry Wynik
        {
            get { return _wynik; }
            
            set 
            {
                _wynik = value;

                OnPropertyChanged("Wynik");
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
            DługośćBokuPlanszy = 3;
            Plansza = new ModelWidoku.Plansza(DługośćBokuPlanszy);
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
            Algorytmy.Ruch algorytm = new Algorytmy.Ruch(_ruchKółka, Int32.MaxValue);
            Algorytmy.Pole[,] gra = new Algorytmy.Pole[DługośćBokuPlanszy, DługośćBokuPlanszy];
            Algorytmy.WynikGry wynik;

            for (int i = 0; i < DługośćBokuPlanszy; i++)
                for (int j = 0; j < DługośćBokuPlanszy; j++)
                    gra[i, j] = Plansza[i][j].Zawartość;

            if (algorytm.Minimax(gra, out wynik))
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