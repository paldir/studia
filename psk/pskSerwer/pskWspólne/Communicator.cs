using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psk
{
    public abstract class Communicator
    {
        public abstract bool WriteLine(string line);
        public abstract string ReadLine();
    }
}