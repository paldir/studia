using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    /// <summary>
    /// Obiekt reprezentujący członka hierarchii będącej częścią wymiaru.
    /// </summary>
    public class Member
    {
        /// <summary>
        /// Zwraca przyjazną nazwę członka hierarchii.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Zwraca nazwę członka hierarchii w języku MDX.
        /// </summary>
        public string UniqueName { get; private set; }

        List<Member> children;
        /// <summary>
        /// Zwraca członków potomnych członka hierarchii.
        /// </summary>
        /// <returns></returns>
        public List<Member> GetChildren() { return children; }

        /// <summary>
        /// Inicjalizuje instancję klasy przy pomocy obiektu reprezentującego członka hierarchii. Z powodu długiego czasu przetwarzania, członkowie potomni reprezentowanego członka są inicjalizowani tylko wtedy, gdy jest ich mniej niż 20.
        /// </summary>
        /// <param name="member">Obiekt reprezentujący członka hierarchii, pochodzący z ADOMD.NET.</param>
        public Member(Microsoft.AnalysisServices.AdomdClient.Member member)
        {
            Name = member.Name;
            UniqueName = member.UniqueName;
            children = new List<Member>();
            Microsoft.AnalysisServices.AdomdClient.MemberCollection aSChildren = member.GetChildren();

            if (aSChildren.Count < 20)
                foreach (Microsoft.AnalysisServices.AdomdClient.Member child in aSChildren)
                    children.Add(new Member(child));
        }
    }
}