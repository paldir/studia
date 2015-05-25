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
            }

            bool praca = true;

            while (praca)
            {
                praca = false;
                
                foreach (ZadanieZWizualizacją zadanie in _zadaniaZWizualizacją)
                    praca = praca | zadanie.Wznów();

                Thread.Sleep(1000);

                foreach (ZadanieZWizualizacją zadanie in _zadaniaZWizualizacją)
                    zadanie.Zawieś();

                foreach (ZadanieZWizualizacją zadanie in _zadaniaZWizualizacją)
                    zadanie.AktualizujWizualizację();
            }
        }
    }
}