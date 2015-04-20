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
        AktualizacjaWizualizacji AktualizujWizualizację { get; set; }
        void Uruchom();
        void Zawieś();
        bool Wznów();
    }
}