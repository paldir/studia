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
        public virtual DbSet<Odpowiedz> Odpowiedzi { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<Model>(null);

            modelBuilder.Entity<Uzytkownik>()
                .HasMany(e => e.Rozmowy0)
                .WithRequired(e => e.Uzytkownik0)
                .HasForeignKey(e => e.IdUzytkownika0)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Uzytkownik>()
                .HasMany(e => e.Rozmowy1)
                .WithRequired(e => e.Uzytkownik1)
                .HasForeignKey(e => e.IdUzytkownika1)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Rozmowa>()
                .HasMany(e => e.Odpowiedzi)
                .WithRequired(e => e.Rozmowa)
                .HasForeignKey(e => e.IdRozmowy)
                .WillCascadeOnDelete(false);
        }
    }
}