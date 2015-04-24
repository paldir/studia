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
        public const int CzasSpania = 1000;

        public static class Pliki
        {
            public const string Katalog = @"C:\Users\pk\Desktop\tmp";
        }

        public static class Tcp
        {
            public static readonly IPAddress AdresIp = Dns.GetHostEntry("localhost").AddressList[0];
            public const int Port = 14400;
            public const int RozmiarBufora = 256;
        }
    }
}