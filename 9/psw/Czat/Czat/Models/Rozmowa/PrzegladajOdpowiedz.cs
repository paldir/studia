using System;
using System.ComponentModel.DataAnnotations;
using Czat.Models.Encje;

namespace Czat.Models.Rozmowa
{
    public class PrzegladajOdpowiedz
    {
        public string NazwaAutora { get; private set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime Data { get; private set; }

        public string Tresc { get; private set; }

        public PrzegladajOdpowiedz(Odpowiedz odpowiedz)
        {
            Data = odpowiedz.Data;
            Tresc = odpowiedz.Tresc;
            Encje.Rozmowa rozmowa = odpowiedz.Rozmowa;
            Uzytkownik autor = odpowiedz.Nadawca == false ? rozmowa.Uzytkownik0 : rozmowa.Uzytkownik1;
            NazwaAutora = autor.Nazwa;
        }
    }
}