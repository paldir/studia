using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psk
{
    public class PingPong : IUsługa
    {
        static Random _los = new Random();

        public string OdpowiedzNaKomendę(string polecenie)
        {
            string[] ss = polecenie.Split(' ');

            if (ss[0] != "ping")
                return Pomocnicze.KomunikatyBłędów.Usługa;

            int rozmiar0;

            if (!int.TryParse(ss[1], out rozmiar0))
                return Pomocnicze.KomunikatyBłędów.Usługa;

            return String.Format("pong {0}\n", Śmieci(rozmiar0));
        }

        static string Śmieci(int ile)
        {
            StringBuilder budowniczyŚmieci = new StringBuilder();

            for (int i = 0; i < ile; i++)
                budowniczyŚmieci.Append(Convert.ToChar('a' + _los.Next(0, 26)));

            return budowniczyŚmieci.ToString();
        }
    }
}