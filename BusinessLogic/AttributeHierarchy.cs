using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class AttributeHierarchy
    {
        string name;
        string description;
        Member[] members;

        public AttributeHierarchy(DataAccess.AttributeHierarchy attributeHierarchy)
        {
            name = attributeHierarchy.GetName();
            description = attributeHierarchy.GetDescription();
            DataAccess.Member[] dAMembers = attributeHierarchy.GetMembers();

            members = new Member[dAMembers.Length];

            for (int i = 0; i < members.Length; i++)
                members[i] = new Member(dAMembers[i]);
        }

        public string GetName() { return name; }
        public string GetDescription() { return description; }
        public Member[] GetMembers() { return members; }
    }
}
