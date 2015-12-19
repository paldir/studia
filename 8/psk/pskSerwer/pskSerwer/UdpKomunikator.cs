using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace psk
{
    class UdpKomunikator : Komunikator, IKomunikator
    {
        UdpClient _serwer;
        Thread _wątek;
        IPEndPoint _adresKlienta;

        public UdpKomunikator(int port)
        {
            _serwer = new UdpClient(port);
            _adresKlienta = new IPEndPoint(IPAddress.Any, 0);
        }

        public void Start(DelegatKomendy obsłużKomendę)
        {
            _wątek = Thread.CurrentThread;

            while (true)
            {
                if (_serwer.Available == 0)
                    Thread.Sleep(Pomocnicze.CzasSpania);
                else
                    if (!PiszLinię(obsłużKomendę(CzytajLinię())))
                        PiszLinię(Pomocnicze.KomunikatyBłędów.Odpowiedź);
            }
        }

        public void Stop()
        {
            _wątek.Abort();
        }

        public override bool PiszLinię(string linia)
        {
            if (!linia.EndsWith(Environment.NewLine))
                linia += Environment.NewLine;

            byte[] bajty = Encoding.UTF8.GetBytes(linia);

            try
            {
                _serwer.Send(bajty, bajty.Length, _adresKlienta);

                return true;
            }
            catch { return false; }
        }

        public override string CzytajLinię()
        {
            StringBuilder budowniczyLinii = new StringBuilder();
            string fragmentLinii = String.Empty;

            while (!fragmentLinii.EndsWith(Environment.NewLine))
            {
                fragmentLinii = Encoding.UTF8.GetString(_serwer.Receive(ref _adresKlienta));

                budowniczyLinii.Append(fragmentLinii);
            }

            return budowniczyLinii.ToString();
        }

        public override void Dispose()
        {
            _serwer.Close();
        }
    }
}