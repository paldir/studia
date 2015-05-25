using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace plecak
{
    struct Przedmiot
    {
        public float p { get; set; }
        public float w { get; set; }

        public Przedmiot(float wartość, float waga)
            : this()
        {
            p = wartość;
            w = waga;
        }
    }
}