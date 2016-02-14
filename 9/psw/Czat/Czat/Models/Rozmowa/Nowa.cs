using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using Czat.Models.Encje;

namespace Czat.Models.Rozmowa
{
    public class Nowa
    {
        public SelectListItem[] DaneUzytkownikow { get; private set; }

        [DisplayName("Wybór użytkownika")]
        public int IdWybranegoUzytkownika { get; set; }

        public Nowa()
        {
        }

        public Nowa(IEnumerable<Uzytkownik> uzytkownicy)
        {
            DaneUzytkownikow = uzytkownicy.Select(u => new SelectListItem() {Text = u.Nazwa, Value = u.Id.ToString()}).ToArray();
        }
    }
}