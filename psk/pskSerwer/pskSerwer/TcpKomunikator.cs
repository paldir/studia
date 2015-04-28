using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Sockets;

namespace psk
{
    class TcpKomunikator : Komunikator, IKomunikator
    {
        TcpClient _klientTcp;
        NetworkStream _strumieńSieciowy;
        DelegatKomunikatora _rozłączenie;

        public TcpKomunikator(TcpClient klientTcp, DelegatKomunikatora rozłączenie)
        {
            _klientTcp = klientTcp;
            _rozłączenie = rozłączenie;
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
            catch { return false; }
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
            Stop();
        }

        public void Start(DelegatKomendy obsłużKomendę)
        {
            _strumieńSieciowy = _klientTcp.GetStream();

            try
            {
                while (true)
                {
                    string linia = CzytajLinię();

                    PiszLinię(obsłużKomendę(linia));
                }
            }
            catch (System.IO.IOException)
            {
            }

            _rozłączenie(this);
        }

        public void Stop()
        {
            _strumieńSieciowy.Dispose();
        }
    }
}