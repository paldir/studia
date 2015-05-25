using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;

namespace sortowania
{
    public delegate void PrześlijDaneDoWizualizacji(List<int> dane);

    public class SortowanieZWizualizacją<T> : ZadanieZWizualizacją where T : IComparable, IComparable<T>
    {
        Thread _wątekSortowania;
        List<T> _kolekcja;
        int _liczbagrup;
        int _liczbaElementówWKażdejGrupie;
        Func<T, int> _metodaKlucza;
        Func<IList<T>, double> _metodaŚredniej;
        IMetodaSortowania<T> _metodaSortowania;
        AktualizacjaWizualizacji _aktualizacjaWizualizacji;
        PrześlijDaneDoWizualizacji _przesłanieDanychDoWizualizacji;
        object _object;

        public SortowanieZWizualizacją(IMetodaSortowania<T> metodaSortująca, IList<T> kolekcja, Func<T, int> metodaKlucza, Func<IList<T>, double> metodaŚredniej, int liczbaGrupWizualizacji, AktualizacjaWizualizacji aktualizacjaWizualizacji, PrześlijDaneDoWizualizacji przesłanieDanychDoWizualizacji)
        {
            _wątekSortowania = new Thread(Sortuj);
            _kolekcja = new List<T>(kolekcja);
            _liczbagrup = liczbaGrupWizualizacji;
            _liczbaElementówWKażdejGrupie = kolekcja.Count / _liczbagrup;
            _metodaKlucza = metodaKlucza;
            _metodaŚredniej = metodaŚredniej;
            _metodaSortowania = metodaSortująca;
            _aktualizacjaWizualizacji = aktualizacjaWizualizacji;
            _przesłanieDanychDoWizualizacji = przesłanieDanychDoWizualizacji;
            _object = new object();
        }

        void Sortuj()
        {
            _metodaSortowania.Sortuj(_kolekcja);

            lock (_object) { };
        }

        public void Uruchom()
        {
            _wątekSortowania.Start();
        }

        public void Zawieś()
        {
            lock (_object)
                if (_wątekSortowania.ThreadState == ThreadState.Running)
                    _wątekSortowania.Suspend();
        }

        public bool Wznów()
        {
            bool praca = false;

            lock (_object)
                if (_wątekSortowania.ThreadState == ThreadState.Suspended)
                {
                    _wątekSortowania.Resume();

                    praca = true;
                }

            return praca;
        }

        public void AktualizujWizualizację()
        {
            List<int> listaKluczy = new List<int>();

            for (int i = 0; i < _liczbagrup; i++)
                listaKluczy.Add(_metodaKlucza((T)Convert.ChangeType(_metodaŚredniej(_kolekcja.GetRange(i * _liczbaElementówWKażdejGrupie, _liczbaElementówWKażdejGrupie)), typeof(T))));

            _przesłanieDanychDoWizualizacji(listaKluczy);
            _aktualizacjaWizualizacji();
        }
    }
}