using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Czat.Models.Encje
{
    [Table("Odpowiedz")]
    public class Odpowiedz
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(160)]
        public string Tresc { get; set; }

        public int IdRozmowy { get; set; }

        public bool Nadawca { get; set; }

        public DateTime Data { get; set; }

        public virtual Rozmowa Rozmowa { get; set; }
    }
}