using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    /// <summary>
    /// Reprezentuje wymiar kostki.
    /// </summary>
    public class Dimension
    {
        /// <summary>
        /// Zwraca przyjazną nazwę wymiaru.
        /// </summary>
        public string Name { get; private set; }

        List<Hierarchy> attributeHierarchies;
        /// <summary>
        /// Zwraca hierarchie atrybutów.
        /// </summary>
        /// <returns>Lista obiektów reprezentujących hierarchie atrybutów.</returns>
        public List<Hierarchy> GetAttributeHierarchies() { return new List<Hierarchy>(attributeHierarchies); }

        List<Hierarchy> hierarchies;
        /// <summary>
        /// Zwraca hierarchie użytkownika.
        /// </summary>
        /// <returns>Lista obiektów reprezentujących hierarchie użytkownika.</returns>
        public List<Hierarchy> GetHierarchies() { return new List<Hierarchy>(hierarchies); }

        /// <summary>
        /// Inicjalizuje instancję klasy przy pomocy obiektu reprezentującego wymiar.
        /// </summary>
        /// <param name="dimension">Obiekt reprezentujący wymiar, pochodzący z ADOMD.NET.</param>
        public Dimension(Microsoft.AnalysisServices.AdomdClient.Dimension dimension)
        {
            Name = dimension.Name;
            attributeHierarchies = new List<Hierarchy>();
            hierarchies = new List<Hierarchy>();

            foreach (Microsoft.AnalysisServices.AdomdClient.Hierarchy hierarchy in dimension.Hierarchies)
            {
                Hierarchy newHierarchy = new Hierarchy(hierarchy);

                switch (hierarchy.HierarchyOrigin)
                {
                    case Microsoft.AnalysisServices.AdomdClient.HierarchyOrigin.AttributeHierarchy:
                        attributeHierarchies.Add(newHierarchy);
                        break;
                    default:
                        hierarchies.Add(newHierarchy);
                        break;
                }
            }
        }
    }
}