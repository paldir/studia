using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alignment
{
    class Komórka
    {
        public int Liczba { get; set; }
        public Strzałka Strzałka { get; set; }
    }

    public enum Strzałka
    {
        Brak,
        Skos,
        Góra,
        Lewo
    }
}
