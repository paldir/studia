using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ef.Models
{
    public class Marka
    {
        public int MarkaId { get; set; }
        public string Nazwa { get; set; }
        public int RokZałożenia { get; set; }
        public virtual IEnumerable<Samochód> Samochody { get; set; }
    }
}