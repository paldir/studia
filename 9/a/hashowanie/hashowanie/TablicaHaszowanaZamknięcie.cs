using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hashowanie
{
    public class TablicaHaszowanaZamknięcie<T> : HaszowanaTablica<T> where T : IEquatable<T>
    {
        public List<T>[] Tablica { get; set; }

        public TablicaHaszowanaZamknięcie(int m, Func<T, int> funkcjaKonwertującaNaInt)
            : base(m, funkcjaKonwertującaNaInt)
        {
            Tablica = new List<T>[M];

            for (int i = 0; i < m; i++)
                Tablica[i] = new List<T>();
        }

        int ObliczHasz(T x)
        {
            return FunkcjaKonwertującaNaInt(x) % M;
        }

        public override bool Umieść(T x)
        {
            List<T> lista = Tablica[ObliczHasz(x)];

            if (lista.Any())
                LiczbaKonfliktówPrzyDodawaniu++;

            LiczbaElementów++;

            lista.Add(x);

            if (LiczbaElementów > M * Próg)
                Rehaszuj(TrybRehaszownia.Rozszerz);

            return true;
        }

        public override bool Usuń(T x)
        {
            if (Tablica[ObliczHasz(x)].Remove(x))
            {
                LiczbaElementów--;

                return true;
            }
            else
                return false;
        }

        public override bool Zawiera(T x)
        {
            return Tablica[ObliczHasz(x)].Contains(x);
        }

        public override void Wyczyść()
        {
            foreach (List<T> lista in Tablica)
                lista.Clear();

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

                List<T>[] staraTablica = Tablica.ToArray();
                Tablica = new List<T>[M];
                LiczbaElementów = 0;

                for (int i = 0; i < M; i++)
                    Tablica[i] = new List<T>();

                foreach (List<T> lista in staraTablica)
                    foreach (T element in lista)
                        Umieść(element);
            }
        }
    }
}