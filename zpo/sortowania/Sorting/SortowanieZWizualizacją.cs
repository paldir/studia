using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;

namespace sortowania
{
    public delegate void PrzesłanieDanychDoWizualizacji(IList<int> dane, IList<float> stopniePosortowania);

    public class SortowanieZWizualizacją<T> : ZadanieZWizualizacją where T : IComparable, IComparable<T>
    {
        Thread _wątekSortowania;
        List<T> _kolekcja;
        int _liczbagrup;
        int _liczbaElementówWKażdejGrupie;
        Func<T, int> _metodaKlucza;
        Func<IList<T>, int, int, double> _metodaŚredniej;
        IMetodaSortowania<T> _metodaSortowania;
        AktualizacjaWizualizacji _aktualizacjaWizualizacji;
        PrzesłanieDanychDoWizualizacji _przesłanieDanychDoWizualizacji;
        object _object;

        public SortowanieZWizualizacją(IMetodaSortowania<T> metodaSortująca, IList<T> kolekcja, Func<T, int> metodaKlucza, Func<IList<T>, int, int, double> metodaŚredniej, int liczbaGrupWizualizacji, AktualizacjaWizualizacji aktualizacjaWizualizacji, PrzesłanieDanychDoWizualizacji przesłanieDanychDoWizualizacji)
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
            List<float> stopniePosortowania = new List<float>();

            for (int i = 0; i < _liczbagrup; i++)
            {
                int indeksPoczątku = i * _liczbaElementówWKażdejGrupie;
                int indeksKońca = indeksPoczątku + _liczbaElementówWKażdejGrupie;
                int liczbaPosortowanychPar = 0;

                for (int j = indeksPoczątku; j < indeksKońca - 1; j++)
                    if (_kolekcja[j].CompareTo(_kolekcja[j + 1]) <= 0)
                        liczbaPosortowanychPar++;

                listaKluczy.Add(_metodaKlucza((T)Convert.ChangeType(_metodaŚredniej(_kolekcja, indeksPoczątku, indeksKońca), typeof(T))));
                stopniePosortowania.Add(liczbaPosortowanychPar / Convert.ToSingle(_liczbaElementówWKażdejGrupie));
            }

            _przesłanieDanychDoWizualizacji(listaKluczy, stopniePosortowania);
            _aktualizacjaWizualizacji();
        }
    }
}