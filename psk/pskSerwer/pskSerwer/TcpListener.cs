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
        List<TcpCommunicator> _tcpCommunicators;
        CommunicatorDelegate _onDisconnect;

        public void Start(CommunicatorDelegate onConnect, CommunicatorDelegate onDisconnect)
        {
            _thread = Thread.CurrentThread;
            _tcpCommunicators = new List<TcpCommunicator>();
            _onDisconnect = onDisconnect;
            System.Net.Sockets.TcpListener tcpListener = new System.Net.Sockets.TcpListener(Auxiliary.Tcp.IpAddress, Auxiliary.Tcp.Port);

            tcpListener.Start();

            while (true)
            {
                if (tcpListener.Pending())
                {
                    TcpCommunicator tcpCommunicator = new TcpCommunicator(tcpListener.AcceptTcpClient());
                    Thread thread = new Thread(() => onConnect(tcpCommunicator));

                    thread.Start();
                    _tcpCommunicators.Add(tcpCommunicator);
                }
                else
                {
                    Thread.Sleep(Auxiliary.SleepTimeOut);

                    foreach (TcpCommunicator tcpCommunicator in _tcpCommunicators)
                        if (!tcpCommunicator.Connected)
                            onDisconnect(tcpCommunicator);

                    _tcpCommunicators.RemoveAll(c => !c.Connected);
                }
            }
        }

        public void Stop()
        {
            foreach (TcpCommunicator tcpCommuncator in _tcpCommunicators)
                _onDisconnect(tcpCommuncator);

            _thread.Abort();
        }
    }
}