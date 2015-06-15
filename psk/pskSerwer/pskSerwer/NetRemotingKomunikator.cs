using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psk
{
    public class NetRemotingKomunikator : MarshalByRefObject, IKomunikator, IDisposable
    {
        public static DelegatKomunikatora Połączenie { get; set; }
        public static DelegatKomunikatora Rozłączenie { get; set; }

        public DelegatKomendy _obsługaKomendy;

        public NetRemotingKomunikator()
        {
            Połączenie(this);
        }

        public string PytanieOdpowiedź(string pytanie)
        {
            return _obsługaKomendy(pytanie);
        }

        public void Start(DelegatKomendy obsłużKomendę)
        {
            _obsługaKomendy = obsłużKomendę;
        }

        public void Stop()
        {

        }

        public void Dispose()
        {
            Rozłączenie(this);
        }
    }
}