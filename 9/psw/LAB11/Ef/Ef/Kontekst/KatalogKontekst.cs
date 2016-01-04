using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Entity;

namespace Ef.Kontekst
{
    public class KatalogKontekst : DbContext
    {
        public DbSet<Models.Marka> Marki { get; set; }
        public DbSet<Models.Silnik> Silniki { get; set; }
        public DbSet<Models.Samochód> Samochody { get; set; }
    }
}