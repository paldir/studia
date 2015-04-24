using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;

namespace psk
{
    class TcpNasłuchiwacz : INasłuchiwacz
    {
        Thread _wątek;
        List<TcpKomunikator> _komunikatory;
        DelegatKomunikatora _rozłączenie;

        public void Start(DelegatKomunikatora połączenie, DelegatKomunikatora rozłączenie)
        {
            _wątek = Thread.CurrentThread;
            _komunikatory = new List<TcpKomunikator>();
            _rozłączenie = rozłączenie;
            System.Net.Sockets.TcpListener nasłuchiwacz = new System.Net.Sockets.TcpListener(Pomocnicze.Tcp.AdresIp, Pomocnicze.Tcp.Port);

            nasłuchiwacz.Start();

            while (true)
            {
                if (nasłuchiwacz.Pending())
                {
                    TcpKomunikator komunikator = new TcpKomunikator(nasłuchiwacz.AcceptTcpClient());
                    Thread wątek = new Thread(() => połączenie(komunikator));

                    wątek.Start();
                    _komunikatory.Add(komunikator);
                }
                else
                {
                    Thread.Sleep(Pomocnicze.CzasSpania);

                    foreach (TcpKomunikator komunikator in _komunikatory)
                        if (!komunikator.Połączony)
                            rozłączenie(komunikator);

                    _komunikatory.RemoveAll(c => !c.Połączony);
                }
            }
        }

        public void Stop()
        {
            foreach (TcpKomunikator komunikator in _komunikatory)
                _rozłączenie(komunikator);

            _wątek.Abort();
        }
    }
}