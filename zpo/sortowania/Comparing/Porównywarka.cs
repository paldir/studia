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
                List<ZadanieZWizualizacją> zadaniaDoUsunięcia = new List<ZadanieZWizualizacją>();

                foreach (ZadanieZWizualizacją zadanie in _zadaniaZWizualizacją)
                    if (zadanie.Wznów())
                        praca = true;
                    else
                        zadaniaDoUsunięcia.Add(zadanie);

                foreach (ZadanieZWizualizacją zadanie in zadaniaDoUsunięcia)
                    _zadaniaZWizualizacją.Remove(zadanie);

                Thread.Sleep(100);

                foreach (ZadanieZWizualizacją zadanie in _zadaniaZWizualizacją)
                    zadanie.Zawieś();

                foreach (ZadanieZWizualizacją zadanie in _zadaniaZWizualizacją)
                    zadanie.AktualizujWizualizację();
            }
        }
    }
}