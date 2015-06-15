using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sortowania
{
    public class DaneDoWykresu
    {
        public int[] ŚrednieGrupKluczy { get; private set; }
        public float[] StopniePosortowania { get; private set; }
        public TimeSpan Czas { get; set; }

        public DaneDoWykresu(int ilośćGrup)
        {
            ŚrednieGrupKluczy = new int[ilośćGrup];
            StopniePosortowania = new float[ilośćGrup];
        }
    }
}