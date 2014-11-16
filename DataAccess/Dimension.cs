using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class Dimension
    {
        string name;
        public string Name { get { return name; } }

        List<Hierarchy> attributeHierarchies;
        public List<Hierarchy> AttributeHierarchies { get { return attributeHierarchies; } }

        List<Hierarchy> hierarchies;
        public List<Hierarchy> Hierarchies { get { return hierarchies; } }

        public Dimension(Microsoft.AnalysisServices.AdomdClient.Dimension dimension)
        {
            name = dimension.Name;
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