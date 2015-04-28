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

        public void Start(DelegatKomunikatora połączenie, DelegatKomunikatora rozłączenie)
        {
            _wątek = Thread.CurrentThread;
            System.Net.Sockets.TcpListener nasłuchiwacz = new System.Net.Sockets.TcpListener(Pomocnicze.Tcp.AdresIp, Pomocnicze.Tcp.Port);

            nasłuchiwacz.Start();

            while (true)
            {
                if (nasłuchiwacz.Pending())
                {
                    TcpKomunikator komunikator = new TcpKomunikator(nasłuchiwacz.AcceptTcpClient(), rozłączenie);
                    Thread wątek = new Thread(() => połączenie(komunikator));

                    wątek.Start();
                }
                else
                    Thread.Sleep(Pomocnicze.CzasSpania);
            }
        }

        public void Stop()
        {
            _wątek.Abort();
        }
    }
}