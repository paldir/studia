using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;

namespace psk
{
    class TcpListener : IListener
    {
        Thread _thread;

        public void Start(CommunicatorDelegate onConnect, CommunicatorDelegate onDisconnect)
        {
            System.Net.Sockets.TcpListener tcpListener = new System.Net.Sockets.TcpListener(Auxiliary.Tcp.IpAddress, Auxiliary.Tcp.Port);
            _thread = Thread.CurrentThread;

            while (true)
            {
                if (tcpListener.Pending())
                {
                    TcpCommunicator tcpCommunicator = new TcpCommunicator(tcpListener.AcceptTcpClient());

                    onConnect(tcpCommunicator);
                }
                else
                    Thread.Sleep(Auxiliary.SleepTimeOut);
            }
        }

        public void Stop()
        {
            _thread.Abort();
        }
    }
}