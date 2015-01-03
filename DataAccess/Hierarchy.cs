using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    /// <summary>
    /// Wyliczenie reprezentujące rodzaje hierarchii.
    /// </summary>
    public enum HierarchyType { AttributeHierarchy, Hierarchy };
    
    /// <summary>
    /// Obiekt reprezentujący hierarchię.
    /// </summary>
    public class Hierarchy
    {        
        /// <summary>
        /// Zwraca przyjazną nazwę hierarchii.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Zwraca nazwę hierarchii w języku MDX.
        /// </summary>
        public string UniqueName { get; private set; }

        /// <summary>
        /// Zwraca nazwę folderu, do którego przypisana jest hierarchia.
        /// </summary>
        public string DisplayFolder { get; private set; }

        /// <summary>
        /// Zwraca typ hierarchii.
        /// </summary>
        public HierarchyType HierarchyType { get; private set; }

        List<Member> members;
        /// <summary>
        /// Zwraca członków hierarchii.
        /// </summary>
        /// <returns>Lista członków hierarchii.</returns>
        public List<Member> GetMembers() { return members; }

        /// <summary>
        /// Inicjalizuje instancję klasy przy pomocy obiektu reprezentującego hierarchię.
        /// </summary>
        /// <param name="hierarchy">Obiekt reprezentujący hierarchię, pochodzący z ADOMD.NET.</param>
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
