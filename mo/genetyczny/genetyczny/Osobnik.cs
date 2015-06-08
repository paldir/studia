using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace genetyczny
{
    class Osobnik : IComparable
    {
        public int[] Geny { get; set; }
        public double DługośćTrasy { get; set; }
        public double Przystosowanie
        {
            get { return 1 / DługośćTrasy; }
        }
        public double PrzystosowanieProcentowo { get; set; }

        public Osobnik(int[] geny, double długośćTrasy)
        {
            Geny = geny;
            DługośćTrasy = długośćTrasy;
        }

        public int CompareTo(object obj)
        {
            return -1 * PrzystosowanieProcentowo.CompareTo(((Osobnik)obj).PrzystosowanieProcentowo);
        }
    }
}