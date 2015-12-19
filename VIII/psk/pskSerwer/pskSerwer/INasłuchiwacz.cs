using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psk
{
    public delegate void DelegatKomunikatora(IKomunikator komunikator);

    public interface INasłuchiwacz
    {
        void Start(DelegatKomunikatora połączenie, DelegatKomunikatora rozłączenie);
        void Stop();
    }
}