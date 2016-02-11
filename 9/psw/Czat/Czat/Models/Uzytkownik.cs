using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace Czat.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Uzytkownik")]
    public class Uzytkownik
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        [DisplayName("Nazwa u�ytkownika")]
        public string Nazwa { get; set; }

        [Required]
        [StringLength(15)]
        [DisplayName("Has�o")]
        public string Haslo { get; set; }

        [NotMapped]
        [DisplayName("Powt�rz has�o")]
        [Compare("Haslo", ErrorMessage = "Has�a nie s� identyczne.")]
        public string PowtorzoneHaslo { get; set; }

        public ICollection<Rozmowa> Rozmowy1 { get; set; }

        public ICollection<Rozmowa> Rozmowy2 { get; set; }

        public Uzytkownik()
        {
            Rozmowy1 = new HashSet<Rozmowa>();
            Rozmowy2 = new HashSet<Rozmowa>();
        }
    }
}