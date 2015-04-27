using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komiwojażer
{
    struct PołożenieGeograficzne
    {
        public double Długość;
        public double Szerokość;

        public PołożenieGeograficzne(double szerokość, double długość)
        {
            Długość = długość;
            Szerokość = szerokość;
        }
    }
}