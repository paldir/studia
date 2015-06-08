using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace genetyczny
{
    struct KawałekRuletki
    {
        public double _Początek { get; set; }
        public double Koniec { get; set; }
        public double Zakres
        {
            get { return Koniec - _Początek; }
        }

        public KawałekRuletki(double początek, double koniec)
            : this()
        {
            _Początek = początek;
            Koniec = koniec;
        }
    }
}