using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SA.App_Code
{
    public class LotyAdapter
    {
        public IEnumerable<Loty> PobierzLoty()
        {
            BazaDanychEntities db = new BazaDanychEntities();

            return db.Loty;
        }

        public void UsuńLot(Loty lot)
        {
            BazaDanychEntities db = new BazaDanychEntities();

            db.Loty.Attach(lot);
            db.Loty.Remove(lot);
            db.SaveChanges();
        }

        public void AktualizujLot(Loty nowy, Loty oryginalny)
        {
            BazaDanychEntities db = new BazaDanychEntities();

            db.Loty.Attach(oryginalny);
            db.Entry<Loty>(oryginalny).CurrentValues.SetValues(nowy);
            db.SaveChanges();
        }

        public void DodajLot(Loty lot)
        {
            BazaDanychEntities db = new BazaDanychEntities();

            db.Loty.Add(lot);
            db.SaveChanges();
        }
    }
}