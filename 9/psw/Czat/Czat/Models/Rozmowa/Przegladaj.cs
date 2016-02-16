using System.Collections.Generic;
using System.Linq;
using Czat.Models.Encje;

namespace Czat.Models.Rozmowa
{
    public class Przegladaj
    {
        public string NazwaRozmówcy { get; private set; }
        public IEnumerable<Wypowiedz> Wypowiedzi { get; private set; }
        public Odpowiedz Odpowiedz { get; set; }
        public int IdRozmowy { get; set; }

        public Przegladaj()
        {
        }

        public Przegladaj(Encje.Rozmowa rozmowa, int idZalogowanegoUzytkownika)
        {
            IdRozmowy = rozmowa.Id;
            Uzytkownik zaczynajacy = rozmowa.Uzytkownik0;
            Uzytkownik odpowiadajacy = rozmowa.Uzytkownik1;
            NazwaRozmówcy = zaczynajacy.Id == idZalogowanegoUzytkownika ? odpowiadajacy.Nazwa : zaczynajacy.Nazwa;
            Wypowiedzi = rozmowa.Odpowiedzi.Select(o => new Wypowiedz(o));
        }
    }
}