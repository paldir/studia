using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Drawing;

namespace Presentation
{
    public partial class ReportConfiguration : System.Web.UI.Page
    {
        #region fields
        enum ListToModify { ListOfHierarchies, ListOfMeasures };
        enum SizeOfPaper { A5, A4, A3 };
        enum Orientation { Vertical, Horizontal };
        int[] countsOfMembersOfEachHierarchy;
        Font font;
        DropDownList listOfFonts;
        DropDownList listOfColorsOfCaptionsTexts;
        DropDownList listOfColorsOfValuesTexts;
        DropDownList listOfColorsOfFirstBackgroundOfCaptions;
        DropDownList listOfColorsOfSecondBackgroundOfCaptions;
        DropDownList listOfColorsOfBackgroundOfValues;

        List<string> namesOfHierarchies
        {
            get
            {
                if (Session["namesOfHierarchies"] == null)
                    Session["namesOfHierarchies"] = new List<string>();

                return (List<string>)Session["namesOfHierarchies"];
            }

            set { Session["namesOfHierarchies"] = value; }
        }

        List<string> namesOfMeasures
        {
            get
            {
                if (Session["namesOfMeasures"] == null)
                    Session["namesOfMeasures"] = new List<string>();

                return (List<string>)Session["namesOfMeasures"];
            }

            set { Session["namesOfMeasures"] = value; }
        }

        List<string[]> rows
        {
            get
            {
                if (Session["rows"] == null)
                    Session["rows"] = new List<string[]>();

                return (List<string[]>)Session["rows"];
            }

            set { Session["rows"] = value; }
        }

        int selectedIndexOfListOfHierarchies
        {
            get { return Convert.ToInt16(ViewState["selectedIndexOfListOfHierarchies"]); }
            set { ViewState["selectedIndexOfListOfHierarchies"] = value; }
        }

        int selectedIndexOfListOfMeasures
        {
            get { return Convert.ToInt16(ViewState["selectedIndexOfListOfMeasures"]); }
            set { ViewState["selectedIndexOfListOfMeasures"] = value; }
        }
        #endregion

        #region methods
        protected void Page_Load(object sender, EventArgs e)
        {
            countsOfMembersOfEachHierarchy = new int[namesOfHierarchies.Count];

            InitializeListsOfItems();
            InitializeButtons();
            InitializeListOfFonts();
            InitializeListsOfColors();
        }

        void InitializeListsOfItems()
        {
            AsyncPostBackTrigger triggerOfListOfHierarchies = new AsyncPostBackTrigger();
            triggerOfListOfHierarchies.ControlID = "listOfHierarchies";
            triggerOfListOfHierarchies.EventName = "SelectedIndexChanged";

            AsyncPostBackTrigger triggerOfButtonOfMovingItemOfListOfHierarchiesUp = new AsyncPostBackTrigger();
            triggerOfButtonOfMovingItemOfListOfHierarchiesUp.ControlID = "buttonOfMovingItemOfListOfHierarchiesUp";
            triggerOfButtonOfMovingItemOfListOfHierarchiesUp.EventName = "Click";

            AsyncPostBackTrigger triggerOfButtonOfMovingItemOfListOfHierarchiesDown = new AsyncPostBackTrigger();
            triggerOfButtonOfMovingItemOfListOfHierarchiesDown.ControlID = "buttonOfMovingItemOfListOfHierarchiesDown";
            triggerOfButtonOfMovingItemOfListOfHierarchiesDown.EventName = "Click";

            AsyncPostBackTrigger triggerOfListOfMeasures = new AsyncPostBackTrigger();
            triggerOfListOfMeasures.ControlID = "listOfMeasures";
            triggerOfListOfMeasures.EventName = "SelectedIndexChanged";

            AsyncPostBackTrigger triggerOfButtonOfMovingItemOfListOfMeasuresUp = new AsyncPostBackTrigger();
            triggerOfButtonOfMovingItemOfListOfMeasuresUp.ControlID = "buttonOfMovingItemOfListOfMeasuresUp";
            triggerOfButtonOfMovingItemOfListOfMeasuresUp.EventName = "Click";

            AsyncPostBackTrigger triggerOfButtonOfMovingItemOfListOfMeasuresDown = new AsyncPostBackTrigger();
            triggerOfButtonOfMovingItemOfListOfMeasuresDown.ControlID = "buttonOfMovingItemOfListOfMeasuresDown";
            triggerOfButtonOfMovingItemOfListOfMeasuresDown.EventName = "Click";

            updatePanelOfListOfHierarchies.Triggers.Add(triggerOfListOfHierarchies);
            updatePanelOfListOfHierarchies.Triggers.Add(triggerOfButtonOfMovingItemOfListOfHierarchiesUp);
            updatePanelOfListOfHierarchies.Triggers.Add(triggerOfButtonOfMovingItemOfListOfHierarchiesDown);

            updatePanelOfListOfMeasures.Triggers.Add(triggerOfListOfMeasures);
            updatePanelOfListOfMeasures.Triggers.Add(triggerOfButtonOfMovingItemOfListOfMeasuresUp);
            updatePanelOfListOfMeasures.Triggers.Add(triggerOfButtonOfMovingItemOfListOfMeasuresDown);
        }

        void InitializeButtons()
        {
            buttonOfMovingItemOfListOfHierarchiesUp.Click += buttonOfMovingItemOfListOfHierarchiesUp_Click;
            buttonOfMovingItemOfListOfHierarchiesDown.Click += buttonOfMovingItemOfListOfHierarchiesDown_Click;
            buttonOfMovingItemOfListOfMeasuresUp.Click += buttonOfMovingItemOfListOfMeasuresUp_Click;
            buttonOfMovingItemOfListOfMeasuresDown.Click += buttonOfMovingItemOfListOfMeasuresDown_Click;
            buttonOfViewingOfReport.Click += buttonOfViewingOfReport_Click;
        }

        void InitializeListOfFonts()
        {
            listOfFonts = new DropDownList();

            FontFamily[] installedFonts = new System.Drawing.Text.InstalledFontCollection().Families;

            foreach (FontFamily font in installedFonts)
                listOfFonts.Items.Add(new ListItem(font.Name, font.Name));

            listOfFonts.SelectedValue = "Arial";

            placeOfListOfFonts.Controls.Add(listOfFonts);
        }

        void InitializeListsOfColors()
        {
            listOfColorsOfCaptionsTexts = GetListOfColors("White");
            listOfColorsOfFirstBackgroundOfCaptions = GetListOfColors("DarkBlue");
            listOfColorsOfSecondBackgroundOfCaptions = GetListOfColors("CornflowerBlue");
            listOfColorsOfValuesTexts = GetListOfColors("Black");
            listOfColorsOfBackgroundOfValues = GetListOfColors("White");

            placeOfListOfColorsOfCaptionsTexts.Controls.Add(listOfColorsOfCaptionsTexts);
            placeOfListOfColorsOfFirstBackgroundOfCaptions.Controls.Add(listOfColorsOfFirstBackgroundOfCaptions);
            placeOfListOfColorsOfSecondBackgroundOfCaptions.Controls.Add(listOfColorsOfSecondBackgroundOfCaptions);
            placeOfListOfColorsOfValuesTexts.Controls.Add(listOfColorsOfValuesTexts);
            placeOfListOfColorsOfBackgroundOfValues.Controls.Add(listOfColorsOfBackgroundOfValues);
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            CreateListOfHierarchies();
            CreateListOfMeasures();
        }

        void CreateListOfHierarchies()
        {
            ListBox listOfHierarchies = new ListBox();
            listOfHierarchies.ID = "listOfHierarchies";
            listOfHierarchies.SelectionMode = ListSelectionMode.Single;
            listOfHierarchies.AutoPostBack = true;
            listOfHierarchies.SelectedIndexChanged += listOfHierarchies_SelectedIndexChanged;

            listOfHierarchies.Items.Clear();

            foreach (string nameOfHierarchy in namesOfHierarchies)
                listOfHierarchies.Items.Add(nameOfHierarchy);

            if (selectedIndexOfListOfHierarchies != -1)
                listOfHierarchies.SelectedIndex = selectedIndexOfListOfHierarchies;

            placeOfListOfHierarchies.Controls.Clear();
            placeOfListOfHierarchies.Controls.Add(listOfHierarchies);
        }

        void CreateListOfMeasures()
        {
            ListBox listOfMeasures = new ListBox();
            listOfMeasures.ID = "listOfMeasures";
            listOfMeasures.SelectionMode = ListSelectionMode.Single;
            listOfMeasures.AutoPostBack = true;
            listOfMeasures.SelectedIndexChanged += listOfMeasures_SelectedIndexChanged;

            listOfMeasures.Items.Clear();

            foreach (string nameOfMeasure in namesOfMeasures)
                listOfMeasures.Items.Add(nameOfMeasure);

            if (selectedIndexOfListOfMeasures != -1)
                listOfMeasures.SelectedIndex = selectedIndexOfListOfMeasures;

            placeOfListOfMeasures.Controls.Clear();
            placeOfListOfMeasures.Controls.Add(listOfMeasures);
        }

        float[] CalculateColumnsWidths()
        {
            float[] result = new float[rows.First().Length];
            Bitmap bitMap = new Bitmap(500, 200);
            Graphics graphics = Graphics.FromImage(bitMap);

            for (int i = 0; i < namesOfHierarchies.Count; i++)
                result[i] = graphics.MeasureString(namesOfHierarchies.ElementAt(i), font).Width;

            for (int i = namesOfHierarchies.Count; i < result.Length; i++)
                result[i] = graphics.MeasureString(namesOfMeasures.ElementAt(i - namesOfHierarchies.Count), font).Width;

            foreach (string[] row in rows)
                for (int i = 0; i < row.Length; i++)
                    if (graphics.MeasureString(row[i], font).Width > result[i])
                        result[i] = graphics.MeasureString(row[i], font).Width;

            return result;
        }

        void MoveItemOfList(ListToModify listToModify, int destination)
        {
            int indexOfSelectedItem = -1;
            int generalIndexOfSelectedItem = -1;
            List<string> list = null;

            switch (listToModify)
            {
                case ListToModify.ListOfHierarchies:
                    indexOfSelectedItem = selectedIndexOfListOfHierarchies;
                    generalIndexOfSelectedItem = selectedIndexOfListOfHierarchies;
                    list = namesOfHierarchies;

                    if (selectedIndexOfListOfHierarchies + destination >= 0 && selectedIndexOfListOfHierarchies + destination < list.Count)
                        selectedIndexOfListOfHierarchies += destination;
                    break;
                case ListToModify.ListOfMeasures:
                    indexOfSelectedItem = selectedIndexOfListOfMeasures;
                    generalIndexOfSelectedItem = selectedIndexOfListOfMeasures + namesOfHierarchies.Count;
                    list = namesOfMeasures;

                    if (selectedIndexOfListOfMeasures + destination >= 0 && selectedIndexOfListOfMeasures + destination < list.Count)
                        selectedIndexOfListOfMeasures += destination;
                    break;
            }

            switch (destination)
            {
                case -1:
                    if (indexOfSelectedItem > 0)
                    {
                        list.Reverse(indexOfSelectedItem - 1, 2);

                        foreach (string[] row in rows)
                            Array.Reverse(row, generalIndexOfSelectedItem - 1, 2);
                    }
                    break;
                case 1:
                    if (indexOfSelectedItem < list.Count - 1)
                    {
                        list.Reverse(indexOfSelectedItem, 2);

                        foreach (string[] row in rows)
                            Array.Reverse(row, generalIndexOfSelectedItem, 2);
                    }
                    break;
            }
        }

        void SortMembersOfHierarchies()
        {
            for (int i = namesOfHierarchies.Count - 1; i >= 0; i--)
            {
                List<string> membersOfHierarchy = rows.Select(r => r[i]).Distinct().ToList();
                int indexOfTarget = 0;

                foreach (string memberOfHierarchy in membersOfHierarchy)
                {
                    List<string[]> rowsToMove = rows.FindAll(r => r[i] == memberOfHierarchy);

                    rows.RemoveAll(r => r[i] == memberOfHierarchy);
                    rows.InsertRange(indexOfTarget, rowsToMove);

                    indexOfTarget += rowsToMove.Count;
                }
            }
        }

        DropDownList GetListOfColors(string defaultColor)
        {
            DropDownList listOfColors = new DropDownList();

            foreach (string colorName in Enum.GetNames(typeof(KnownColor)))
                if (!Color.FromName(colorName).IsSystemColor)
                    listOfColors.Items.Add(new ListItem(colorName, colorName));

            listOfColors.SelectedValue = defaultColor;

            return listOfColors;
        }

        string ConvertReportDefinitionToPDFDefinition(string reportDefinition, float pDFRowHeight)
        {
            string pDFDefinition = String.Copy(reportDefinition);
            int index = reportDefinition.IndexOf("<TablixRow>");

            while (index != -1)
            {
                index = reportDefinition.IndexOf("<Height>", index);
                int end = reportDefinition.IndexOf("pt", index);

                if (index != -1)
                {
                    float value = Convert.ToSingle(reportDefinition.Substring(index + "<Height>".Length, end - (index + "<Height>".Length)));

                    if (value % pDFRowHeight != 0)
                    {
                        pDFDefinition = pDFDefinition.Remove(index + "<Height>".Length, end - (index + "<Height>".Length));
                        pDFDefinition = pDFDefinition.Insert(index + "<Height>".Length, (Math.Floor(value / pDFRowHeight) * pDFRowHeight).ToString());
                    }
                }

                index = reportDefinition.IndexOf("<TablixRow>", end);
            }

            return pDFDefinition;
        }
        #endregion

        #region events handlers
        void listOfHierarchies_SelectedIndexChanged(object sender, EventArgs e) { selectedIndexOfListOfHierarchies = ((ListBox)sender).SelectedIndex; }

        void listOfMeasures_SelectedIndexChanged(object sender, EventArgs e) { selectedIndexOfListOfMeasures = ((ListBox)sender).SelectedIndex; }

        void buttonOfMovingItemOfListOfHierarchiesUp_Click(object sender, EventArgs e)
        {
            MoveItemOfList(ListToModify.ListOfHierarchies, -1);
            CreateListOfHierarchies();
        }

        void buttonOfMovingItemOfListOfHierarchiesDown_Click(object sender, EventArgs e)
        {
            MoveItemOfList(ListToModify.ListOfHierarchies, 1);
            CreateListOfHierarchies();
        }

        void buttonOfMovingItemOfListOfMeasuresUp_Click(object sender, EventArgs e)
        {
            MoveItemOfList(ListToModify.ListOfMeasures, -1);
            CreateListOfMeasures();
        }

        void buttonOfMovingItemOfListOfMeasuresDown_Click(object sender, EventArgs e)
        {
            MoveItemOfList(ListToModify.ListOfMeasures, 1);
            CreateListOfMeasures();
        }

        void buttonOfViewingOfReport_Click(object sender, EventArgs e)
        {
            SortMembersOfHierarchies();

            float fontSize;
            SizeF sizeOfPaper = new SizeF();
            float marginSize;

            try { fontSize = Convert.ToSingle(textBoxOfFontSize.Text); }
            catch { fontSize = 10; }

            font = new Font(listOfFonts.SelectedValue, fontSize);

            switch ((SizeOfPaper)listOfSizesOfPaper.SelectedIndex)
            {
                case SizeOfPaper.A5:
                    sizeOfPaper = new SizeF(14.8f, 21);
                    break;
                case SizeOfPaper.A4:
                    sizeOfPaper = new SizeF(21, 29.7f);
                    break;
                case SizeOfPaper.A3:
                    sizeOfPaper = new SizeF(29.7f, 42);
                    break;
            }

            if ((Orientation)listOfOrientations.SelectedIndex == Orientation.Horizontal)
            {
                float tmp = sizeOfPaper.Width;
                sizeOfPaper.Width = sizeOfPaper.Height;
                sizeOfPaper.Height = tmp;
            }

            try { marginSize = Convert.ToSingle(textBoxOfMarginSize.Text); }
            catch { marginSize = 1; }

            RdlGenerator rdlGenerator = new RdlGenerator(CalculateColumnsWidths(), sizeOfPaper, marginSize, font, listOfColorsOfCaptionsTexts.SelectedValue, new string[] { listOfColorsOfFirstBackgroundOfCaptions.SelectedValue, listOfColorsOfSecondBackgroundOfCaptions.SelectedValue }, listOfColorsOfValuesTexts.SelectedValue, listOfColorsOfBackgroundOfValues.SelectedValue);
            string reportDefinition = rdlGenerator.WriteReport(namesOfHierarchies, namesOfMeasures, rows);
            Session["reportDefinition"] = reportDefinition;
            Session["pDFDefinition"] = ConvertReportDefinitionToPDFDefinition(reportDefinition, font.Size * 2);

            Response.Redirect("Report.aspx");
        }
        #endregion
    }
}