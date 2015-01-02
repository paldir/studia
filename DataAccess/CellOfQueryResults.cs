using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    /// <summary>
    /// Reprezentuje komórkę obiektu QueryResults.
    /// </summary>
    public class CellOfQueryResults
    {
        /// <summary>
        /// Zwraca użyteczną informację komórki.
        /// </summary>
        public string Result { get; private set; }

        /// <summary>
        /// Zwraca odpowiednik komórki w języku MDX.
        /// </summary>
        public string Mdx { get; private set; }

        /// <summary>
        /// Inicjalizuje nową instancję klasy CellOfQueryResults z podanymi wartościami.
        /// </summary>
        /// <param name="result">Użyteczna informacja.</param>
        /// <param name="mdx">MDX odpowiadający użytecznej informacji.</param>
        public CellOfQueryResults(string result, string mdx)
        {
            Result = result;
            Mdx = mdx;
        }
    }
}
