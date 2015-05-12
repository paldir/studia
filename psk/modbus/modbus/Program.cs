using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modbus
{
    class Program
    {
        static void Main(string[] args)
        {
            Modbus mb = new Modbus("localhost", 502);
            ushort[] odpowiedź;

            odpowiedź = mb.PobierzRejestry(3, 3);

            mb.UstawRejestry(3, new ushort[] { 14, 40 });
        }
    }
}