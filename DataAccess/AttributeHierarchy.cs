using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class AttributeHierarchy
    {
        string name;
        string uniqueName;
        string description;
        Member[] members;

        public AttributeHierarchy(Microsoft.AnalysisServices.AdomdClient.Hierarchy attributeHierarchy)
        {
            name = attributeHierarchy.Name;
            uniqueName = attributeHierarchy.UniqueName;
            description = attributeHierarchy.Description;
            Microsoft.AnalysisServices.AdomdClient.MemberCollection ASMembers = attributeHierarchy.Levels[0].GetMembers();

            members = new Member[ASMembers.Count];

            for (int i = 0; i < members.Length; i++)
                members[i] = new Member(ASMembers[i]);
        }

        public string GetName() { return name; }
        public string GetUniqueName() { return uniqueName; }
        public string GetDescription() { return description; }
        public Member[] GetMembers() { return members; }
    }
}
