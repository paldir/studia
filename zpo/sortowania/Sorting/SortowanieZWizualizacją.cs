using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.Diagnostics;

namespace sortowania
{
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
        DaneDoWykresu _dane;
        object _object;
        bool _sortowanieTrwa;
        ProcessThread _wątekProcesu;

        public SortowanieZWizualizacją(IMetodaSortowania<T> metodaSortująca, IList<T> kolekcja, Func<T, int> metodaKlucza, Func<IList<T>, int, int, double> metodaŚredniej, DaneDoWykresu dane, AktualizacjaWizualizacji aktualizacjaWizualizacji)
        {
            _wątekSortowania = new Thread(Sortuj);
            _kolekcja = new List<T>(kolekcja);
            _dane = dane;
            _liczbagrup = dane.ŚrednieGrupKluczy.Length;
            _liczbaElementówWKażdejGrupie = kolekcja.Count / _liczbagrup;
            _metodaKlucza = metodaKlucza;
            _metodaŚredniej = metodaŚredniej;
            _metodaSortowania = metodaSortująca;
            _aktualizacjaWizualizacji = aktualizacjaWizualizacji;
            _sortowanieTrwa = true;
            _object = new object();
        }

        void Sortuj()
        {
            Thread.BeginThreadAffinity();
            _wątekProcesu = Process.GetCurrentProcess().Threads.Cast<ProcessThread>().Single(w => w.Id == AppDomain.GetCurrentThreadId());
            _metodaSortowania.Sortuj(_kolekcja);
            Thread.EndThreadAffinity();

            lock (_object)
            {
                _sortowanieTrwa = false;
            }
        }

        public void Uruchom()
        {
            _wątekSortowania.Start();
        }

        public bool Zawieś()
        {
            lock (_object)
                if (_sortowanieTrwa)
                    _wątekSortowania.Suspend();

            return _sortowanieTrwa;
        }

        public void Wznów()
        {
            if (_wątekSortowania.ThreadState == System.Threading.ThreadState.Suspended)
                _wątekSortowania.Resume();
        }

        public void AktualizujWizualizację()
        {
            for (int i = 0; i < _liczbagrup; i++)
            {
                int indeksPoczątku = i * _liczbaElementówWKażdejGrupie;
                int indeksKońca = indeksPoczątku + _liczbaElementówWKażdejGrupie;
                int liczbaPosortowanychPar = 0;

                for (int j = indeksPoczątku; j < indeksKońca - 1; j++)
                    if (_kolekcja[j].CompareTo(_kolekcja[j + 1]) <= 0)
                        liczbaPosortowanychPar++;

                _dane.ŚrednieGrupKluczy[i] = _metodaKlucza((T)Convert.ChangeType(_metodaŚredniej(_kolekcja, indeksPoczątku, indeksKońca), typeof(T)));
                _dane.StopniePosortowania[i] = liczbaPosortowanychPar / Convert.ToSingle(_liczbaElementówWKażdejGrupie);
            }

            if (_wątekProcesu != null)
                _dane.Czas = _wątekProcesu.TotalProcessorTime;

            _aktualizacjaWizualizacji();
        }
    }
}