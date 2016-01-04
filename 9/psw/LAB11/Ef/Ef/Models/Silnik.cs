using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ef.Models
{
    public class Silnik
    {
        public int SilnikId { get; set; }
        public string Nazwa { get; set; }
        public int Moc { get; set; }
        public virtual IEnumerable<Samochód> Samochody { get; set; }
    }
}