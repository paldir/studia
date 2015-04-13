using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psk
{
    public delegate void CommunicatorDelegate(ICommunicator communicator);

    public interface IListener
    {
        void Start(CommunicatorDelegate onConnect, CommunicatorDelegate onDisconnect);
        void Stop();
    }
}