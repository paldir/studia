using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psk
{
    class ChatKlient : PoKlient
    {
        public override Komunikator Komunikator { get; set; }

        public override string PytanieOdpowiedź(string pytanie)
        {
            if (Komunikator.PiszLinię(pytanie))
                return Komunikator.CzytajLinię();
            else
                return Pomocnicze.KomunikatyBłędów.Pytanie;
        }
    }
}