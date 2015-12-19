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

            mb.UstawRejestry(1, new ushort[] { 19, 91 });

            ushort[] odpowiedź = mb.PobierzRejestry(1, 2);
        }
    }
}