using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AnalysisServices.AdomdClient;

namespace DataAccess
{
    public class Dimension
    {
        string name;
        string description;
        AttributeHierarchy[] attributeHierarchies;

        public Dimension(Microsoft.AnalysisServices.AdomdClient.Dimension dimension)
        {
            name = dimension.Name;
            description = dimension.Description;

            attributeHierarchies = new AttributeHierarchy[dimension.AttributeHierarchies.Count];

            for (int i = 0; i < attributeHierarchies.Length; i++)
                attributeHierarchies[i] = new AttributeHierarchy(dimension.AttributeHierarchies[i]);
        }

        public string GetName() { return name; }
        public string GetDescription() { return description; }
        public AttributeHierarchy[] GetAttributeHierarchies() { return attributeHierarchies; }
    }
}
