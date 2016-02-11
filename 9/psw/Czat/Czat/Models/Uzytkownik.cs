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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Rozmowa> Rozmowa1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Rozmowa> Rozmowa2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Uzytkownik()
        {
            Rozmowa1 = new HashSet<Rozmowa>();
            Rozmowa2 = new HashSet<Rozmowa>();
        }
    }
}