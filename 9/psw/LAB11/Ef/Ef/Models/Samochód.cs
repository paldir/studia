using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ef.Models
{
    public class Samochód
    {
        public int SamochódId { get; set; }
        public string Nazwa { get; set; }
        public int LiczbaDrzwi { get; set; }

        public int SilnikId { get; set; }
        public virtual Silnik Silnik { get; set; }

        public int MarkaId { get; set; }
        public virtual Marka Marka { get; set; }
    }
}