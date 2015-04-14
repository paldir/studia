using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;

namespace psk
{
    public static class Auxiliary
    {
        public const int SleepTimeOut = 1000;

        public static class Files
        {
            public const string Directory = @"C:\Users\pk\Desktop\tmp";
        }

        public static class Tcp
        {
            public static readonly IPAddress IpAddress = Dns.GetHostEntry("localhost").AddressList[0];
            public const int Port = 14400;
            public const int BufferSize = 256;
        }
    }
}