using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorytmy
{
    public enum Pole { Puste, Kółko, Krzyżyk }
    public enum WynikGry { Nierozpoczęta, Trwająca, Kółko, Krzyżyk, Remis }
    public enum KierunekZwycięskiejLinii { BrakWygranej, Poziomy, Pionowy, UkośnieOdLewejDoPrawej, UkośnieOdPrawejDoLewej }
}