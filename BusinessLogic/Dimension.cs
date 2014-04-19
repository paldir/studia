using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    class Dimension
    {
        string name;
        string description;

        public Dimension(DataAccess.Dimension dimension)
        {
            name = dimension.GetName();
            description = dimension.GetDescription();
        }
    }
}
