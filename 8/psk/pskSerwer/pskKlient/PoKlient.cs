using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psk
{
    abstract class PoKlient
    {
        public abstract Komunikator Komunikator { get; set; }

        public abstract string PytanieOdpowiedź(string pytanie);
    }
}