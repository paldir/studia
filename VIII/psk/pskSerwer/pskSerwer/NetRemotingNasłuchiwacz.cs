using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Security.Permissions;

namespace psk
{
    [SecurityPermission(SecurityAction.Demand)]
    class NetRemotingNasłuchiwacz : INasłuchiwacz
    {
        HttpServerChannel _kanał;
        int _port;

        public NetRemotingNasłuchiwacz(int port)
        {
            _port = port;
        }

        public void Start(DelegatKomunikatora połączenie, DelegatKomunikatora rozłączenie)
        {
            _kanał = new HttpServerChannel(_port);
            NetRemotingKomunikator.Połączenie = połączenie;
            NetRemotingKomunikator.Rozłączenie = rozłączenie;

            ChannelServices.RegisterChannel(_kanał, false);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(NetRemotingKomunikator), Pomocnicze.NetRemoting.NazwaUsługi, WellKnownObjectMode.SingleCall);
        }

        public void Stop()
        {
            ChannelServices.UnregisterChannel(_kanał);
        }
    }
}