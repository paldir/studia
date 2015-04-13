using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Sockets;

namespace psk
{
    class TcpCommunicator : Communicator, ICommunicator
    {
        TcpClient _tcpClient;
        
        public TcpCommunicator(TcpClient tcpClient)
        {
            _tcpClient = tcpClient;
        }
        
        public override bool WriteLine(string line)
        {
            throw new NotImplementedException();
        }

        public override string ReadLine()
        {
            throw new NotImplementedException();
        }

        public void Start(CommandDelegate onCommand)
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}