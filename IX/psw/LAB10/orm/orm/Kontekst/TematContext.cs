using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Entity;

namespace orm.Kontekst
{
    public class TematContext : DbContext
    {
        public DbSet<Models.Temat> Tematy { get; set; }
        public DbSet<Models.Post> Posty { get; set; }
    }
}