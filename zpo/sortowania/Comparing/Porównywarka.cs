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
        ZadanieZWizualizacją _zadanieZWizualizacją;

        public Porównywarka(ZadanieZWizualizacją zadanieZWizualizacją)
        {
            _zadanieZWizualizacją = zadanieZWizualizacją;
        }

        public void Porównaj()
        {
            _zadanieZWizualizacją.Uruchom();
            _zadanieZWizualizacją.Zawieś();

            while (true)
            {
                if (!_zadanieZWizualizacją.Wznów())
                    break;

                Thread.Sleep(1000);
                _zadanieZWizualizacją.Zawieś();
                _zadanieZWizualizacją.AktualizujWizualizację();
            }
        }
    }
}