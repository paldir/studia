using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestyJednostkowe
{
    [TestClass]
    public class TestSortowań
    {
        int[] _kolekcja;
        const int LiczbaElementów = 10000000;

        public TestSortowań()
        {
            _kolekcja = LosujKolekcję();
        }

        /*[TestMethod]
        public void Bąbelkowe()
        {
            int[] kolekcja = new int[LiczbaElementów];
            sortowania.Bąbelkowe<int> sortowanie = new sortowania.Bąbelkowe<int>();

            Array.Copy(_kolekcja, kolekcja, LiczbaElementów);
            sortowanie.Sortuj(kolekcja);
            SprawdźPosortowanie(kolekcja);
        }

        [TestMethod]
        public void PrzezWybór()
        {
            int[] kolekcja = new int[LiczbaElementów];
            sortowania.PrzezWybór<int> sortowanie = new sortowania.PrzezWybór<int>();

            Array.Copy(_kolekcja, kolekcja, LiczbaElementów);
            sortowanie.Sortuj(kolekcja);
            SprawdźPosortowanie(kolekcja);
        }*/

        [TestMethod]
        public void PrzezZliczanie()
        {
            int[] kolekcja = new int[LiczbaElementów];
            sortowania.PrzezZliczanie<int> sortowanie = new sortowania.PrzezZliczanie<int>(e => Math.Abs(e));

            Array.Copy(_kolekcja, kolekcja, LiczbaElementów);
            sortowanie.Sortuj(kolekcja);
            SprawdźPosortowanie(kolekcja);
        }

        [TestMethod]
        public void PrzezKopcowanie()
        {
            int[] kolekcja = new int[LiczbaElementów];
            sortowania.PrzezKopcowanie<int> sortowanie = new sortowania.PrzezKopcowanie<int>();

            Array.Copy(_kolekcja, kolekcja, LiczbaElementów);
            sortowanie.Sortuj(kolekcja);
            SprawdźPosortowanie(kolekcja);
        }

        [TestMethod]
        public void PrzezŁączenie()
        {
            int[] kolekcja = new int[LiczbaElementów];
            sortowania.PrzezŁączenie<int> sortowanie = new sortowania.PrzezŁączenie<int>();

            Array.Copy(_kolekcja, kolekcja, LiczbaElementów);
            sortowanie.Sortuj(kolekcja);
            SprawdźPosortowanie(kolekcja);
        }

        [TestMethod]
        public void Szybkie()
        {
            int[] kolekcja = new int[LiczbaElementów];
            sortowania.Szybkie<int> sortowanie = new sortowania.Szybkie<int>();

            Array.Copy(_kolekcja, kolekcja, LiczbaElementów);
            sortowanie.Sortuj(kolekcja);
            SprawdźPosortowanie(kolekcja);
        }

        int[] LosujKolekcję()
        {
            int[] kolekcja = new int[LiczbaElementów];
            Random los = new Random();

            for (int i = 0; i < LiczbaElementów; i++)
                kolekcja[i] = los.Next(1, LiczbaElementów);

            return kolekcja;
        }

        void SprawdźPosortowanie(int[] kolekcja)
        {
            for (int i = 1; i < kolekcja.Length; i++)
                Assert.IsTrue(kolekcja[i - 1] <= kolekcja[i]);
        }
    }
}