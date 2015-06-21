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

namespace Hopfield
{
    public partial class Form1 : Form
    {
        const int _liczbaNeuronów = 5;
        const int _bokPrzycisku = 25;
        const int _N = _liczbaNeuronów * _liczbaNeuronów;
        readonly Color _aktywnyKolor = Color.Black;
        readonly Color _nieaktywnyKolor = Color.White;
        double[,] _w;
        List<Button> _neurony;
        Random _los;
        List<int[]> _wektoryUczące;
        Thread _wątek;

        public Form1()
        {
            InitializeComponent();

            _w = new double[_N, _N];
            _neurony = new List<Button>();
            _los = new Random();
            _wektoryUczące = new List<int[]>();

            for (int i = 0; i < _liczbaNeuronów; i++)
                for (int j = 0; j < _liczbaNeuronów; j++)
                {
                    Button neuron = new Button();
                    neuron.Text = String.Empty;
                    neuron.BackColor = _nieaktywnyKolor;
                    neuron.Width = neuron.Height = _bokPrzycisku;
                    neuron.Location = new Point(i * _bokPrzycisku, j * _bokPrzycisku);
                    neuron.Click += neuron_Click;

                    _neurony.Add(neuron);
                    Controls.Add(neuron);
                }

            MessageBox.Show("Proszę dodawać kolejne wzorce. Po dodaniu wszystkich wzorców proszę nacisnąć przycisk 'Zakończ naukę'.");
        }

        void Symulacja()
        {
            while (true)
            {
                int i = _los.Next(_N);
                double wartość = 0;
                Button neuron = _neurony[i];
                Color oryginalnyKolor = neuron.BackColor;

                for (int j = 0; j < _N; j++)
                    wartość += _w[i, j] * (_neurony[j].BackColor == _aktywnyKolor ? 1 : -1);

                if (wartość >= 0)
                    neuron.BackColor = _aktywnyKolor;
                else
                    neuron.BackColor = _nieaktywnyKolor;

                if (oryginalnyKolor != neuron.BackColor)
                    Thread.Sleep(500);
            }
        }

        private void _dodawanieWektoraUczącego_Click(object sender, EventArgs e)
        {
            int[] wektorUczący = new int[_N];

            for (int i = 0; i < _N; i++)
            {
                Button neuron = _neurony[i];
                wektorUczący[i] = neuron.BackColor == _aktywnyKolor ? 1 : -1;
                neuron.BackColor = _nieaktywnyKolor;
            }

            _wektoryUczące.Add(wektorUczący);

            _liczbaWzorców.Text = _wektoryUczące.Count.ToString();
        }

        void neuron_Click(object sender, EventArgs e)
        {
            Button neuron = sender as Button;

            if (neuron.BackColor == _nieaktywnyKolor)
                neuron.BackColor = _aktywnyKolor;
            else
                neuron.BackColor = _nieaktywnyKolor;
        }

        private void _usuwanieWzorców_Click(object sender, EventArgs e)
        {
            _wektoryUczące.Clear();

            _liczbaWzorców.Text = "0";
        }

        private void _koniecUczenia_Click(object sender, EventArgs e)
        {
            _dodawanieWektoraUczącego.Enabled = _usuwanieWzorców.Enabled = _koniecUczenia.Enabled = false;
            _symulacja.Enabled = true;

            for (int i = 0; i < _N; i++)
                for (int j = 0; j < _N; j++)
                {
                    double suma = 0;

                    foreach (int[] wektor in _wektoryUczące)
                        suma += wektor[i] * wektor[j];

                    suma *= 1.0 / _N;
                    _w[i, j] = suma;
                }

            for (int i = 0; i < _N; i++)
            {
                _w[i, i] = 0;
                _neurony[i].BackColor = _nieaktywnyKolor;
            }

            MessageBox.Show("Proszę wprowadzić wektor testowy i nacisnąć 'Start'.");
        }

        private void _symulacja_Click(object sender, EventArgs e)
        {
            _symulacja.Enabled = false;
            _nowyWektorTestowy.Enabled = true;
            _wątek = new Thread(Symulacja);

            _wątek.Start();
        }

        private void _nowyWektorTestowy_Click(object sender, EventArgs e)
        {
            if (_wątek != null)
                _wątek.Abort();

            _nowyWektorTestowy.Enabled = false;
            _symulacja.Enabled = true;

            foreach (Button neuron in _neurony)
                neuron.BackColor = _nieaktywnyKolor;
        }

        private void _anulujWszystko_Click(object sender, EventArgs e)
        {
            _usuwanieWzorców_Click(null, null);
            _nowyWektorTestowy_Click(null, null);

            _dodawanieWektoraUczącego.Enabled = _usuwanieWzorców.Enabled = _koniecUczenia.Enabled = true;
            _symulacja.Enabled = false;
        }
    }
}