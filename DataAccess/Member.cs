using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    /// <summary>
    /// Obiekt reprezentujący element hierarchii będącej częścią wymiaru.
    /// </summary>
    public class Member
    {
        /// <summary>
        /// Zwraca nazwę elementu hierarchii.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Zwraca nazwę elementu hierarchii w języku MDX.
        /// </summary>
        public string UniqueName { get; private set; }

        List<Member> children;
        /// <summary>
        /// Zwraca elementy potomne elementu hierarchii.
        /// </summary>
        /// <returns></returns>
        public List<Member> GetChildren() { return children; }

        /// <summary>
        /// Inicjalizuje instancję klasy przy pomocy obiektu reprezentującego element hierarchii. Z powodu długiego czasu przetwarzania, elementy potomne reprezentowanego elementu są inicjalizowane tylko wtedy, gdy jest ich mniej niż 20.
        /// </summary>
        /// <param name="member">Obiekt reprezentujący element hierarchii, pochodzący z ADOMD.NET.</param>
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