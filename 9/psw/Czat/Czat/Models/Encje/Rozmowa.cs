using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Czat.Models.Encje
{
    [Table("Rozmowa")]
    public class Rozmowa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int IdUzytkownika1 { get; set; }

        public int IdUzytkownika2 { get; set; }

        public DateTime OstatniaAktywnosc { get; set; }

        public virtual Uzytkownik Uzytkownik1 { get; set; }

        public virtual Uzytkownik Uzytkownik2 { get; set; }
    }
}