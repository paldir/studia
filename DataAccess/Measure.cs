using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    /// <summary>
    /// Obiekt reprezentujący miarę kostki.
    /// </summary>
    public class Measure
    {
        /// <summary>
        /// Zwraca przyjazną nazwę miary.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Zwraca nazwę miary w języku MDX.
        /// </summary>
        public string UniqueName { get; private set; }

        /// <summary>
        /// Zwraca nazwę grupy miar, do której należy miara.
        /// </summary>
        public string MeasureGroup { get; private set; }

        /// <summary>
        /// Inicjalizuje instancję klasy za pomocą obiektu reprezentującego miarę.
        /// </summary>
        /// <param name="measure">Obiekt reprezentujący miarę, pochodzący z ADOMD.NET.</param>
        public Measure(Microsoft.AnalysisServices.AdomdClient.Measure measure)
        {
            Name = measure.Name;
            UniqueName = measure.UniqueName;
            MeasureGroup = measure.Properties["MEASUREGROUP_NAME"].Value.ToString();
        }
    }
}