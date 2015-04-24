using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psk
{
    class PingPongKlient : PoKlient
    {
        public override Komunikator Komunikator { get; set; }

        public override string PytanieOdpowiedź(string question)
        {
            Komunikator.PiszLinię(question);

            return Komunikator.CzytajLinię();
        }
    }
}