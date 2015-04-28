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

        Dictionary<string, List<string>> wiadomości;

        public string OdpowiedzNaKomendę(string komenda)
        {
            komenda = komenda.Replace("chat ", String.Empty);
            TrybObsługi tryb = (TrybObsługi)Enum.Parse(typeof(TrybObsługi), komenda.Substring(0, komenda.IndexOf(' ')), true);

            switch (tryb)
            {
            }

            return "Usługa działa";
        }
    }
}