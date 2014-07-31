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

        string selectedValueOfListOfFonts
        {
            get
            {
                if (ViewState["selectedValueOfListOfFonts"] == null)
                    ViewState["selectedValueOfListOfFonts"] = String.Empty;

                return ViewState["selectedValueOfListOfFonts"].ToString();
            }

            set { ViewState["selectedValueOfListOfFonts"] = value; }
        }

        string selectedValueOfListOfColorsOfCaptionsTexts
        {
            get
            {
                if (ViewState["selectedValueOfListOfColorsOfCaptionsTexts"] == null)
                    ViewState["selectedValueOfListOfColorsOfCaptionsTexts"] = String.Empty;

                return ViewState["selectedValueOfListOfColorsOfCaptionsTexts"].ToString();
            }

            set { ViewState["selectedValueOfListOfColorsOfCaptionsTexts"] = value; }
        }

        string selectedValueOfListOfColorsOfValuesTexts
        {
            get
            {
                if (ViewState["selectedValueOfListOfColorsOfValuesTexts"] == null)
                    ViewState["selectedValueOfListOfColorsOfValuesTexts"] = String.Empty;

                return ViewState["selectedValueOfListOfColorsOfValuesTexts"].ToString();
            }

            set { ViewState["selectedValueOfListOfColorsOfValuesTexts"] = value; }
        }

        string selectedValueOfListOfColorsOfFirstBackgroundOfCaptions
        {
            get
            {
                if (ViewState["selectedValueOfListOfColorsOfFirstBackgroundOfCaptions"] == null)
                    ViewState["selectedValueOfListOfColorsOfFirstBackgroundOfCaptions"] = String.Empty;

                return ViewState["selectedValueOfListOfColorsOfFirstBackgroundOfCaptions"].ToString();
            }

            set { ViewState["selectedValueOfListOfColorsOfFirstBackgroundOfCaptions"] = value; }
        }

        string selectedValueOfListOfColorsOfSecondBackgroundOfCaptions
        {
            get
            {
                if (ViewState["selectedValueOfListOfColorsOfSecondBackgroundOfCaptions"] == null)
                    ViewState["selectedValueOfListOfColorsOfSecondBackgroundOfCaptions"] = String.Empty;

                return ViewState["selectedValueOfListOfColorsOfSecondBackgroundOfCaptions"].ToString();
            }

            set { ViewState["selectedValueOfListOfColorsOfSecondBackgroundOfCaptions"] = value; }
        }

        string selectedValueOfListOfColorsOfBackgroundOfValues
        {
            get
            {
                if (ViewState["selectedValueOfListOfColorsOfBackgroundOfValues"] == null)
                    ViewState["selectedValueOfListOfColorsOfBackgroundOfValues"] = String.Empty;

                return ViewState["selectedValueOfListOfColorsOfBackgroundOfValues"].ToString();
            }

            set { ViewState["selectedValueOfListOfColorsOfBackgroundOfValues"] = value; }
        }
        #endregion

        #region methods
        protected void Page_Load(object sender, EventArgs e)
        {
            countsOfMembersOfEachHierarchy = new int[namesOfHierarchies.Count];

            InitializeButtons();
            InitializeListOfFonts();
            InitializeListsOfColors();
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
            listOfFonts.ID = "listOfFonts";
            listOfFonts.AutoPostBack = true;
            listOfFonts.SelectedIndexChanged += listOfFonts_SelectedIndexChanged;
            FontFamily[] installedFonts = new System.Drawing.Text.InstalledFontCollection().Families;

            foreach (FontFamily font in installedFonts)
                if (font.IsStyleAvailable(FontStyle.Regular))
                    listOfFonts.Items.Add(new ListItem(font.Name, font.Name));

            listOfFonts.SelectedValue = "Arial";

            if (selectedValueOfListOfFonts == String.Empty)
                selectedValueOfListOfFonts = listOfFonts.SelectedValue;
            else
                listOfFonts.SelectedValue = selectedValueOfListOfFonts;

            placeOfListOfFonts.Controls.Add(listOfFonts);

            textBoxOfFontSize.TextChanged += textBoxOfFontSize_TextChanged;
        }

        void InitializeListsOfColors()
        {
            listOfColorsOfCaptionsTexts = GetListOfColors("listOfColorsOfCaptionsTexts", "White");
            listOfColorsOfCaptionsTexts.SelectedIndexChanged += listOfColorsOfCaptionsTexts_SelectedIndexChanged;
            listOfColorsOfFirstBackgroundOfCaptions = GetListOfColors("listOfColorsOfFirstBackgroundOfCaptions", "DarkBlue");
            listOfColorsOfFirstBackgroundOfCaptions.SelectedIndexChanged += listOfColorsOfFirstBackgroundOfCaptions_SelectedIndexChanged;
            listOfColorsOfSecondBackgroundOfCaptions = GetListOfColors("listOfColorsOfSecondBackgroundOfCaptions", "CornflowerBlue");
            listOfColorsOfSecondBackgroundOfCaptions.SelectedIndexChanged += listOfColorsOfSecondBackgroundOfCaptions_SelectedIndexChanged;
            listOfColorsOfValuesTexts = GetListOfColors("listOfColorsOfValuesTexts", "Black");
            listOfColorsOfValuesTexts.SelectedIndexChanged += listOfColorsOfValuesTexts_SelectedIndexChanged;
            listOfColorsOfBackgroundOfValues = GetListOfColors("listOfColorsOfBackgroundOfValues", "White");
            listOfColorsOfBackgroundOfValues.SelectedIndexChanged += listOfColorsOfBackgroundOfValues_SelectedIndexChanged;

            if (selectedValueOfListOfColorsOfCaptionsTexts == String.Empty)
                selectedValueOfListOfColorsOfCaptionsTexts = listOfColorsOfCaptionsTexts.SelectedValue;
            else
                listOfColorsOfCaptionsTexts.SelectedValue = selectedValueOfListOfColorsOfCaptionsTexts;

            if (selectedValueOfListOfColorsOfFirstBackgroundOfCaptions == String.Empty)
                selectedValueOfListOfColorsOfFirstBackgroundOfCaptions = listOfColorsOfFirstBackgroundOfCaptions.SelectedValue;
            else
                listOfColorsOfFirstBackgroundOfCaptions.SelectedValue = selectedValueOfListOfColorsOfFirstBackgroundOfCaptions;

            if (selectedValueOfListOfColorsOfSecondBackgroundOfCaptions == String.Empty)
                selectedValueOfListOfColorsOfSecondBackgroundOfCaptions = listOfColorsOfSecondBackgroundOfCaptions.SelectedValue;
            else
                listOfColorsOfSecondBackgroundOfCaptions.SelectedValue = selectedValueOfListOfColorsOfSecondBackgroundOfCaptions;

            if (selectedValueOfListOfColorsOfValuesTexts == String.Empty)
                selectedValueOfListOfColorsOfValuesTexts = listOfColorsOfValuesTexts.SelectedValue;
            else
                listOfColorsOfValuesTexts.SelectedValue = selectedValueOfListOfColorsOfValuesTexts;

            if (selectedValueOfListOfColorsOfBackgroundOfValues == String.Empty)
                selectedValueOfListOfColorsOfBackgroundOfValues = listOfColorsOfCaptionsTexts.SelectedValue;
            else
                listOfColorsOfCaptionsTexts.SelectedValue = selectedValueOfListOfColorsOfBackgroundOfValues;

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
            CreateLabelOfTextExample();
            CreateLabelsOfColorsExamples();
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

        void CreateLabelOfTextExample()
        {
            Label labelOfTextExample = new Label();
            labelOfTextExample.ID = "labelOfTextExample";
            labelOfTextExample.Text = "tekst";
            labelOfTextExample.Font.Name = selectedValueOfListOfFonts;
            labelOfTextExample.Font.Size = new FontUnit(GetFontSizeFromTextBox());

            placeOfLabelOfTextExample.Controls.Clear();
            placeOfLabelOfTextExample.Controls.Add(labelOfTextExample);
        }

        void CreateLabelsOfColorsExamples()
        {
            placeOfLabelOfColorsOfCaptionsTexts.Controls.Clear();
            placeOfLabelOfColorsOfCaptionsTexts.Controls.Add(GetLabelOfColorExample(selectedValueOfListOfColorsOfCaptionsTexts));
            placeOfLabelOfColorsOfFirstBackgroundOfCaptions.Controls.Clear();
            placeOfLabelOfColorsOfFirstBackgroundOfCaptions.Controls.Add(GetLabelOfColorExample(selectedValueOfListOfColorsOfFirstBackgroundOfCaptions));
            placeOfLabelOfColorsOfSecondBackgroundOfCaptions.Controls.Clear();
            placeOfLabelOfColorsOfSecondBackgroundOfCaptions.Controls.Add(GetLabelOfColorExample(selectedValueOfListOfColorsOfSecondBackgroundOfCaptions));
            placeOfLabelOfColorsOfValuesTexts.Controls.Clear();
            placeOfLabelOfColorsOfValuesTexts.Controls.Add(GetLabelOfColorExample(selectedValueOfListOfColorsOfValuesTexts));
            placeOfLabelOfColorsOfBackgroundOfValues.Controls.Clear();
            placeOfLabelOfColorsOfBackgroundOfValues.Controls.Add(GetLabelOfColorExample(selectedValueOfListOfColorsOfBackgroundOfValues));
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

        DropDownList GetListOfColors(string id, string defaultColor)
        {
            DropDownList listOfColors = new DropDownList();
            listOfColors.ID = id;
            listOfColors.AutoPostBack = true;

            foreach (string colorName in Enum.GetNames(typeof(KnownColor)))
                if (!Color.FromName(colorName).IsSystemColor)
                    listOfColors.Items.Add(new ListItem(colorName, colorName));

            listOfColors.SelectedValue = defaultColor;

            return listOfColors;
        }

        Label GetLabelOfColorExample(string colorName)
        {
            Label labelOfColorExample = new Label();
            labelOfColorExample.Height = 10;
            labelOfColorExample.Width = 10;
            labelOfColorExample.BorderWidth = 1;
            labelOfColorExample.BorderStyle = BorderStyle.Solid;
            labelOfColorExample.BorderColor = Color.Black;
            labelOfColorExample.BackColor = Color.FromName(colorName);

            return labelOfColorExample;
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

        string ReplacePolishCharacters(string statement)
        {
            statement = statement.Replace("ą", "a").Replace("ć", "c").Replace("ę", "e").Replace("ń", "n").Replace("ó", "o").Replace("ś", "s").Replace("ź", "z").Replace("ż", "z");

            return statement;
        }
        
        float GetFontSizeFromTextBox()
        {
            float fontSize;

            try { fontSize = Convert.ToSingle(textBoxOfFontSize.Text); }
            catch { fontSize = 10; }

            return fontSize;
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

        void listOfFonts_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedValueOfListOfFonts = listOfFonts.SelectedValue;

            CreateLabelOfTextExample();
        }

        void textBoxOfFontSize_TextChanged(object sender, EventArgs e) { CreateLabelOfTextExample(); }

        void listOfColorsOfCaptionsTexts_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedValueOfListOfColorsOfCaptionsTexts = listOfColorsOfCaptionsTexts.SelectedValue;

            CreateLabelsOfColorsExamples();
        }

        void listOfColorsOfFirstBackgroundOfCaptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedValueOfListOfColorsOfFirstBackgroundOfCaptions = listOfColorsOfFirstBackgroundOfCaptions.SelectedValue;

            CreateLabelsOfColorsExamples();
        }

        void listOfColorsOfSecondBackgroundOfCaptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedValueOfListOfColorsOfSecondBackgroundOfCaptions = listOfColorsOfSecondBackgroundOfCaptions.SelectedValue;

            CreateLabelsOfColorsExamples();
        }

        void listOfColorsOfValuesTexts_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedValueOfListOfColorsOfValuesTexts = listOfColorsOfValuesTexts.SelectedValue;

            CreateLabelsOfColorsExamples();
        }

        void listOfColorsOfBackgroundOfValues_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedValueOfListOfColorsOfBackgroundOfValues = listOfColorsOfBackgroundOfValues.SelectedValue;

            CreateLabelsOfColorsExamples();
        }

        void buttonOfViewingOfReport_Click(object sender, EventArgs e)
        {
            SortMembersOfHierarchies();

            SizeF sizeOfPaper = new SizeF();
            float marginSize;

            font = new Font(listOfFonts.SelectedValue, GetFontSizeFromTextBox());

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

            RdlGenerator rdlGenerator = new RdlGenerator(ReplacePolishCharacters(textBoxOfTitle.Text), CalculateColumnsWidths(), sizeOfPaper, marginSize, font, listOfColorsOfCaptionsTexts.SelectedValue, new string[] { listOfColorsOfFirstBackgroundOfCaptions.SelectedValue, listOfColorsOfSecondBackgroundOfCaptions.SelectedValue }, listOfColorsOfValuesTexts.SelectedValue, listOfColorsOfBackgroundOfValues.SelectedValue);
            string reportDefinition = rdlGenerator.WriteReport(namesOfHierarchies, namesOfMeasures, rows);
            Session["reportDefinition"] = reportDefinition;
            Session["pDFDefinition"] = ConvertReportDefinitionToPDFDefinition(reportDefinition, font.Size * 2);

            Response.Redirect("Report.aspx");
        }
        #endregion
    }
}