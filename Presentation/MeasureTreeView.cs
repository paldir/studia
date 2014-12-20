using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI.WebControls;

namespace Presentation
{
    public class MeasureTreeView : MyTreeView
    {
        public MeasureTreeView(List<DataAccess.Measure> measures)
        {
            ID = "MeasuresTreeView";
            List<string> groupsOfMeasures = measures.Select(m => m.MeasureGroup).Distinct().ToList();

            Attributes.Add("onclick", "postBackFromMeasuresTreeView()");
            groupsOfMeasures.Sort();

            foreach (string measureGroup in groupsOfMeasures)
            {
                List<DataAccess.Measure> children = measures.FindAll(m => m.MeasureGroup == measureGroup).OrderBy(m => m.Name).ToList();
                TreeNodeSelectAction treeNodeSelectAction = TreeNodeSelectAction.None;

                if (children.Count > 0)
                    treeNodeSelectAction = TreeNodeSelectAction.Expand;

                Nodes.Add(new MyTreeNode(measureGroup, measureGroup, treeNodeSelectAction, false, "~/Images/folder.png"));

                foreach (DataAccess.Measure measure in children)
                    Nodes[Nodes.Count - 1].ChildNodes.Add(new MyTreeNode(measure.Name, measure.UniqueName, TreeNodeSelectAction.None, true, "~/Images/measure.png"));
            }
        }
    }
}