using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gui
{
    public partial class WykresStopniaPosortowaniaKolekcji : UserControl
    {
        public IList<float> Kolekcja { get; set; }

        public WykresStopniaPosortowaniaKolekcji()
        {
            InitializeComponent();
        }

        public void PrzedstawKolekcjęNaWykresie()
        {
            int liczbaElementów = Kolekcja.Count;
            int liczbaElementówWGrupie = liczbaElementów / 100;

            for (int i = 0; i < 100; i++)
            {
                float średnia = 0;

                for (int j = i * liczbaElementówWGrupie; j < (i + 1) * liczbaElementówWGrupie; j++)
                    średnia += Kolekcja[j];

                średnia /= liczbaElementówWGrupie;


            }
        }
    }
}