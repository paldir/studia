﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace psk
{
    class Serwer
    {
        static Dictionary<string, IUsługa> _usługi;
        static List<IKomunikator> _komunikatory = new List<IKomunikator>();
        static List<INasłuchiwacz> _nasłuchiwacze;
        static object _locker = new object();

        static void Main(string[] args)
        {
            _usługi = new Dictionary<string, IUsługa>()
        {
            {"ping", new PingPong()},
            {"chat", new Chat()},
            {"ftp", new Ftp()}
        };

            _nasłuchiwacze = new List<INasłuchiwacz>()
        {
            new PlikiNasłuchiwacz(Pomocnicze.Pliki.Katalog),
            new TcpNasłuchiwacz(Pomocnicze.Tcp.Port),
            new UdpNasłuchiwacz(Pomocnicze.Udp.Port),
            new NetRemotingNasłuchiwacz(Pomocnicze.NetRemoting.Port)
        };

            foreach (INasłuchiwacz nasłuchiwacz in _nasłuchiwacze)
            {
                System.Threading.Thread wątek = new System.Threading.Thread(() => nasłuchiwacz.Start(DodajKomunikator, UsuńKomunikator));

                wątek.Start();
            }

            Console.WriteLine("Naciśnij klawisz, aby zakończyć...");
            Console.ReadKey();

            foreach (INasłuchiwacz nasłuchiwacz in _nasłuchiwacze)
                nasłuchiwacz.Stop();
        }

        static void DodajKomunikator(IKomunikator komunikator)
        {
            lock (_locker)
                _komunikatory.Add(komunikator);

            komunikator.Start(AnalizujKomendę);
        }

        static void UsuńKomunikator(IKomunikator komunikator)
        {
            komunikator.Stop();

            lock (_locker)
                _komunikatory.Remove(komunikator);
        }

        static string AnalizujKomendę(string komenda)
        {
            int indeksSpacji = komenda.IndexOf(" ");

            if (indeksSpacji != -1)
            {
                string usługa = komenda.Substring(0, indeksSpacji).ToLower();

                if (_usługi.ContainsKey(usługa))
                    return _usługi[usługa].OdpowiedzNaKomendę(komenda);
            }

            return String.Empty;
        }
    }
}