using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    class Measure
    {
        string name;
        string description;

        public Measure(DataAccess.Measure measure)
        {
            name = measure.GetName();
            description = measure.GetDescription();
        }
    }
}
