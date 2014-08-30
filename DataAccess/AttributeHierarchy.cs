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
        string displayFolder;
        Member[] members;

        public AttributeHierarchy(Microsoft.AnalysisServices.AdomdClient.Hierarchy attributeHierarchy)
        {
            name = attributeHierarchy.Name;
            uniqueName = attributeHierarchy.UniqueName;
            description = attributeHierarchy.Description;
            displayFolder = attributeHierarchy.DisplayFolder;
            Microsoft.AnalysisServices.AdomdClient.MemberCollection ASMembers = attributeHierarchy.Levels[0].GetMembers();

            members = new Member[ASMembers.Count];

            for (int i = 0; i < members.Length; i++)
                members[i] = new Member(ASMembers[i]);
        }

        public string GetName() { return name; }
        public string GetUniqueName() { return uniqueName; }
        public string GetDescription() { return description; }
        public string GetDisplayFolder() { return displayFolder; }
        public Member[] GetMembers() { return members; }
    }
}
