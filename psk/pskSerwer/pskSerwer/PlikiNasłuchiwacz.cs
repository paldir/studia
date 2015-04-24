using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace psk
{
    class PlikiNasłuchiwacz : INasłuchiwacz
    {
        bool _stop = false;

        public void Start(DelegatKomunikatora połączenie, DelegatKomunikatora rozłączenie)
        {
            while (!_stop)
            {
                string[] pliki = Directory.GetFiles(Pomocnicze.Pliki.Katalog, "*.in");

                if (pliki.Any())
                    foreach (string plik in pliki)
                    {
                        PlikiKomunikator plikiKomunikator = new PlikiKomunikator(plik);

                        połączenie(plikiKomunikator);
                        rozłączenie(plikiKomunikator);
                    }
                else
                    System.Threading.Thread.Sleep(Pomocnicze.CzasSpania);
            }
        }

        public void Stop()
        {
            _stop = true;
        }
    }
}