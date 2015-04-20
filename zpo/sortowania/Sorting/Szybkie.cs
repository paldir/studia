using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sortowania
{
    public class Szybkie<T> : IMetodaSortowania<T> where T : IComparable, IComparable<T>
    {
        public void Sortuj(IList<T> kolekcja)
        {
            Szybko(kolekcja, 0, kolekcja.Count - 1);
        }

        void Szybko(IList<T> A, int początek, int koniec)
        {
            if (początek < koniec)
            {
                int p = Podział(A, początek, koniec);

                Szybko(A, początek, p - 1);
                Szybko(A, p + 1, koniec);
            }
        }

        int Podział(IList<T> A, int początek, int koniec)
        {
            int indeksPodziału = (początek + koniec) / 2;
            T wartośćPodziału = A[indeksPodziału];

            A.Zamień(indeksPodziału, koniec);

            int iTmp = początek;

            for (int i = początek; i <= koniec - 1; i++)
                if (A[i].CompareTo(wartośćPodziału) < 0)
                {
                    A.Zamień(i, iTmp);

                    iTmp++;
                }

            A.Zamień(iTmp, koniec);

            return iTmp;
        }
    }
}