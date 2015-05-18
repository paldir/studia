using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;

namespace sortowania
{
    public class SortowanieZWizualizacją<T> : ZadanieZWizualizacją where T : IComparable, IComparable<T>
    {
        IList<Thread> _wątkiSortowań;

        public AktualizacjaWizualizacji AktualizujWizualizację { get; set; }

        public SortowanieZWizualizacją(IList<IMetodaSortowania<T>> metodySortujące, Func<IList<T>, int> metodaŚredniej, IList<T> kolekcja, AktualizacjaWizualizacji aktualizacjaWizualizacji)
        {
            _wątkiSortowań = new List<Thread>();
            AktualizujWizualizację = aktualizacjaWizualizacji;

            foreach (IMetodaSortowania<T> metodaSortująca in metodySortujące)
            {
                Thread wątek = new Thread(metodaSortująca);

                _wątkiSortowań.Add(wątek);
            }
        }

        public void Uruchom()
        {
            foreach (Thread wątekSortowania in _wątkiSortowań)
                wątekSortowania.Start();
        }

        public void Zawieś()
        {
            foreach (Thread wątekSortowania in _wątkiSortowań)
                if (wątekSortowania.ThreadState == ThreadState.Running)
                    try
                    {
                        wątekSortowania.Suspend();
                    }
                    catch (ThreadStateException) { }
        }

        public bool Wznów()
        {
            bool byłoCoWznawiać = false;

            foreach (Thread wątekSortowania in _wątkiSortowań)
                if (wątekSortowania.ThreadState == ThreadState.Suspended)
                    try
                    {
                        wątekSortowania.Resume();

                        byłoCoWznawiać = true;
                    }
                    catch (ThreadStateException) { }

            return byłoCoWznawiać;
        }
    }
}