using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Czat.Models.Encje;

namespace Czat.Models.Rozmowa
{
    public class Lista
    {
        public int IdRozmowy { get; private set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime OstatniaAktywnosc { get; private set; }

        public string NazwaZnajomego { get; private set; }

        public bool NoweWiadomości { get; private set; }

        public Lista(Encje.Rozmowa rozmowa, Uzytkownik zalogowanyUzytkownik)
        {
            IdRozmowy = rozmowa.Id;
            OstatniaAktywnosc = rozmowa.OstatniaAktywnosc;
            Uzytkownik zaczynający = rozmowa.Uzytkownik0;
            Uzytkownik odpowiadający = rozmowa.Uzytkownik1;
            Uzytkownik znajomy = zaczynający == zalogowanyUzytkownik ? odpowiadający : zaczynający;
            NazwaZnajomego = znajomy.Nazwa;
            NoweWiadomości = rozmowa.Odpowiedzi.Any(o => !o.Przeczytana);
        }
    }
}