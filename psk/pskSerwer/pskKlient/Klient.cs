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

            PoKlient klient = new PingPongKlient();
            klient.Komunikator = new TcpKom();

            throw new Exception("misz masz");

            using (Komunikator komunikator = new TcpKom())
            {
                komunikator.PiszLinię(polecenie);
                Console.WriteLine("Wysyłam: {0}", polecenie);

                string odpowiedź = komunikator.CzytajLinię();

                Console.WriteLine("Odebrano: {0}", odpowiedź);
            }

            Console.ReadKey();
        }
    }
}