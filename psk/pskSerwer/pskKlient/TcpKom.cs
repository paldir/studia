using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Sockets;

namespace psk
{
    public class TcpKom : Komunikator
    {
        TcpClient _klientTcp;
        NetworkStream _strumieńSieciowy;

        public TcpKom()
        {
            _klientTcp = new TcpClient("localhost", Pomocnicze.Tcp.Port);
            _strumieńSieciowy = _klientTcp.GetStream();
        }

        public override bool PiszLinię(string linia)
        {
            try
            {
                if (!linia.EndsWith(Environment.NewLine))
                    linia += Environment.NewLine;

                byte[] liniaBajtowa = Encoding.UTF8.GetBytes(linia);

                _strumieńSieciowy.Write(liniaBajtowa, 0, liniaBajtowa.Length);

                return true;
            }
            catch
            { return false; }
        }

        public override string CzytajLinię()
        {
            string linia = String.Empty;
            byte[] bufor = new byte[Pomocnicze.Tcp.RozmiarBufora];

            while (!linia.EndsWith(Environment.NewLine))
            {
                _strumieńSieciowy.Read(bufor, 0, bufor.Length);

                linia += Encoding.UTF8.GetString(bufor).Replace("\0", String.Empty);
            }

            return linia;
        }

        public override void Dispose()
        {
            _strumieńSieciowy.Dispose();
            _klientTcp.Close();
        }
    }
}