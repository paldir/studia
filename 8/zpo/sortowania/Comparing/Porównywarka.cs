using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;

namespace sortowania
{
    public class Porównywarka
    {
        IList<ZadanieZWizualizacją> _zadaniaZWizualizacją;

        int _odstęp = 100;
        public int OdstępPomiędzyWizualizacją
        {
            get { return _odstęp; }
            set { _odstęp = value; }
        }

        public Porównywarka(IList<ZadanieZWizualizacją> zadaniaZWizualizacją)
        {
            _zadaniaZWizualizacją = zadaniaZWizualizacją;
        }

        public void Porównaj()
        {
            foreach (ZadanieZWizualizacją zadanie in _zadaniaZWizualizacją)
            {
                zadanie.Uruchom();
                zadanie.Zawieś();
                zadanie.AktualizujWizualizację();
            }

            bool praca = true;

            while (praca)
            {
                praca = false;

                foreach (ZadanieZWizualizacją zadanie in _zadaniaZWizualizacją)
                    zadanie.Wznów();

                Thread.Sleep(OdstępPomiędzyWizualizacją);

                foreach (ZadanieZWizualizacją zadanie in _zadaniaZWizualizacją)
                    if (zadanie.Zawieś())
                        praca = true;

                foreach (ZadanieZWizualizacją zadanie in _zadaniaZWizualizacją)
                    zadanie.AktualizujWizualizację();

            }

            foreach (ZadanieZWizualizacją zadanie in _zadaniaZWizualizacją)
                zadanie.AktualizujWizualizację();
        }
    }
}