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
            byte[] pytanie = new byte[] { 0, 1, 0, 0, 0, 6, 0, 4, 0, 3, 0, 5 };
            byte[] odpowiedź = Modbus.PytanieOdpowiedź("localhost", 502, pytanie);
        }
    }
}