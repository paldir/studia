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
        float max;
        float min;

        IList<float> _kolekcja = new List<float>();
        public IList<float> Kolekcja
        {
            get
            {
                return _kolekcja;
            }

            set
            {
                _kolekcja = value;

                max = _kolekcja.Max();
                min = _kolekcja.Min();
            }
        }

        public WykresStopniaPosortowaniaKolekcji()
        {
            InitializeComponent();
        }

        public void PrzedstawKolekcjęNaWykresie<T>(int g)
        {
            int liczbaElementów = Kolekcja.Count;
            int liczbaElementówWGrupie = liczbaElementów / 100;
            float szerokośćSłupka = wykres.Width / 100.0f;
            float maksymalnaWysokośćSłupka = wykres.Height;

            using (Graphics g = wykres.CreateGraphics())
            using (Pen p = new Pen(Color.Black))
                for (int i = 0; i < 100; i++)
                {
                    float średnia = 0;

                    for (int j = i * liczbaElementówWGrupie; j < (i + 1) * liczbaElementówWGrupie; j++)
                        średnia += Kolekcja[j];

                    średnia /= liczbaElementówWGrupie;
                    float xSłupka = i * szerokośćSłupka;

                    g.DrawLine(p, xSłupka, 0, xSłupka, średnia / max * maksymalnaWysokośćSłupka);
                }
        }
    }
}