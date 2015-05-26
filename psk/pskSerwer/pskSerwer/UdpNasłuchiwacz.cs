using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Threading;

namespace psk
{
    class UdpNasłuchiwacz : INasłuchiwacz
    {
        DelegatKomunikatora _rozłączenie;
        UdpKomunikator _komunikator;

        public void Start(DelegatKomunikatora połączenie, DelegatKomunikatora rozłączenie)
        {
            _komunikator = new UdpKomunikator();
            _rozłączenie = rozłączenie;

            połączenie(_komunikator);
        }

        public void Stop()
        {
            _rozłączenie(_komunikator);
        }
    }
}