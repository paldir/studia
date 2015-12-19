using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace razorĆwiczenia.Models
{
    public class Lokal
    {
        public int ID { get; set; }
        public string Nazwa { get; set; }
        public string Miasto { get; set; }
        public string Kraj { get; set; }
        public double Ocena { get; set; }
        public string PhotoPath { get; set; }
    }
}