using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gui
{
    public partial class Start : Form
    {
        public Start()
        {
            InitializeComponent();

            Properties.Settings ustawienia = Properties.Settings.Default;
            _liczbaElementów.Value = ustawienia.LiczbaElementów;
            _liczbaGrup.Value = ustawienia.LiczbaGrup;
            _losowa.Tag = PoczątkowaKolekcja.Losowa;
            _odwrotna.Tag = PoczątkowaKolekcja.OdwrotniePosortowana;
            _wstępnie.Tag = PoczątkowaKolekcja.WstępniePosortowana;
            int kolekcja = ustawienia.PoczątkowaKolekcja;

            foreach (Control kontrolka in _kolekcja.Controls)
            {
                RadioButton radio = (RadioButton)kontrolka;

                if ((PoczątkowaKolekcja)radio.Tag == (PoczątkowaKolekcja)kolekcja)
                {
                    radio.Checked = true;

                    break;
                }
            }
        }

        private void _dalej_Click(object sender, EventArgs e)
        {
            Properties.Settings ustawienia = Properties.Settings.Default;
            Form1.Ilość = ustawienia.LiczbaElementów = Convert.ToInt32(_liczbaElementów.Value);
            Form1.LiczbaGrup = ustawienia.LiczbaGrup = Convert.ToInt32(_liczbaGrup.Value);

            foreach (Control kontrolka in _kolekcja.Controls)
            {
                RadioButton radio = (RadioButton)kontrolka;

                if (radio.Checked)
                {
                    object tag = radio.Tag;
                    Form1.PoczątkowaKolekcja = (PoczątkowaKolekcja)tag;
                    ustawienia.PoczątkowaKolekcja = Convert.ToInt32(tag);

                    break;
                }
            }

            ustawienia.Save();
            new Form1().Show();
            Close();
        }

        private void _liczbaElementów_ValueChanged(object sender, EventArgs e)
        {
            _liczbaGrup.Maximum = _liczbaElementów.Value;
        }
    }
}