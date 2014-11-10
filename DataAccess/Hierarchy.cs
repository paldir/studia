using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public enum HierarchyType { AttributeHierarchy, Hierarchy };

    public class Hierarchy
    {

        string name;
        public string Name { get { return name; } }

        string uniqueName;
        public string UniqueName { get { return uniqueName; } }

        string displayFolder;
        public string DisplayFolder { get { return displayFolder; } }

        HierarchyType hierarchyType;
        public HierarchyType HierarchyType { get { return hierarchyType; } }

        List<Member> members;

        public Hierarchy(Microsoft.AnalysisServices.AdomdClient.Hierarchy hierarchy)
        {
            name = hierarchy.Name;
            uniqueName = hierarchy.UniqueName;
            displayFolder = hierarchy.DisplayFolder;
            members = new List<Member>();
            Microsoft.AnalysisServices.AdomdClient.MemberCollection ASMembers = hierarchy.Levels[0].GetMembers();

            switch (hierarchy.HierarchyOrigin)
            {
                case Microsoft.AnalysisServices.AdomdClient.HierarchyOrigin.AttributeHierarchy:
                    hierarchyType = HierarchyType.AttributeHierarchy;
                    break;
                default:
                    hierarchyType = HierarchyType.Hierarchy;
                    break;
            }

            foreach (Microsoft.AnalysisServices.AdomdClient.Member member in ASMembers)
                members.Add(new Member(member));
        }

        public List<Member> GetMembers() { return members; }
    }
}
