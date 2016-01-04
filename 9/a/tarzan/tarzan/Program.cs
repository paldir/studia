using System;
using System.Collections.Generic;
using System.Xml;

namespace tarzan
{
    class Program
    {
        static ParaWęzłów[] _p;
        
        static void Main()
        {
            XmlDocument konfig = new XmlDocument();

            konfig.Load("drzewo.xml");

            if (konfig.DocumentElement != null)
            {
                Węzeł korzeń = ParaWęzłów.Korzeń = new Węzeł(konfig.DocumentElement.Name);

                Stwórz(korzeń, konfig.DocumentElement);

                _p = new[] { new ParaWęzłów("g", "d"), new ParaWęzłów("g","f"), new ParaWęzłów("b", "d") };

                Tarjan(korzeń);
            }

            Console.ReadKey();
        }

        static void Tarjan(Węzeł u)
        {
            MakeSet(u);

            u.Przodek = u;

            foreach (Węzeł v in u.Dzieci)
            {
                Tarjan(v);
                Union(u, v);

                Find(u).Przodek = u;
            }

            u.Kolor = ConsoleColor.Black;
            List<Węzeł> węzłyDoSprawdzenia = new List<Węzeł>();

            foreach (ParaWęzłów para in _p)
            {
                Węzeł a = para.A;
                Węzeł b = para.B;

                if (a == u)
                    węzłyDoSprawdzenia.Add(b);

                if (b == u)
                    węzłyDoSprawdzenia.Add(a);
            }

            foreach (Węzeł v in węzłyDoSprawdzenia)
                if (v.Kolor == ConsoleColor.Black)
                    Console.WriteLine("Najstarszy wspólny przodek {0} i {1} to {2}.", u.Nazwa, v.Nazwa, Find(v).Przodek.Nazwa);
        }

        static void MakeSet(Węzeł x)
        {
            x.Rodzic = x;
            x.Stopień = 0;
        }

        static void Union(Węzeł x, Węzeł y)
        {
            Węzeł xRoot = Find(x);
            Węzeł yRoot = Find(y);

            if (xRoot.Stopień > yRoot.Stopień)
                yRoot.Rodzic = xRoot;
            else if (xRoot.Stopień < yRoot.Stopień)
                xRoot.Rodzic = yRoot;
            else if (xRoot != yRoot)
            {
                yRoot.Rodzic = xRoot;
                xRoot.Stopień++;
            }
        }

        static Węzeł Find(Węzeł x)
        {
            if (x.Rodzic == x)
                return x;
            else
                x.Rodzic = Find(x.Rodzic);

            return x.Rodzic;
        }

        static void Stwórz(Węzeł węzeł, XmlElement elementXml)
        {
            foreach (XmlElement elementPotomnyXml in elementXml.ChildNodes)
            {
                Węzeł węzełPotomny = new Węzeł(elementPotomnyXml.Name, węzeł);

                węzeł.Dzieci.Add(węzełPotomny);

                Stwórz(węzełPotomny, elementPotomnyXml);
            }
        }
    }
}