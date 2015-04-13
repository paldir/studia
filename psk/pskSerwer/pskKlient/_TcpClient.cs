using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psk
{
    /*class TcpClient : ICommunicatorClient
    {
        public string SendRequest(string command)
        {
            byte[] request = Encoding.UTF8.GetBytes(command);
            byte[] buffer = new byte[256];
            string response = String.Empty;

            using (System.Net.Sockets.TcpClient tcpClient = new System.Net.Sockets.TcpClient("localhost", 14400))
            using (System.Net.Sockets.NetworkStream networkStream = tcpClient.GetStream())
            {
                networkStream.Write(request, 0, request.Length);

                while (!response.EndsWith(Environment.NewLine))
                    if (tcpClient.Available != 0)
                    {
                        networkStream.Read(buffer, 0, buffer.Length);

                        response += Encoding.UTF8.GetString(buffer).Replace("\0", String.Empty);
                    }
            }

            return response;
        }
    }*/
}