using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Czat.Models.Encje
{
    [Table("Uzytkownik")]
    public class Uzytkownik
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        [DisplayName("Nazwa u¿ytkownika")]
        public string Nazwa { get; set; }

        [Required]
        [StringLength(15)]
        [DisplayName("Has³o")]
        public string Haslo { get; set; }

        [NotMapped]
        [DisplayName("Powtórz has³o")]
        [Compare("Haslo", ErrorMessage = "Has³a nie s¹ identyczne.")]
        public string PowtorzoneHaslo { get; set; }

        public virtual ICollection<Rozmowa> Rozmowy1 { get; set; }

        public virtual ICollection<Rozmowa> Rozmowy2 { get; set; }
    }
}