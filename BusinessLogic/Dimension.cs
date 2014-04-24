using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class Dimension
    {
        string name;
        string description;
        AttributeHierarchy[] attributeHierarchies;

        public Dimension(DataAccess.Dimension dimension)
        {
            name = dimension.GetName();
            description = dimension.GetDescription();
            DataAccess.AttributeHierarchy[] dAAttributeHierarchies = dimension.GetAttributeHierarchies();

            attributeHierarchies = new AttributeHierarchy[dAAttributeHierarchies.Length];

            for (int i = 0; i < attributeHierarchies.Length; i++)
                attributeHierarchies[i] = new AttributeHierarchy(dAAttributeHierarchies[i]);
        }

        public string GetName() { return name; }
        public string GetDescription() { return description; }
        public AttributeHierarchy[] GetAttributeHierarchies() { return attributeHierarchies; }
    }
}
