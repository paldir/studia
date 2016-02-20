using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Czat.Models.Encje;

namespace Czat.Models.Rozmowa
{
    public class Lista
    {
        public int IdRozmowy { get; private set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm:ss}")]
        public DateTime OstatniaAktywnosc { get; private set; }

        public string NazwaZnajomego { get; private set; }

        public bool NoweWiadomości { get; private set; }

        public int LiczbaNowychWiadomości { get; private set; }

        public string Skrot { get; private set; }

        public Lista(Encje.Rozmowa rozmowa, Uzytkownik zalogowanyUzytkownik)
        {
            IdRozmowy = rozmowa.Id;
            OstatniaAktywnosc = rozmowa.OstatniaAktywnosc;
            Uzytkownik zaczynający = rozmowa.Uzytkownik0;
            Uzytkownik odpowiadający = rozmowa.Uzytkownik1;
            Uzytkownik znajomy = zaczynający == zalogowanyUzytkownik ? odpowiadający : zaczynający;
            NazwaZnajomego = znajomy.Nazwa;
            ICollection<Odpowiedz> odpowiedzi = rozmowa.Odpowiedzi;
            LiczbaNowychWiadomości = odpowiedzi.Count(o => (o.Autor == znajomy) && !o.Przeczytana);
            string ostatniaWiadomość = odpowiedzi.Last().Tresc;
            const int długośćSkrótu = 20;
            Skrot = ostatniaWiadomość.Length > długośćSkrótu ? string.Concat(ostatniaWiadomość.Substring(0, długośćSkrótu), "...") : ostatniaWiadomość;

            if (LiczbaNowychWiadomości > 0)
                NoweWiadomości = true;
        }
    }
}