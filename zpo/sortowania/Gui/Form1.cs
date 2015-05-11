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
            new Bąbelkowe<int>()/*,
            new PrzezZliczanie<int>(e=>Math.Abs(e)),
            new PrzezKopcowanie<int>(),
            new PrzezŁączenie<int>(),
            new Szybkie<int>(),
            new PrzezWybór<int>()*/
        };

        public Form1()
        {
            InitializeComponent();

            IList<int> kolekcja = new int[ilość];
            Random los = new Random();

            for (int i = 0; i < kolekcja.Count; i++)
                kolekcja[i] = los.Next(1, ilość * 2);

            List<System.Threading.ThreadStart> wątkiSortowania = new List<System.Threading.ThreadStart>();

            foreach (IMetodaSortowania<int> metodaSortowania in metodySortowania)
            {
                List<int> tablica = new List<int>(kolekcja);

                wykresStopniaPosortowaniaKolekcji.Kolekcja = tablica.Select(e => Convert.ToSingle(e)).ToList();
                wątkiSortowania.Add(() => tablica.Sortuj(metodaSortowania));
            }

            SortowanieZWizualizacją sortowanieZWizualizacją = new SortowanieZWizualizacją(wątkiSortowania, wykresStopniaPosortowaniaKolekcji.PrzedstawKolekcjęNaWykresie);
            Porównywarka porównywarka = new Porównywarka(sortowanieZWizualizacją);

            porównywarka.Porównaj();
        }
    }
}