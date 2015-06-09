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
        FileSystemWatcher _obserwator;
        DelegatKomunikatora _połączenie;
        DelegatKomunikatora _rozłączenie;

        public PlikiNasłuchiwacz()
        {
            if (!Directory.Exists(Pomocnicze.Pliki.Katalog))
                Directory.CreateDirectory(Pomocnicze.Pliki.Katalog);
        }

        public void Start(DelegatKomunikatora połączenie, DelegatKomunikatora rozłączenie)
        {
            _obserwator = new FileSystemWatcher(Pomocnicze.Pliki.Katalog, "*.in");
            _obserwator.Created += _obserwator_Created;
            _obserwator.EnableRaisingEvents = true;
            _połączenie = połączenie;
            _rozłączenie = rozłączenie;
        }

        void _obserwator_Created(object sender, FileSystemEventArgs e)
        {
            PlikiKomunikator plikiKomunikator = new PlikiKomunikator(e.FullPath);

            _połączenie(plikiKomunikator);
            _rozłączenie(plikiKomunikator);
        }

        public void Stop()
        {
            _obserwator.Dispose();
        }
    }
}