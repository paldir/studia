using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cyklEulera
{
    class Program
    {
        static List<Wierzchołek> CyklEulera(List<Wierzchołek> graf, Wierzchołek u)
        {
            List<Wierzchołek> lista = new List<Wierzchołek>();
            Stack<Wierzchołek> stos = new Stack<Wierzchołek>();

            foreach (Wierzchołek wierzchołek in graf)
                if (wierzchołek.Sąsiedzi.Count % 2 != 0)
                    return lista;

            stos.Push(u);

            while (stos.Count != 0)
            {
                Wierzchołek v = stos.Peek();

                if (v.Sąsiedzi.Count == 0)
                {
                    stos.Pop();
                    lista.Insert(0, v);
                }
                else
                {
                    Wierzchołek w = graf.Find(g => g.Nazwa == v.Sąsiedzi.First());

                    stos.Push(w);
                    v.Sąsiedzi.RemoveAt(0);
                    w.Sąsiedzi.Remove(v.Nazwa);
                }
            }

            return lista;
        }

        static void Main(string[] args)
        {
            List<Wierzchołek> graf = new List<Wierzchołek>()
            {
                new Wierzchołek(1, 2, 3),
                new Wierzchołek(2, 1, 3),
                new Wierzchołek(3, 1, 2, 4, 5),
                new Wierzchołek(4, 3, 5),
                new Wierzchołek(5, 3, 4)
            };

            /*List<Wierzchołek> graf = new List<Wierzchołek>()
            {
                new Wierzchołek(1, 2, 3, 5),
                new Wierzchołek(2, 1, 3, 4),
                new Wierzchołek(3, 1, 2, 4, 5),
                new Wierzchołek(4, 3, 5, 2),
                new Wierzchołek(5, 3, 4, 1)
            };*/

            /*List<Wierzchołek> graf = new List<Wierzchołek>()
            {
                new Wierzchołek(1, 2, 3, 5, 4),
                new Wierzchołek(2, 1, 3, 4, 5),
                new Wierzchołek(3, 1, 2, 4, 5),
                new Wierzchołek(4, 3, 5, 2, 1),
                new Wierzchołek(5, 3, 4, 1, 2)
            };*/

            foreach (Wierzchołek wierzchołek in CyklEulera(graf, graf[0]))
                Console.Write("{0} ", wierzchołek.Nazwa);

            Console.ReadKey();
        }
    }
}