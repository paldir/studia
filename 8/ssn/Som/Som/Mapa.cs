﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Threading;

namespace Som
{
    public partial class Mapa : UserControl
    {
        PointF[] _neurony;
        Random _los;
        PointF[] _punktyOgraniczające;
        Prosta[] _prosteOgraniczające;
        PointF _punktPrzyciągający;
        float _promień;

        public int SzybkośćAnimacji { get; set; }
        public int LiczbaNeuronów { get; set; }
        public bool Stop { get; set; }

        public Mapa()
        {
            InitializeComponent();

            _los = new Random();
            _punktyOgraniczające = new PointF[4];
            _prosteOgraniczające = new Prosta[3];
            _punktyOgraniczające[0] = _punktyOgraniczające[3] = new PointF(ClientRectangle.Width / 2, 0);
            _punktyOgraniczające[1] = new PointF(0, ClientRectangle.Height);
            _punktyOgraniczające[2] = new PointF(ClientSize.Width, ClientRectangle.Height);

            for (int i = 0; i < 3; i++)
            {
                PointF punkt1 = _punktyOgraniczające[i];
                PointF punkt2 = _punktyOgraniczające[i + 1];

                float a = (punkt2.Y - punkt1.Y) / (punkt2.X - punkt1.X);
                float b = punkt1.Y - a * punkt1.X;
                _prosteOgraniczające[i] = new Prosta(a, b);
            }
        }

        public void Animacja()
        {
            _neurony = new PointF[LiczbaNeuronów];
            float początkowyPromień = 100;
            _promień = początkowyPromień;
            float zmniejszaniePromienia = 1.0f / LiczbaNeuronów;

            for (int i = 0; i < LiczbaNeuronów; i++)
                _neurony[i] = new PointF(LosowaSzerokość(), LosowaWysokość());

            while (!Stop && _promień > 0)
            {
                bool punktWPrzestrzeni;
                PointF losowyPunkt;

                do
                {
                    losowyPunkt = new PointF(LosowaSzerokość(), LosowaWysokość());
                    float x = losowyPunkt.X;
                    float y = losowyPunkt.Y;
                    punktWPrzestrzeni = _prosteOgraniczające[0].A * x + _prosteOgraniczające[0].B < y && _prosteOgraniczające[1].A * x + _prosteOgraniczające[1].B > y && _prosteOgraniczające[2].A * x + _prosteOgraniczające[2].B < y;

                }
                while (!punktWPrzestrzeni);

                _punktPrzyciągający = losowyPunkt;

                Odśwież();

                PointF zwycięzca;
                float odległośćZwycięzcy = Single.MaxValue;
                int indeksZwycięzcy = -1;

                for (int i = 0; i < _neurony.Length; i++)
                {
                    PointF neuron = _neurony[i];
                    float odległość = DługośćWektora(neuron, _punktPrzyciągający);

                    if (odległość < odległośćZwycięzcy)
                    {
                        odległośćZwycięzcy = odległość;
                        zwycięzca = neuron;
                        indeksZwycięzcy = i;
                    }
                }

                _promień -= zmniejszaniePromienia;
                float siłaPrzyciągania = _promień / początkowyPromień;

                {
                    int liczbaPrzesuwanychNeuronów = Convert.ToInt32(Math.Floor(siłaPrzyciągania * LiczbaNeuronów * 0.25));
                    siłaPrzyciągania *= 0.5f;
                    _neurony[indeksZwycięzcy].X += siłaPrzyciągania * (_punktPrzyciągający.X - _neurony[indeksZwycięzcy].X);
                    _neurony[indeksZwycięzcy].Y += siłaPrzyciągania * (_punktPrzyciągający.Y - _neurony[indeksZwycięzcy].Y);

                    Odśwież();
                    PrzyciągnijLeweNeurony(indeksZwycięzcy, liczbaPrzesuwanychNeuronów, siłaPrzyciągania);
                    PrzyciągnijPraweNeurony(indeksZwycięzcy, liczbaPrzesuwanychNeuronów, siłaPrzyciągania);
                }
            }

            Stop = true;
        }

        void PrzyciągnijLeweNeurony(int indeksZwycięzcy, int ilośćNeuronów, float siłaPrzyciągania)
        {
            int indeks = indeksZwycięzcy - 1;

            for (int i = 0; i < ilośćNeuronów && indeks >= 0; i++)
            {
                _neurony[indeks].X += siłaPrzyciągania * (_punktPrzyciągający.X - _neurony[indeks].X);
                _neurony[indeks].Y += siłaPrzyciągania * (_punktPrzyciągający.Y - _neurony[indeks].Y);
                indeks--;
            }
        }

        void PrzyciągnijPraweNeurony(int indeksZwycięzcy, int ilośćNeuronów, float siłaPrzyciągania)
        {
            int indeks = indeksZwycięzcy + 1;

            for (int i = 0; i < ilośćNeuronów && indeks < _neurony.Length; i++)
            {
                _neurony[indeks].X += siłaPrzyciągania * (_punktPrzyciągający.X - _neurony[indeks].X);
                _neurony[indeks].Y += siłaPrzyciągania * (_punktPrzyciągający.Y - _neurony[indeks].Y);
                indeks++;
            }
        }

        void Odśwież()
        {
            Invalidate();
            Thread.Sleep(SzybkośćAnimacji);
        }

        float LosowaSzerokość(float zakres = 1)
        {
            return (Convert.ToSingle(_los.NextDouble()) * zakres + (1 - zakres)) * ClientSize.Width;
        }

        float LosowaWysokość(float zakres = 1)
        {
            return (Convert.ToSingle(_los.NextDouble()) * zakres + (1 - zakres)) * ClientSize.Height;
        }

        float DługośćWektora(PointF punkt1, PointF punkt2)
        {
            return Convert.ToSingle(Math.Sqrt(Math.Pow(punkt2.X - punkt1.X, 2) + Math.Pow(punkt2.Y - punkt1.Y, 2)));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            using (Pen ograniczenia = new Pen(Color.LightGray))
                g.DrawLines(ograniczenia, _punktyOgraniczające);

            if (_neurony != null)
            {
                using (Pen połączenia = new Pen(Color.Blue))
                    g.DrawLines(połączenia, _neurony);

                using (SolidBrush neurony = new SolidBrush(Color.Blue))
                    foreach (PointF neuron in _neurony)
                        NarysujPunkt(g, neurony, neuron);
            }

            using (SolidBrush punkt = new SolidBrush(Color.Red))
                NarysujPunkt(g, punkt, _punktPrzyciągający);

            using (Pen promień = new Pen(Color.Red))
            {
                float bok = _promień * 2;

                g.DrawEllipse(promień, 475 - _promień, 100 - _promień, bok, bok);
            }
        }

        void NarysujPunkt(Graphics g, SolidBrush pędzel, PointF punkt)
        {
            const float promieńPunktu = 5;
            float x = punkt.X - promieńPunktu / 2;
            float y = punkt.Y - promieńPunktu / 2;

            g.FillEllipse(pędzel, x, y, promieńPunktu, promieńPunktu);
        }
    }
}