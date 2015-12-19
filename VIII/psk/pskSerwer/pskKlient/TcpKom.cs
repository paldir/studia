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

        public TcpKom(string ip, int port)
        {
            _klientTcp = new TcpClient(ip, port);
            _strumieńSieciowy = _klientTcp.GetStream();
        }

        public override bool PiszLinię(string linia)
        {
            if (!linia.EndsWith(Environment.NewLine))
                linia += Environment.NewLine;

            byte[] liniaBajtowa = Encoding.UTF8.GetBytes(linia);

            try
            {
                _strumieńSieciowy.Write(liniaBajtowa, 0, liniaBajtowa.Length);

                return true;
            }
            catch
            { return false; }
        }

        public override string CzytajLinię()
        {
            StringBuilder budowniczyLinii = new StringBuilder();
            string fragmentLinii = String.Empty;

            while (!fragmentLinii.EndsWith(Environment.NewLine))
            {
                byte[] bufor = new byte[Pomocnicze.Tcp.RozmiarBufora];

                _strumieńSieciowy.Read(bufor, 0, bufor.Length);

                fragmentLinii = Encoding.UTF8.GetString(bufor).Replace("\0", String.Empty);

                budowniczyLinii.Append(fragmentLinii);
            }

            return budowniczyLinii.ToString();
        }

        public override void Dispose()
        {
            PiszLinię(Pomocnicze.Tcp.KomendaZakończeniaPołączenia);
            _strumieńSieciowy.Dispose();
            _klientTcp.Close();
        }
    }
}