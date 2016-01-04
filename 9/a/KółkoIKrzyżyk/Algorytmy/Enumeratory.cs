using System;

namespace Algorytmy
{
    public enum Pole { Puste, Kółko, Krzyżyk, ZwycięskieKółko, ZwycięskiKrzyżyk }

    [Flags]
    public enum WynikGry { Nierozpoczęta, Trwająca, Kółko, Krzyżyk, Remis }

    public enum KierunekZwycięskiejLinii { BrakWygranej, Poziomy, Pionowy, UkośnieOdLewejDoPrawej, UkośnieOdPrawejDoLewej }
}