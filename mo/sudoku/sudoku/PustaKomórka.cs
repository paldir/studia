using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sudoku
{
    class PustaKomórka
    {
        public int I { get; private set; }
        public int J { get; private set; }
        public List<int> Możliwości { get; private set; }

        public PustaKomórka(int i, int j)
        {
            I = i;
            J = j;
            Możliwości = new List<int>(Program.Liczby);
        }
    }
}