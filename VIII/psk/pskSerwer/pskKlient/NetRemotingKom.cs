using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Security.Permissions;

namespace psk
{
    [SecurityPermission(SecurityAction.Demand)]
    class NetRemotingKom : Komunikator
    {
        HttpClientChannel _kanał;
        string _linia;

        public NetRemotingKom(string urlUsługi)
        {
            IDictionary konfiguracja = new Hashtable();
            konfiguracja["port"] = Pomocnicze.NetRemoting.Port.ToString();

            _kanał = new HttpClientChannel(konfiguracja, null);
            WellKnownClientTypeEntry zdalnyTyp = new WellKnownClientTypeEntry(typeof(NetRemotingKomunikator), urlUsługi);

            ChannelServices.RegisterChannel(_kanał, false);
            RemotingConfiguration.RegisterWellKnownClientType(zdalnyTyp);
        }

        public override bool PiszLinię(string linia)
        {
            try
            {
                using (NetRemotingKomunikator komunikator = new NetRemotingKomunikator())
                    _linia = komunikator.PytanieOdpowiedź(linia);

                return true;
            }
            catch { return false; }
        }

        public override string CzytajLinię()
        {
            if (String.IsNullOrEmpty(_linia))
                return String.Empty;
            else
                return _linia;
        }

        public override void Dispose()
        {
            ChannelServices.UnregisterChannel(_kanał);
        }
    }
}