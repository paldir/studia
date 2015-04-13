using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psk
{
    abstract class QaClient
    {
        public abstract Communicator Communicator { get; set; }

        public abstract string Qa(string question);
    }
}