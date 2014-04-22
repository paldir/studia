using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class Member
    {
        string name;
        string description;
        Member[] children;

        public Member(DataAccess.Member member)
        {
            name = member.GetName();
            description = member.GetDescription();
            DataAccess.Member[] dAChildren = member.GetChildren();

            children = new Member[dAChildren.Length];

            for (int i = 0; i < children.Length; i++)
                children[i] = new Member(dAChildren[i]);
        }

        public string GetName() { return name; }
        public string GetDescription() { return description; }
        public Member[] GetChildren() { return children; }
    }
}
