using System.Data.Entity;

namespace Czat.Models.Encje
{
    public class Model : DbContext
    {
        public Model()
            : base("name=Model")
        {
        }

        public virtual DbSet<Uzytkownik> Uzytkownicy { get; set; }
        public virtual DbSet<Rozmowa> Rozmowy { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<Model>(null);

            modelBuilder.Entity<Uzytkownik>()
                .HasMany(e => e.Rozmowy1)
                .WithRequired(e => e.Uzytkownik1)
                .HasForeignKey(e => e.IdUzytkownika1)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Uzytkownik>()
                .HasMany(e => e.Rozmowy2)
                .WithRequired(e => e.Uzytkownik2)
                .HasForeignKey(e => e.IdUzytkownika2)
                .WillCascadeOnDelete(false);
        }
    }
}