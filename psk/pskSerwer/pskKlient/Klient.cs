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
            Communicator communicator = new FilesCom();

            communicator.WriteLine(polecenie);
            Console.WriteLine("Wysyłam: {0}", polecenie);

            string odpowiedź = communicator.ReadLine();

            Console.WriteLine("Odebrano: {0}", odpowiedź);
            Console.ReadKey();
        }
    }
}