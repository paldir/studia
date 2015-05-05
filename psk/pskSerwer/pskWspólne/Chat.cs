using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psk
{
    public class Chat : IUsługa
    {
        enum TrybObsługi
        {
            Czytaj,
            Pisz
        };

        Dictionary<string, List<string>> _wiadomości;

        public Chat()
        {
            _wiadomości = new Dictionary<string, List<string>>();
        }

        public string OdpowiedzNaKomendę(string komenda)
        {
            string[] argumenty = komenda.Split(' ');

            if (argumenty.Length > 2)
            {
                TrybObsługi tryb = (TrybObsługi)Enum.Parse(typeof(TrybObsługi), argumenty[1], true);

                switch (tryb)
                {
                    case TrybObsługi.Czytaj:

                        break;

                    case TrybObsługi.Pisz:

                        break;
                }
            }

            return "Niepoprawna składnia polecenia.";
        }
    }
}