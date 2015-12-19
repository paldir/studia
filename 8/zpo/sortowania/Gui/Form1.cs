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
    public enum PoczątkowaKolekcja
    {
        Losowa,
        OdwrotniePosortowana,
        WstępniePosortowana
    }
    
    public partial class Form1 : Form
    {
        Porównywarka _porównywarka;

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

        public static int Ilość { get; set; }
        public static int LiczbaGrup { get; set; }
        public static PoczątkowaKolekcja PoczątkowaKolekcja { get; set; }

        public Form1()
        {
            InitializeComponent();

            IList<int> kolekcja = new int[Ilość];
            Random los = new Random();

            switch (PoczątkowaKolekcja)
            {
                case Gui.PoczątkowaKolekcja.Losowa:
                    for (int i = 0; i < Ilość; i++)
                        kolekcja[i] = los.Next(1, Ilość);

                    break;

                case Gui.PoczątkowaKolekcja.OdwrotniePosortowana:
                    for (int i = 0; i < Ilość; i++)
                        kolekcja[i] = Ilość - i;

                    break;

                case Gui.PoczątkowaKolekcja.WstępniePosortowana:
                    for (int i = 0; i < Ilość; i++)
                        if (i % 3 == 0)
                            kolekcja[i] = los.Next(1, Ilość);
                        else
                            kolekcja[i] = i;

                    break;
            }

            int maksymalnyKlucz = kolekcja.Select(k => Math.Abs(k)).Max();

            List<WykresStopniaPosortowaniaKolekcji> wykresy = new List<WykresStopniaPosortowaniaKolekcji>() { wykres1, wykres2, wykres3, wykres4, wykres5, wykres6 };
            List<ZadanieZWizualizacją> sortowania = new List<ZadanieZWizualizacją>();

            for (int i = 0; i < metodySortowania.Count; i++)
            {
                WykresStopniaPosortowaniaKolekcji wykres = wykresy[i];
                wykres.MaksymalnyElement = maksymalnyKlucz;
                wykres.TytułWykresu = nazwySortowań[i];
                DaneDoWykresu dane = new DaneDoWykresu(LiczbaGrup);
                wykres.Dane = dane;
                SortowanieZWizualizacją<int> sortowanie = new SortowanieZWizualizacją<int>(metodySortowania[i], kolekcja, k => Math.Abs(k), Średnia, dane, wykres.PrzedstawKolekcjęNaWykresie);

                sortowania.Add(sortowanie);
            }

            _porównywarka = new Porównywarka(sortowania);

            szybkość_Scroll(null, null);
            new System.Threading.Thread(_porównywarka.Porównaj).Start();
        }

        static double Średnia(IList<int> kolekcja, int początek, int koniec)
        {
            double suma = 0;

            for (int i = początek; i < koniec; i++)
                suma += kolekcja[i];

            return suma / (koniec - początek);
        }

        private void szybkość_Scroll(object sender, EventArgs e)
        {
            _porównywarka.OdstępPomiędzyWizualizacją = szybkość.Value;
        }
    }
}