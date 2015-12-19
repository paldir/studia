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
            if (!linia.EndsWith(Environment.NewLine))
                linia += Environment.NewLine;

            byte[] liniaBajtowa = Encoding.UTF8.GetBytes(linia);

            try
            {
                _strumieńSieciowy.Write(liniaBajtowa, 0, liniaBajtowa.Length);

                return true;
            }
            catch { return false; }
        }

        public override string CzytajLinię()
        {
            string fragmentLinii = String.Empty;
            StringBuilder budowniczyLinii = new StringBuilder();

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