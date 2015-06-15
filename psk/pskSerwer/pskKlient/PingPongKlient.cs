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

        public override string PytanieOdpowiedź(string pytanie)
        {
            DateTime tic = DateTime.Now;

            if (Komunikator.PiszLinię(pytanie))
            {
                string odpowiedź = Komunikator.CzytajLinię();
                DateTime toc = DateTime.Now;
                TimeSpan czas = toc - tic;

                return String.Format("{0}\n{1}ms\n", odpowiedź, czas.TotalMilliseconds);
            }
            else
                return Pomocnicze.KomunikatyBłędów.Pytanie;
        }
    }
}