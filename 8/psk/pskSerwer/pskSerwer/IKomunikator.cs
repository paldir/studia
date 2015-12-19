using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psk
{
    public delegate string DelegatKomendy(string komenda);

    public interface IKomunikator
    {
        void Start(DelegatKomendy obsłużKomendę);
        void Stop();
    }
}