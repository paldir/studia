﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psk
{
    class Klient
    {
        static Dictionary<string, Komunikator> _komunikatory;
        static Dictionary<string, PoKlient> _klienci;
        static Komunikator _aktywnyKomunikator;

        static void Main(string[] args)
        {
            _komunikatory = new Dictionary<string, Komunikator>()  
            {
                {"pliki", new PlikiKom(Pomocnicze.Pliki.Katalog)},
                {"tcp", new TcpKom(Pomocnicze.Tcp.AdresIp,Pomocnicze.Tcp.Port)},
                {"udp", new UdpKom(Pomocnicze.Udp.Ip, Pomocnicze.Udp.Port)},
                {"net", new NetRemotingKom(String.Format("http://{0}:{1}/{2}", Pomocnicze.NetRemoting.Ip, Pomocnicze.NetRemoting.Port, Pomocnicze.NetRemoting.NazwaUsługi))}
            };

            _klienci = new Dictionary<string, PoKlient>()
            {
                {"ping", new PingPongKlient()},
                {"chat", new ChatKlient()},
                {"ftp", new FtpKlient()}
            };

            string linia = null;
            _aktywnyKomunikator = _komunikatory["tcp"];
            WypiszNazwęAktywnegoKomunikatora();
            Console.Write("> ");

            while (true)
            {
                linia = Console.ReadLine();

                if (linia == "exit")
                    break;

                if (!String.IsNullOrEmpty(linia))
                {
                    if (linia.StartsWith("kom"))
                    {
                        _aktywnyKomunikator = _komunikatory[linia.Replace("kom ", String.Empty)];

                        WypiszNazwęAktywnegoKomunikatora();
                    }
                    else
                    {
                        string[] polecenie = linia.Split(' ');
                        PoKlient _klient = _klienci[polecenie[0]];
                        _klient.Komunikator = _aktywnyKomunikator;

                        Console.WriteLine(_klient.PytanieOdpowiedź(linia));
                    }

                    Console.Write("> ");
                }

                System.Threading.Thread.Sleep(Pomocnicze.CzasSpania);
            }

            foreach (Komunikator komunikator in _komunikatory.Values)
                komunikator.Dispose();
        }

        static void WypiszNazwęAktywnegoKomunikatora()
        {
            Console.WriteLine("Aktywny komunikator to {0}.", _komunikatory.FirstOrDefault(k => k.Value == _aktywnyKomunikator).Key);
        }
    }
}