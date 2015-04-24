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

        public static string Ping(int rozmiarPolecenia, int rozmiarOdpowiedzi)
        {
            string x = String.Format("ping {0} ", rozmiarOdpowiedzi);

            return x + Śmieci(rozmiarPolecenia);
        }

        public string OdpowiedzNaKomendę(string polecenie)
        {
            string[] ss = polecenie.Split(' ');

            if (ss[0] != "ping")
                return "Błąd!";

            int rozmiar0;

            if (!int.TryParse(ss[1], out rozmiar0))
                return "Błąd!";

            return "pong " + Śmieci(rozmiar0) + Environment.NewLine;
        }

        static string Śmieci(int ile)
        {
            string śmieci = String.Empty;

            for (int i = 0; i < ile; i++)
                śmieci += (char)('a' + _los.Next(0, 26));

            return śmieci;
        }
    }
}