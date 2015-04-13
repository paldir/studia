using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Sockets;
using System.Threading;

namespace psk
{
    /*class TcpServer : ICommunicator
    {
        TcpListener tcpListener;
        Thread thread;

        public override void StartListening()
        {
            tcpListener = new TcpListener(System.Net.Dns.GetHostEntry("localhost").AddressList[0], 14400);
            thread = System.Threading.Thread.CurrentThread;
            byte[] buffer = new byte[256];
            string request = String.Empty;

            tcpListener.Start();

            while (true)
            {
                if (tcpListener.Pending())
                    using (TcpClient tcpClient = tcpListener.AcceptTcpClient())
                    using (NetworkStream networkStream = tcpClient.GetStream())
                        while (tcpClient.Available != 0)
                        {
                            int realBufferSize = networkStream.Read(buffer, 0, buffer.Length);
                            request += Encoding.UTF8.GetString(buffer).Replace("\0", String.Empty);

                            if (realBufferSize != 256)
                            {
                                string response;
                                IServiceModule service = DetermineServiceByRequest(Serwer.Services, request);

                                if (service == null)
                                    response = "Polecenie nie znalezione." + Environment.NewLine;
                                else
                                    response = service.Response(request);

                                byte[] byteResponse = Encoding.UTF8.GetBytes(response);

                                networkStream.Write(byteResponse, 0, byteResponse.Length);
                            }
                        }
                else
                {
                    Thread.Sleep(100);

                    continue;
                }
            }

        }

        public override void StopListening()
        {
            thread.Abort();
        }
    }*/
}