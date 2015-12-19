using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace regExILinq
{
    class Biegacz
    {
        public int Miejsce { get; set; }
        public int Numer { get; set; }
        public int RokUrodzenia { get; set; }
        public string Miejscowość { get; set; }
        public TimeSpan Czas { get; set; }

        public Biegacz(int miejsce, int numer, int rokUrodzenia, string miejscowość, TimeSpan czas)
        {
            Miejsce = miejsce;
            Numer = numer;
            RokUrodzenia = rokUrodzenia;
            Miejscowość = miejscowość;
            Czas = czas;
        }
    }
}