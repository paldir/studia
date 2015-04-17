using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psk
{
    class Klient
    {
        static void Main(string[] args)
        {
            string polecenie = PingPong.Ping(5, 10);

            QaClient client = new PingPongClient();
            client.Communicator = new TcpCom();

            throw new Exception("misz masz");

            using (Communicator communicator = new TcpCom())
            {
                communicator.WriteLine(polecenie);
                Console.WriteLine("Wysyłam: {0}", polecenie);

                string odpowiedź = communicator.ReadLine();

                Console.WriteLine("Odebrano: {0}", odpowiedź);
            }

            Console.ReadKey();
        }
    }
}