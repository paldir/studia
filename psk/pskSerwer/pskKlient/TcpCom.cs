using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Sockets;

namespace psk
{
    public class TcpCom : Communicator
    {
        TcpClient _tcpClient;
        NetworkStream _networkStream;

        public TcpCom()
        {
            _tcpClient = new TcpClient("localhost", Auxiliary.Tcp.Port);
            _networkStream = _tcpClient.GetStream();
        }

        public override bool WriteLine(string line)
        {
            try
            {
                if (!line.EndsWith(Environment.NewLine))
                    line += Environment.NewLine;

                byte[] byteLine = Encoding.UTF8.GetBytes(line);

                _networkStream.Write(byteLine, 0, byteLine.Length);

                return true;
            }
            catch
            { return false; }
        }

        public override string ReadLine()
        {
            string line = String.Empty;
            byte[] buffer = new byte[Auxiliary.Tcp.BufferSize];

            while (!line.EndsWith(Environment.NewLine))
            {
                _networkStream.Read(buffer, 0, buffer.Length);

                line += Encoding.UTF8.GetString(buffer).Replace("\0", String.Empty);
            }

            return line;
        }

        public override void Dispose()
        {
            _networkStream.Dispose();
            _tcpClient.Close();
        }
    }
}