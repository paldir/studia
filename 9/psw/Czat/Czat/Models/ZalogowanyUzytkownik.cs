using System;
using System.Security.Principal;

namespace Czat.Models
{
    public class ZalogowanyUzytkownik : IPrincipal
    {
        public IIdentity Identity { get; private set; }
        public int Id { get; private set; }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }

        public ZalogowanyUzytkownik(int id, string nazwa)
        {
            Id = id;
            Identity = new GenericIdentity(nazwa);
        }
    }
}