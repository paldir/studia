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
        IList<int> _daneDoWykresu;
        IList<float> _stopniePosortowania;

        public int MaksymalnyElement { get; set; }

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
            if (_daneDoWykresu == null)
                return;

            int liczbaElementów = _daneDoWykresu.Count;
            float szerokośćSłupka = Width / Convert.ToSingle(liczbaElementów) - 0.125f;
            float maksymalnaWysokośćSłupka = Height;

            Graphics g = e.Graphics;
            using (Pen krawędzie = new Pen(Color.Black))
                for (int i = 0; i < liczbaElementów; i++)
                {
                    float stopień = _stopniePosortowania[i] * 510;
                    int zielony = Convert.ToInt32(stopień % 255);
                    int czerwony = 255 - zielony;

                    using (Brush wypełnienie = new SolidBrush(Color.FromArgb(czerwony, zielony, 0)))
                    {
                        float x1 = i * szerokośćSłupka;
                        float wysokośćSłupka = _daneDoWykresu[i] / Convert.ToSingle(MaksymalnyElement) * maksymalnaWysokośćSłupka;
                        float y1 = maksymalnaWysokośćSłupka - wysokośćSłupka - 3;

                        g.FillRectangle(wypełnienie, x1 + 0.5f, y1 + 0.5f, szerokośćSłupka - 0.5f, wysokośćSłupka - 0.5f);
                        g.DrawRectangle(krawędzie, x1, y1, szerokośćSłupka, wysokośćSłupka);
                    }
                }
        }

        public void AktualizujDaneDoWykresu(IList<int> dane, IList<float> stopniePosortowania)
        {
            _daneDoWykresu = dane;
            _stopniePosortowania = stopniePosortowania;
        }
    }
}