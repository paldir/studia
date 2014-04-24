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
        string description;
        Member[] children;

        public Member(Microsoft.AnalysisServices.AdomdClient.Member member)
        {
            name = member.Name;
            description = member.Description;
            Microsoft.AnalysisServices.AdomdClient.MemberCollection aSChildren = member.GetChildren();

            if (aSChildren.Count < 100)
                children = new Member[aSChildren.Count];
            else
                children = new Member[0];

            for (int i = 0; i < children.Length; i++)
                children[i] = new Member(aSChildren[i]);
        }

        public string GetName() { return name; }
        public string GetDescription() { return description; }
        public Member[] GetChildren() { return children; }
    }
}
