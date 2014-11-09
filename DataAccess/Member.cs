using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class Member
    {
        string name;
        public string Name { get { return name; } }

        string uniqueName;
        public string UniqueName { get { return uniqueName; } }

        List<Member> children;

        public Member(Microsoft.AnalysisServices.AdomdClient.Member member)
        {
            name = member.Name;
            uniqueName = member.UniqueName;
            children = new List<Member>();
            Microsoft.AnalysisServices.AdomdClient.MemberCollection aSChildren = member.GetChildren();

            if (aSChildren.Count < 20)
                foreach (Microsoft.AnalysisServices.AdomdClient.Member child in aSChildren)
                    children.Add(new Member(child));
        }

        public List<Member> GetChildren() { return children; }
    }
}