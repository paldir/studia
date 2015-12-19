using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace maksymalneSkojarzenie
{
    public class Wierzchołek
    {
        Wierzchołek _skojarzonyZ = null;
        public Wierzchołek SkojarzonyZ
        {
            get { return _skojarzonyZ; }

            set
            {
                if (value != null && !Sąsiedzi.Contains(value))
                    throw new Exception("Wierzchołek nie może być skojarzony z wierzchołkiem niebędącym jego sąsiadem.");

                _skojarzonyZ = value;
            }
        }

        bool _odwiedzony = false;
        public bool Odwiedzony
        {
            get { return _odwiedzony; }
            set { _odwiedzony = value; }
        }

        Wierzchołek _rodzic = null;
        public Wierzchołek Rodzic
        {
            get { return _rodzic; }
            set { _rodzic = value; }
        }

        public int Nazwa { get; private set; }
        public int ZbiórWGrafieDwudzielnym { get; private set; }
        public List<Wierzchołek> Sąsiedzi { get; set; }

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

                foreach (Wierzchołek wierzchołek in Sąsiedzi)
                    result += String.Format("{0} ", wierzchołek.Nazwa);

                return result;
            }
        }

        public Wierzchołek(int nazwa, int zbiórWGrafieDwudzielnym)
        {
            Nazwa = nazwa;
            ZbiórWGrafieDwudzielnym = zbiórWGrafieDwudzielnym;
            Sąsiedzi = new List<Wierzchołek>();
        }
    }
}