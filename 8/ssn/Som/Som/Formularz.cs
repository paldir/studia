using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Threading;

namespace Som
{
    public partial class Formularz : Form
    {
        public Formularz()
        {
            InitializeComponent();

            suwak_Scroll(null, null);
        }

        private void suwak_Scroll(object sender, EventArgs e)
        {
            mapa.SzybkośćAnimacji = suwak.Maximum - suwak.Value + 1;
        }

        private void przyciskStartu_Click(object sender, EventArgs e)
        {
            mapa.LiczbaNeuronów = Convert.ToInt32(ilość.Text);
            ilość.Enabled = mapa.Stop = przyciskStartu.Enabled = false;
            przyciskStopu.Enabled = true;

            new Thread(mapa.Animacja).Start();
        }

        private void przyciskStopu_Click(object sender, EventArgs e)
        {
            ilość.Enabled = mapa.Stop = przyciskStartu.Enabled = true;
            przyciskStopu.Enabled = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            mapa.Stop = true;
        }
    }
}