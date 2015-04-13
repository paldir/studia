using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psk
{
    class PingPongClient : QaClient
    {
        public override Communicator Communicator { get; set; }

        public override string Qa(string question)
        {
            Communicator.WriteLine(question);

            return Communicator.ReadLine();
        }
    }
}