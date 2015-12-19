using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace orm.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Tytul { get; set; }

        public int IdTematu { get; set; }
        public virtual Temat Temat { get; set; }
    }
}