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
        List<int> _daneDoWykresu;
        int _maksymalnyElement;

        public WykresStopniaPosortowaniaKolekcji()
        {
            InitializeComponent();
        }

        public void PrzedstawKolekcjęNaWykresie()
        {

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);

            if (_daneDoWykresu == null)
                return;

            int liczbaElementów = _daneDoWykresu.Count;
            float szerokośćSłupka = this.Width / liczbaElementów;
            float maksymalnaWysokośćSłupka = this.Height;

            Graphics g = e.Graphics;
            using (Pen p = new Pen(Color.Black))
                for (int i = 0; i < liczbaElementów; i++)
                {
                    float xSłupka = i * szerokośćSłupka;
                    float x1 = i * szerokośćSłupka;
                    float y1 = maksymalnaWysokośćSłupka;
                    float x2 = x1;
                    float y2 = maksymalnaWysokośćSłupka - _daneDoWykresu[i] / Convert.ToSingle(_maksymalnyElement) * maksymalnaWysokośćSłupka;

                    g.DrawLine(p, x1, y1, x2, y2);
                }
        }

        public void AktualizujDaneDoWykresu(List<int> dane)
        {
            _daneDoWykresu = dane;
            _maksymalnyElement = _daneDoWykresu.Max();
        }
    }
}