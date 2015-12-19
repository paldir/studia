using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algorytmWęgierski
{
    class Wierzchołek
    {
        Wierzchołek _skojarzonyZ = null;
        public Wierzchołek SkojarzonyZ
        {
            get { return _skojarzonyZ; }

            set
            {
                if (value != null && !Sąsiedzi.Select(s => s.Wierzchołek).Contains(value))
                    throw new Exception("Wierzchołek nie może być skojarzony z wierzchołkiem niebędącym jego sąsiadem.");

                _skojarzonyZ = value;
            }
        }

        Wierzchołek _rodzic = null;
        public Wierzchołek Rodzic
        {
            get { return _rodzic; }
            set { _rodzic = value; }
        }

        bool _odwiedzony = false;
        public bool Odwiedzony
        {
            get { return _odwiedzony; }
            set { _odwiedzony = value; }
        }

        public int Nazwa { get; private set; }
        public int ZbiórWGrafieDwudzielnym { get; private set; }
        public List<Sąsiad> Sąsiedzi { get; set; }
        public int Etykieta { get; set; }

        public int? NazwaSkojarzonegoWierzchołka
        {
            get
            {
                if (SkojarzonyZ == null)
                    return null;

                return SkojarzonyZ.Nazwa;
            }
        }

        public string NazwySąsiadów
        {
            get
            {
                string result = String.Empty;

                foreach (Sąsiad sąsiad in Sąsiedzi)
                    result += String.Format("{0} ", sąsiad.Wierzchołek.Nazwa);

                return result;
            }
        }

        public Wierzchołek(int nazwa, int zbiórWGrafieDwudzielnym)
        {
            Nazwa = nazwa;
            ZbiórWGrafieDwudzielnym = zbiórWGrafieDwudzielnym;
            Sąsiedzi = new List<Sąsiad>();
        }
    }
}