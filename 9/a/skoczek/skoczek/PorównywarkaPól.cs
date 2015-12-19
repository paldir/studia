using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skoczek
{
    class PorównywarkaPól : IComparer<Pole>
    {
        Random _los;
        static readonly int[] _wynik = new int[] { -1, 1 };

        public PorównywarkaPól()
        {
            _los = new Random(DateTime.Now.Millisecond);
        }

        public int Compare(Pole x, Pole y)
        {
            int a = x.IlośćWolnychSąsiadów;
            int b = y.IlośćWolnychSąsiadów;

            if (a > b)
                return 1;

            if (a < b)
                return -1;

            return _wynik[_los.Next(2)];

            //return x.IlośćWolnychSąsiadów.CompareTo(y.IlośćWolnychSąsiadów);
        }
    }
}