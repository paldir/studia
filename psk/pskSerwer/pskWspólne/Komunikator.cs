using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psk
{
    public abstract class Komunikator : IDisposable
    {
        public abstract bool PiszLinię(string linia);
        public abstract string CzytajLinię();
        public abstract void Dispose();
    }
}