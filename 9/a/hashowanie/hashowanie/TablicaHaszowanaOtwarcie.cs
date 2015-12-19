using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hashowanie
{
    class TablicaHaszowanaOtwarcie<T> : HaszowanaTablica<T> where T : IEquatable<T>
    {
        int[] _tablicaDostępu;
        const int _pusteMiejsce = Int32.MaxValue;
        const int _pusteMiejscePoElemencieUsuniętym = Int32.MinValue;
        const int _normalnaWartość = 0;
        static readonly T _wartośćDomyślna = default(T);

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
                int j = h(x, i);
                int dostęp = _tablicaDostępu[j];

                if (dostęp == _pusteMiejsce || dostęp == _pusteMiejscePoElemencieUsuniętym)
                {
                    Tablica[j] = x;
                    _tablicaDostępu[j] = _normalnaWartość;
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
                int j = h(x, i);
                int dostęp = _tablicaDostępu[j];

                if (dostęp == _pusteMiejsce)
                    return false;

                if (dostęp == _normalnaWartość && Tablica[j].Equals(x))
                {
                    Tablica[j] = _wartośćDomyślna;
                    _tablicaDostępu[j] = _pusteMiejscePoElemencieUsuniętym;
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
                int j = h(x, i);

                if (_tablicaDostępu[j] == _pusteMiejsce)
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
                Tablica[i] = _wartośćDomyślna;
                _tablicaDostępu[i] = _pusteMiejsce;
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

        int h(T k, int i)
        {
            return (h_1(k) + Convert.ToInt32(Math.Pow(i, 2))) % M;
        }

        int h_1(T k)
        {
            double alfa = (Math.Sqrt(5) - 1) / 2;
            double alfaRazyK = alfa * FunkcjaKonwertującaNaInt(k);

            return Convert.ToInt32(Math.Floor(M * (alfaRazyK - Math.Floor(alfaRazyK))));
        }
    }
}