using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kieszonka
{
    static class Rozszerzenia
    {
        public static List<T> Permutacja<T>(this List<T> lista)
        {
            List<T> nowaLista = new List<T>();
            Random los = new Random();

            while (lista.Count != 0)
            {
                T pozycja = lista[los.Next(0, lista.Count)];

                lista.Remove(pozycja);
                nowaLista.Add(pozycja);
            }

            return nowaLista;
        }
    }
}
