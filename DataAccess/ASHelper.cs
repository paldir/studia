using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AnalysisServices.AdomdClient;

namespace DataAccess
{
    class ASHelper
    {
        const string connectionString = "Data Source=localhost;";

        public static AdomdConnection EstablishConnection() { return new AdomdConnection(connectionString); }
    }
}
