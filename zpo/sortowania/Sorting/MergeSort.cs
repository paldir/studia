using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sortowania
{
    public class MergeSort<T> : ISortMethod<T> where T : IComparable, IComparable<T>
    {
        public void Sort(IList<T> collection)
        {
            int n = collection.Count;
            T[] B = new T[n];

            Split(collection, 0, n, B);
        }

        void Split(IList<T> A, int iBegin, int iEnd, T[] B)
        {
            if (iEnd - iBegin >= 2)
            {
                int iMiddle = (iEnd + iBegin) / 2;

                Split(A, iBegin, iMiddle, B);
                Split(A, iMiddle, iEnd, B);
                Merge(A, iBegin, iMiddle, iEnd, B);
                CopyArray(B, iBegin, iEnd, A);
            }
        }

        void Merge(IList<T> A, int iBegin, int iMiddle, int iEnd, T[] B)
        {
            int i0 = iBegin;
            int i1 = iMiddle;

            for (int j = iBegin; j < iEnd; j++)
                if (i0 < iMiddle && (i1 >= iEnd || A[i0].CompareTo(A[i1]) <= 0))
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

        void CopyArray(T[] B, int iBegin, int iEnd, IList<T> A)
        {
            for (int k = iBegin; k < iEnd; k++)
                A[k] = B[k];
        }
    }
}