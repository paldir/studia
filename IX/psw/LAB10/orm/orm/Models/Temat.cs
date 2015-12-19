using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace orm.Models
{
    public class Temat
    {
        public int Id { get; set; }
        public string Nazwa { get; set; }

        public virtual List<Post> Posty { get; set; }
    }
}