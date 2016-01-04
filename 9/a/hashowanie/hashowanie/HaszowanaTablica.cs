using System;

namespace hashowanie
{
    public abstract class HaszowanaTablica<T> where T : IEquatable<T>
    {
        public const double Próg = 0.65;
        public enum TrybRehaszownia { Rozszerz, Skurcz };

        protected bool Rehaszowanie { get; set; }
        
        public int M { get; protected set; }
        public int LiczbaElementów { get; protected set; }
        public Func<T, int> FunkcjaKonwertującaNaInt { get; protected set; }
        public int LiczbaKonfliktówPrzyDodawaniu { get; set; }
        public int LiczbaRehaszowań { get; set; }

        public abstract bool Umieść(T x);
        public abstract bool Usuń(T x);
        public abstract bool Zawiera(T x);
        public abstract void Wyczyść();
        protected abstract void Rehaszuj(TrybRehaszownia tryb);

        protected HaszowanaTablica(int m, Func<T, int> funkcjaKonwertującaNaInt)
        {
            M = m;
            FunkcjaKonwertującaNaInt = funkcjaKonwertującaNaInt;
            Rehaszowanie = true;
        }
    }
}