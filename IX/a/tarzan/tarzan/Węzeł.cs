using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tarzan
{
    class Węzeł
    {
        public List<Węzeł> Dzieci { get; set; }
        public Węzeł Rodzic { get; set; }
        public ConsoleColor Kolor { get; set; }
        public string Nazwa { get; set; }
        public Węzeł Przodek { get; set; }
        public int Stopień { get; set; }

        public Węzeł(string nazwa)
        {
            Nazwa = nazwa;
            Dzieci = new List<Węzeł>();
            Kolor = ConsoleColor.White;
        }

        public Węzeł(string nazwa, Węzeł rodzic)
            : this(nazwa)
        {
            Rodzic = rodzic;
        }
    }
}