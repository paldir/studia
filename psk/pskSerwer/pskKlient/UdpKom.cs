using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;

namespace psk
{
    class UdpKom : Komunikator
    {
        UdpClient _klient;

        public UdpKom()
        {
            _klient = new UdpClient();

            _klient.Connect(Pomocnicze.Udp.Ip, Pomocnicze.Udp.Port);
        }

        public override bool PiszLinię(string linia)
        {
            if (!linia.EndsWith(Environment.NewLine))
                linia += Environment.NewLine;

            byte[] liniaBajtowo = Encoding.UTF8.GetBytes(linia);

            try
            {
                _klient.Send(liniaBajtowo, liniaBajtowo.Length);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public override string CzytajLinię()
        {
            string linia = String.Empty;
            StringBuilder budowniczyLinii = new StringBuilder();

            while (!linia.EndsWith(Environment.NewLine))
            {
                if (_klient.Available != 0)
                {
                    IPEndPoint serwer = new IPEndPoint(IPAddress.Any, 0);

                    try
                    {
                        linia = Encoding.UTF8.GetString(_klient.Receive(ref serwer));
                    }
                    catch (SocketException) { }

                    budowniczyLinii.Append(linia);
                }
                else
                    System.Threading.Thread.Sleep(Pomocnicze.CzasSpania);
            }

            return budowniczyLinii.ToString();
        }

        public override void Dispose()
        {
            _klient.Close();
        }
    }
}