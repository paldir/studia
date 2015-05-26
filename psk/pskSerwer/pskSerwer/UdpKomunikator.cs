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
        System.Net.IPEndPoint _adresKlienta;

        public UdpKomunikator()
        {
            _serwer = new UdpClient(Pomocnicze.Udp.Ip, Pomocnicze.Udp.Port);
            _adresKlienta = new System.Net.IPEndPoint(IPAddress.Any, 0);
        }

        public void Start(DelegatKomendy obsłużKomendę)
        {
            _wątek = Thread.CurrentThread;

            while (true)
            {
                if (_serwer.Available != 0)
                {
                    StringBuilder budowniczyLinii = new StringBuilder();
                    string fragmentLinii = String.Empty;

                    while (!fragmentLinii.EndsWith(Environment.NewLine))
                    {
                        fragmentLinii = CzytajLinię();

                        budowniczyLinii.Append(fragmentLinii);
                    }

                    PiszLinię(obsłużKomendę(budowniczyLinii.ToString()));
                }
                else
                    Thread.Sleep(Pomocnicze.CzasSpania);
            }
        }

        public void Stop()
        {
            _wątek.Abort();
        }

        public override bool PiszLinię(string linia)
        {
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
            return Encoding.UTF8.GetString(_serwer.Receive(ref _adresKlienta));
        }

        public override void Dispose()
        {
            _serwer.Close();
        }
    }
}