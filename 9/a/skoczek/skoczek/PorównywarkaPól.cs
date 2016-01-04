using System;
using System.Collections.Generic;

namespace skoczek
{
    class PorównywarkaPól : IComparer<Pole>
    {
        readonly Random _los;
        static readonly int[] Wynik = new int[] { -1, 1 };

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

            return Wynik[_los.Next(2)];

            //return x.IlośćWolnychSąsiadów.CompareTo(y.IlośćWolnychSąsiadów);
        }
    }
}