using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using sortowania;

namespace Gui
{
    public partial class Form1 : Form
    {
        const int ilość = (int)1e7;

        static readonly List<IMetodaSortowania<int>> metodySortowania = new List<IMetodaSortowania<int>>()
        {
            new Bąbelkowe<int>(),
            new PrzezZliczanie<int>(e=>Math.Abs(e)),
            new PrzezKopcowanie<int>(),
            new PrzezŁączenie<int>(),
            new Szybkie<int>(),
            new PrzezWybór<int>()
        };

        static readonly List<string> nazwySortowań = new List<string>()
        {
            "Bąbelkowe",
            "Przez zliczanie",
            "Przez kopcowanie",
            "Przez łączenie",
            "Szybkie",
            "Przez wybór"
        };

        public Form1()
        {
            InitializeComponent();

            IList<int> kolekcja = new int[ilość];
            Random los = new Random();

            for (int i = 0; i < kolekcja.Count; i++)
                kolekcja[i] = los.Next(1, ilość * 2);

            int maksymalnyKlucz = kolekcja.Select(k => Math.Abs(k)).Max();

            List<WykresStopniaPosortowaniaKolekcji> wykresy = new List<WykresStopniaPosortowaniaKolekcji>() { wykres1, wykres2, wykres3, wykres4, wykres5, wykres6 };
            List<ZadanieZWizualizacją> sortowania = new List<ZadanieZWizualizacją>();

            for (int i = 0; i < metodySortowania.Count; i++)
            {
                wykresy[i].MaksymalnyElement = maksymalnyKlucz;
                wykresy[i].TytułWykresu = nazwySortowań[i];
                SortowanieZWizualizacją<int> sortowanie = new SortowanieZWizualizacją<int>(metodySortowania[i], kolekcja, k => Math.Abs(k), Średnia, 50, wykresy[i].PrzedstawKolekcjęNaWykresie, wykresy[i].AktualizujDaneDoWykresu);

                sortowania.Add(sortowanie);
            }

            Porównywarka porównywarka = new Porównywarka(sortowania);

            new System.Threading.Thread(porównywarka.Porównaj).Start();
        }

        static double Średnia(IList<int> kolekcja, int początek, int koniec)
        {
            double suma = 0;

            for (int i = początek; i < koniec; i++)
                suma += kolekcja[i];

            return suma / (koniec - początek);
        }
    }
}