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

        //string name;
        public string Name { get; private set; }

        //string uniqueName;
        public string UniqueName { get; private set; }

        //string displayFolder;
        public string DisplayFolder { get; private set; }

        //HierarchyType hierarchyType;
        public HierarchyType HierarchyType { get; private set; }

        List<Member> members;
        public List<Member> GetMembers() { return members; }

        public Hierarchy(Microsoft.AnalysisServices.AdomdClient.Hierarchy hierarchy)
        {
            Name = hierarchy.Name;
            UniqueName = hierarchy.UniqueName;
            DisplayFolder = hierarchy.DisplayFolder;
            members = new List<Member>();
            Microsoft.AnalysisServices.AdomdClient.MemberCollection ASMembers = hierarchy.Levels[0].GetMembers();

            switch (hierarchy.HierarchyOrigin)
            {
                case Microsoft.AnalysisServices.AdomdClient.HierarchyOrigin.AttributeHierarchy:
                    HierarchyType = HierarchyType.AttributeHierarchy;
                    break;
                default:
                    HierarchyType = HierarchyType.Hierarchy;
                    break;
            }

            foreach (Microsoft.AnalysisServices.AdomdClient.Member member in ASMembers)
                members.Add(new Member(member));
        }
    }
}
