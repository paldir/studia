using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;

namespace psk
{
    public static class Pomocnicze
    {
        public const int CzasSpania = 1;
        public const string Pulpit = @"C:\Users\paldir\Desktop";

        public static class Pliki
        {
            public static readonly string Katalog = System.IO.Path.Combine(Pulpit, "tmp");
        }

        public static class Tcp
        {
            public const string AdresIp = "localhost";
            public const int Port = 14400;
            public const int RozmiarBufora = 256;
            public const string KomendaZakończeniaPołączenia = "---<! END CONNECTION >!---";
        }

        public static class Udp
        {
            public const string Ip = "localhost";
            public const int Port = 14401;
            public const int RozmiarBufora = 256;
        }

        public static class NetRemoting
        {
            public const string Ip = "localhost";
            public const int Port = 14402;
            public const string NazwaUsługi = "NetKom";
        }

        public static class KomunikatyBłędów
        {
            public const string Pytanie = "Błąd podczas wysyłania polecenia.\n";
            public const string Odpowiedź = "Błąd podczas wysyłania odpowiedzi.\n";
            public const string Usługa = "Niepoprawna składnia polecenia.\n";
        }
    }
}