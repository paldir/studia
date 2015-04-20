using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;

namespace sortowania
{
    public class SortowanieZWizualizacją : ZadanieZWizualizacją
    {
        IList<Thread> _wątkiSortowań;
        object _semafor = new object();

        public AktualizacjaWizualizacji AktualizujWizualizację { get; set; }

        public SortowanieZWizualizacją(IList<ThreadStart> metodySortujące, AktualizacjaWizualizacji aktulizacjaWizualizacji)
        {
            _wątkiSortowań = new List<Thread>();
            AktualizujWizualizację = aktulizacjaWizualizacji;

            foreach (ThreadStart metodaSortująca in metodySortujące)
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