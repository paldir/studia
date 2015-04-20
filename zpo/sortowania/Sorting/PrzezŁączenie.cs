using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sortowania
{
    public class PrzezŁączenie<T> : IMetodaSortowania<T> where T : IComparable, IComparable<T>
    {
        public void Sortuj(IList<T> kolekcja)
        {
            int n = kolekcja.Count;
            T[] B = new T[n];

            Podziel(kolekcja, 0, n, B);
        }

        void Podziel(IList<T> A, int początek, int koniec, T[] B)
        {
            if (koniec - początek >= 2)
            {
                int środek = (koniec + początek) / 2;

                Podziel(A, początek, środek, B);
                Podziel(A, środek, koniec, B);
                Połącz(A, początek, środek, koniec, B);
                KopiujTablicę(B, początek, koniec, A);
            }
        }

        void Połącz(IList<T> A, int początek, int środek, int koniec, T[] B)
        {
            int i0 = początek;
            int i1 = środek;

            for (int j = początek; j < koniec; j++)
                if (i0 < środek && (i1 >= koniec || A[i0].CompareTo(A[i1]) <= 0))
                {
                    B[j] = A[i0];
                    i0++;
                }
                else
                {
                    B[j] = A[i1];
                    i1++;
                }
        }

        void KopiujTablicę(T[] B, int początek, int koniec, IList<T> A)
        {
            for (int k = początek; k < koniec; k++)
                A[k] = B[k];
        }
    }
}