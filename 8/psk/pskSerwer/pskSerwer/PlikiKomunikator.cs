using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace psk
{
    class PlikiKomunikator : Komunikator, IKomunikator
    {
        string _ścieżka;

        public PlikiKomunikator(string ścieżka)
        {
            _ścieżka = ścieżka;
        }

        public override bool PiszLinię(string linia)
        {
            try
            {
                using (StreamWriter strumień = new StreamWriter(Path.ChangeExtension(_ścieżka, "out")))
                    strumień.WriteLine(linia);
            }
            catch
            { return false; }

            return true;
        }

        public override string CzytajLinię()
        {
            while (true)
            {
                try
                {
                    using (StreamReader strumień = new StreamReader(_ścieżka))
                        return strumień.ReadToEnd();
                }
                catch (IOException) { }
            }
        }

        public override void Dispose()
        {
            Stop();
        }

        public void Start(DelegatKomendy onCommand)
        {
            PiszLinię(onCommand(CzytajLinię()));
        }

        public void Stop()
        {
            File.Delete(_ścieżka);
        }
    }
}