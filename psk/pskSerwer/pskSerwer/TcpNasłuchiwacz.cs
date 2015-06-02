using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.Net.Sockets;

namespace psk
{
    class TcpNasłuchiwacz : INasłuchiwacz
    {
        Thread _wątek;

        public void Start(DelegatKomunikatora połączenie, DelegatKomunikatora rozłączenie)
        {
            _wątek = Thread.CurrentThread;
            TcpListener nasłuchiwacz = new TcpListener(System.Net.Dns.GetHostEntry(Pomocnicze.Tcp.AdresIp).AddressList[0], Pomocnicze.Tcp.Port);

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