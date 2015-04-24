using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Threading;

namespace psk
{
    class PlikiKom : Komunikator
    {
        static int _indeks = 0;
        string _plikWynikowy;

        public override bool PiszLinię(string linia)
        {
            try
            {
                string plik = Path.Combine(Pomocnicze.Pliki.Katalog, String.Format("command{0}.in", _indeks));

                Interlocked.Increment(ref _indeks);

                using (StreamWriter strumień = new StreamWriter(plik))
                    strumień.WriteLine(linia);

                _plikWynikowy = Path.ChangeExtension(plik, "out");

                return true;
            }
            catch { return false; }
        }

        public override string CzytajLinię()
        {
            string linia = null;

            while (linia == null)
            {
                if (File.Exists(_plikWynikowy))
                    using (StreamReader strumień = new StreamReader(_plikWynikowy))
                        linia = strumień.ReadToEnd();
                else
                    Thread.Sleep(Pomocnicze.CzasSpania);
            }

            File.Delete(_plikWynikowy);

            return linia;
        }

        public override void Dispose()
        {
            
        }
    }
}