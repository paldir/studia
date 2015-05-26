using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psk
{
    class UdpKom : Komunikator
    {
        System.Net.Sockets.UdpClient _klient;

        public UdpKom()
        {
            _klient = new System.Net.Sockets.UdpClient();
        }

        public override bool PiszLinię(string linia)
        {
            //_klient.Send()
        }

        public override string CzytajLinię()
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}