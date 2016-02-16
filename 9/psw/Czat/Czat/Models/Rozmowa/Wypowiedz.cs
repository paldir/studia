using System;
using System.ComponentModel.DataAnnotations;
using Czat.Models.Encje;

namespace Czat.Models.Rozmowa
{
    public class Wypowiedz
    {
        public string NazwaAutora { get; private set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime Data { get; private set; }

        public string Tresc { get; private set; }

        public Wypowiedz(Odpowiedz odpowiedz)
        {
            Data = odpowiedz.Data;
            Tresc = odpowiedz.Tresc;
            NazwaAutora = odpowiedz.Autor.Nazwa;
        }
    }
}