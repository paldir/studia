using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sortowania
{
    public delegate void AktualizacjaWizualizacji();

    public interface ZadanieZWizualizacją
    {
        void AktualizujWizualizację();
        void Uruchom();
        void Zawieś();
        bool Wznów();
    }
}