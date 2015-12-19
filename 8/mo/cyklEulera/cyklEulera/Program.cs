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
            List<Wierzchołek> graf = new List<Wierzchołek>();

            using (System.IO.StreamReader strumień = new System.IO.StreamReader("input.txt"))
                while (!strumień.EndOfStream)
                {
                    IEnumerable<int> linia = strumień.ReadLine().Split('\t').Select(w => Convert.ToInt32(w));
                    int wierzchołek = linia.First();

                    graf.Add(new Wierzchołek(wierzchołek, linia.Except(new int[] { wierzchołek })));
                }

            foreach (Wierzchołek wierzchołek in CyklEulera(graf, graf[0]))
                Console.Write("{0} ", wierzchołek.Nazwa);

            Console.ReadKey();
        }
    }
}