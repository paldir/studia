using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    internal class CellOfQueryResults
    {
        public string Result { get; private set; }
        public string Mdx { get; private set; }

        public CellOfQueryResults(string result, string mdx)
        {
            Result = result;
            Mdx = mdx;
        }
    }
}
