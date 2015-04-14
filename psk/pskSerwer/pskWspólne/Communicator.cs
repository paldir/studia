using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psk
{
    public abstract class Communicator : IDisposable
    {
        public abstract bool WriteLine(string line);
        public abstract string ReadLine();
        public abstract void Dispose();
    }
}