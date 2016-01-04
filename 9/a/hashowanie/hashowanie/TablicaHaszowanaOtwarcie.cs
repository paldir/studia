using System;
using System.Linq;

namespace hashowanie
{
    sealed class TablicaHaszowanaOtwarcie<T> : HaszowanaTablica<T> where T : IEquatable<T>
    {
        int[] _tablicaDostępu;
        const int PusteMiejsce = Int32.MaxValue;
        const int PusteMiejscePoElemencieUsuniętym = Int32.MinValue;
        const int NormalnaWartość = 0;
        static readonly T WartośćDomyślna = default(T);

        public T[] Tablica { get; set; }

        public TablicaHaszowanaOtwarcie(int m, Func<T, int> funkcjaKonwertującaNaInt)
            : base(m, funkcjaKonwertującaNaInt)
        {
            Tablica = new T[M];
            _tablicaDostępu = new int[M];

            Wyczyść();
        }

        public override bool Umieść(T x)
        {
            int i = 0;

            while (i != M)
            {
                int j = H(x, i);
                int dostęp = _tablicaDostępu[j];

                if (dostęp == PusteMiejsce || dostęp == PusteMiejscePoElemencieUsuniętym)
                {
                    Tablica[j] = x;
                    _tablicaDostępu[j] = NormalnaWartość;
                    LiczbaElementów++;

                    if (LiczbaElementów > M * Próg)
                        Rehaszuj(TrybRehaszownia.Rozszerz);

                    return true;
                }
                else
                {
                    i++;
                    LiczbaKonfliktówPrzyDodawaniu++;
                }
            }

            return false;
        }

        public override bool Usuń(T x)
        {
            int i = 0;

            while (i != M)
            {
                int j = H(x, i);
                int dostęp = _tablicaDostępu[j];

                if (dostęp == PusteMiejsce)
                    return false;

                if (dostęp == NormalnaWartość && Tablica[j].Equals(x))
                {
                    Tablica[j] = WartośćDomyślna;
                    _tablicaDostępu[j] = PusteMiejscePoElemencieUsuniętym;
                    LiczbaElementów--;

                    return true;
                }
                else
                    i++;
            }

            return false;
        }

        public override bool Zawiera(T x)
        {
            int i = 0;

            while (i != M)
            {
                int j = H(x, i);

                if (_tablicaDostępu[j] == PusteMiejsce)
                    return false;

                if (Tablica[j].Equals(x))
                    return true;
                else
                    i++;
            }

            return false;
        }

        public override void Wyczyść()
        {
            for (int i = 0; i < M; i++)
            {
                Tablica[i] = WartośćDomyślna;
                _tablicaDostępu[i] = PusteMiejsce;
            }

            LiczbaElementów = 0;
        }

        protected override void Rehaszuj(TrybRehaszownia tryb)
        {
            if (Rehaszowanie)
            {
                switch (tryb)
                {
                    case TrybRehaszownia.Rozszerz:
                        M *= 2;

                        break;

                    case TrybRehaszownia.Skurcz:
                        M /= 2;

                        break;
                }

                T[] staraTablica = Tablica.ToArray();
                Tablica = new T[M];
                _tablicaDostępu = new int[M];

                Wyczyść();

                foreach (T element in staraTablica)
                    if (element != null)
                        Umieść(element);
            }
        }

        int H(T k, int i)
        {
            return (H_1(k) + Convert.ToInt32(Math.Pow(i, 2))) % M;
        }

        int H_1(T k)
        {
            double alfa = (Math.Sqrt(5) - 1) / 2;
            double alfaRazyK = alfa * FunkcjaKonwertującaNaInt(k);

            return Convert.ToInt32(Math.Floor(M * (alfaRazyK - Math.Floor(alfaRazyK))));
        }
    }
}