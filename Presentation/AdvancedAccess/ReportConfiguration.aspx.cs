using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Drawing;

namespace Presentation.AdvancedAccess
{
    /// <summary>
    /// Reprezentuje stronę aspx, która zawiera pola konfigurujące raport. Umożliwia zmianę wyglądu raportu.
    /// </summary>
    public partial class ReportConfiguration : System.Web.UI.Page
    {
        #region fields
        enum ListToModify { ListOfHierarchies, ListOfMeasures };
        enum PaperSize { A5, A4, A3 };
        enum Orientation { Vertical, Horizontal };
        int[] countsOfMembersOfEachHierarchy;
        Font font;
        DropDownList listOfFonts;

        List<string> namesOfHierarchies
        {
            get
            {
                string key = SessionKeys.ReportConfiguration.NamesOfHierarchies;
                
                if (Session[key] == null)
                    Session[key] = new List<string>();

                return (List<string>)Session[key];
            }

            set { Session[SessionKeys.ReportConfiguration.NamesOfHierarchies] = value; }
        }

        List<string> namesOfMeasures
        {
            get
            {
                string key = SessionKeys.ReportConfiguration.NamesOfMeasures;
                
                if (Session[key] == null)
                    Session[key] = new List<string>();

                return (List<string>)Session[key];
            }

            set { Session[SessionKeys.ReportConfiguration.NamesOfMeasures] = value; }
        }

        List<string[]> rows
        {
            get
            {
                string key = SessionKeys.ReportConfiguration.Rows;

                if (Session[key] == null)
                    Session[key] = new List<string[]>();

                return (List<string[]>)Session[key];
            }

            set { Session[SessionKeys.ReportConfiguration.Rows] = value; }
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

        string selectedValueOfTitle
        {
            get
            {
                string key = SessionKeys.ReportConfiguration.Title;
                
                if (Session[key] == null)
                    Session[key] = String.Empty;

                return Session[key].ToString();
            }
            set { Session[SessionKeys.ReportConfiguration.Title] = value; }
        }

        string selectedValueOfListOfFormats
        {
            get
            {
                string key = SessionKeys.ReportConfiguration.ListOfFormats;

                if (Session[key] == null)
                    Session[key] = String.Empty;

                return Session[key].ToString();
            }
            set { Session[SessionKeys.ReportConfiguration.ListOfFormats] = value; }
        }

        string selectedValueOfListOfOrientations
        {
            get
            {
                string key = SessionKeys.ReportConfiguration.ListOfOrientations;
                
                if (Session[key] == null)
                    Session[key] = String.Empty;

                return Session[key].ToString();
            }
            set { Session[SessionKeys.ReportConfiguration.ListOfOrientations] = value; }
        }

        string selectedValueOfTextBoxOfMarginSize
        {
            get
            {
                string key = SessionKeys.ReportConfiguration.TextBoxOfMarginSize;
                
                if (Session[key] == null)
                    Session[key] = String.Empty;

                return Session[key].ToString();
            }
            set { Session[SessionKeys.ReportConfiguration.TextBoxOfMarginSize] = value; }
        }

        string selectedValueOfListOfFonts
        {
            get
            {
                string key = SessionKeys.ReportConfiguration.ListOfFonts;
                
                if (Session[key] == null)
                    Session[key] = String.Empty;

                return Session[key].ToString();
            }

            set { Session[SessionKeys.ReportConfiguration.ListOfFonts] = value; }
        }

        string selectedValueOfTextBoxOfFontSize
        {
            get
            {
                string key = SessionKeys.ReportConfiguration.TextBoxOfFontSize;

                if (Session[key] == null)
                    Session[key] = String.Empty;

                return Session[key].ToString();
            }
            set { Session[SessionKeys.ReportConfiguration.TextBoxOfFontSize] = value; }
        }

        string selectedValueOfListOfColorsOfCaptionsTexts
        {
            get
            {
                string key = SessionKeys.ReportConfiguration.SelectedValueOfListOfColorsOfCaptionsTexts;
                
                if (Session[key] == null)
                    Session[key] = String.Empty;

                return Session[key].ToString();
            }

            set { Session[SessionKeys.ReportConfiguration.SelectedValueOfListOfColorsOfCaptionsTexts] = value; }
        }

        string selectedValueOfListOfColorsOfValuesTexts
        {
            get
            {
                string key = SessionKeys.ReportConfiguration.SelectedValueOfListOfColorsOfValuesTexts;
                
                if (Session[key] == null)
                    Session[key] = String.Empty;

                return Session[key].ToString();
            }

            set { Session[SessionKeys.ReportConfiguration.SelectedValueOfListOfColorsOfValuesTexts] = value; }
        }

        string selectedValueOfListOfColorsOfFirstBackgroundOfCaptions
        {
            get
            {
                string key = SessionKeys.ReportConfiguration.SelectedValueOfListOfColorsOfFirstBackgroundOfCaptions;
                
                if (Session[key] == null)
                    Session[key] = String.Empty;

                return Session[key].ToString();
            }

            set { Session[SessionKeys.ReportConfiguration.SelectedValueOfListOfColorsOfFirstBackgroundOfCaptions] = value; }
        }

        string selectedValueOfListOfColorsOfSecondBackgroundOfCaptions
        {
            get
            {
                string key = SessionKeys.ReportConfiguration.SelectedValueOfListOfColorsOfSecondBackgroundOfCaptions;
                
                if (Session[key] == null)
                    Session[key] = String.Empty;

                return Session[key].ToString();
            }

            set { Session[SessionKeys.ReportConfiguration.SelectedValueOfListOfColorsOfSecondBackgroundOfCaptions] = value; }
        }

        string selectedValueOfListOfColorsOfBackgroundOfValues
        {
            get
            {
                string key = SessionKeys.ReportConfiguration.SelectedValueOfListOfColorsOfBackgroundOfValues;

                if (Session[key] == null)
                    Session[key] = String.Empty;

                return Session[key].ToString();
            }

            set { Session[SessionKeys.ReportConfiguration.SelectedValueOfListOfColorsOfBackgroundOfValues] = value; }
        }
        #endregion

        #region methods
        protected void Page_Load(object sender, EventArgs e)
        {
            countsOfMembersOfEachHierarchy = new int[namesOfHierarchies.Count];

            InitializeButtons();
            InitializeListOfFonts();
            InitializeValues();
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

            if (String.IsNullOrEmpty(selectedValueOfListOfFonts))
                selectedValueOfListOfFonts = listOfFonts.SelectedValue;
            else
                listOfFonts.SelectedValue = selectedValueOfListOfFonts;

            placeOfListOfFonts.Controls.Add(listOfFonts);

            textBoxOfFontSize.TextChanged += textBoxOfFontSize_TextChanged;
        }

        void InitializeValues()
        {
            if (!String.IsNullOrEmpty(selectedValueOfTitle))
                textBoxOfTitle.Text = selectedValueOfTitle;

            if (!String.IsNullOrEmpty(selectedValueOfListOfFormats))
                listOfSizesOfPaper.SelectedValue = selectedValueOfListOfFormats;

            if (!String.IsNullOrEmpty(selectedValueOfListOfOrientations))
                listOfOrientations.SelectedValue = selectedValueOfListOfOrientations;

            if (!String.IsNullOrEmpty(selectedValueOfTextBoxOfMarginSize))
                textBoxOfMarginSize.Text = selectedValueOfTextBoxOfMarginSize;

            if (!String.IsNullOrEmpty(selectedValueOfTextBoxOfFontSize))
                textBoxOfFontSize.Text = selectedValueOfTextBoxOfFontSize;

            if (!String.IsNullOrEmpty(selectedValueOfListOfColorsOfCaptionsTexts))
                listOfColorsOfCaptionsTexts.Text = selectedValueOfListOfColorsOfCaptionsTexts;

            if (!String.IsNullOrEmpty(selectedValueOfListOfColorsOfFirstBackgroundOfCaptions))
                listOfColorsOfFirstBackgroundOfCaptions.Text = selectedValueOfListOfColorsOfFirstBackgroundOfCaptions;

            if (!String.IsNullOrEmpty(selectedValueOfListOfColorsOfSecondBackgroundOfCaptions))
                listOfColorsOfSecondBackgroundOfCaptions.Text = selectedValueOfListOfColorsOfSecondBackgroundOfCaptions;

            if (!String.IsNullOrEmpty(selectedValueOfListOfColorsOfValuesTexts))
                listOfColorsOfValuesTexts.Text = selectedValueOfListOfColorsOfValuesTexts;

            if (!String.IsNullOrEmpty(selectedValueOfListOfColorsOfBackgroundOfValues))
                listOfColorsOfBackgroundOfValues.Text = selectedValueOfListOfColorsOfBackgroundOfValues;

            foreach (string key in SessionKeys.ReportConfiguration.All)
                Session.Remove(key);
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            CreateListOfHierarchies();
            CreateListOfMeasures();
            CreateLabelOfTextExample();
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

        float GetFontSizeFromTextBox()
        {
            float fontSize;

            try { fontSize = Convert.ToSingle(textBoxOfFontSize.Text); }
            catch { fontSize = 10; }

            return fontSize;
        }

        static string ReplacePolishCharacters(string statement)
        {
            statement = statement.Replace("ą", "a").Replace("ć", "c").Replace("ę", "e").Replace("ń", "n").Replace("ó", "o").Replace("ś", "s").Replace("ź", "z").Replace("ż", "z");

            return statement;
        }

        static string ConvertReportDefinitionToPDFDefinition(string reportDefinition, float pDFRowHeight)
        {
            string pDFDefinition = String.Copy(reportDefinition);
            int index = reportDefinition.IndexOf("<TablixRow>");

            while (index != -1)
            {
                index = reportDefinition.IndexOf("<Height>", index);
                int end = reportDefinition.IndexOf("pt", index);

                if (index != -1)
                {
                    float value = Convert.ToSingle(reportDefinition.Substring(index + "<Height>".Length, end - (index + "<Height>".Length)).Replace('.', ','));

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

        void textBoxOfTitle_TextChanged(object sender, EventArgs e)
        {
            selectedValueOfTitle = textBoxOfTitle.Text;
        }

        void listOfSizesOfPaper_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedValueOfListOfFormats = listOfSizesOfPaper.SelectedValue;
        }

        void listOfOrientations_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedValueOfListOfOrientations = listOfOrientations.SelectedValue;
        }

        void textBoxOfMarginSize_TextChanged(object sender, EventArgs e)
        {
            selectedValueOfTextBoxOfMarginSize = textBoxOfMarginSize.Text;
        }

        void listOfFonts_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedValueOfListOfFonts = listOfFonts.SelectedValue;

            CreateLabelOfTextExample();
        }

        void textBoxOfFontSize_TextChanged(object sender, EventArgs e)
        {
            //selectedValueOfTextBoxOfFontSize = textBoxOfFontSize.Text;

            CreateLabelOfTextExample();
        }

        void listOfColorsOfCaptionsTexts_TextChanged(object sender, EventArgs e)
        {
            selectedValueOfListOfColorsOfCaptionsTexts = listOfColorsOfCaptionsTexts.Text;
        }

        void listOfColorsOfFirstBackgroundOfCaptions_TextChanged(object sender, EventArgs e)
        {
            selectedValueOfListOfColorsOfFirstBackgroundOfCaptions = listOfColorsOfFirstBackgroundOfCaptions.Text;
        }

        void listOfColorsOfSecondBackgroundOfCaptions_TextChanged(object sender, EventArgs e)
        {
            selectedValueOfListOfColorsOfSecondBackgroundOfCaptions = listOfColorsOfSecondBackgroundOfCaptions.Text;
        }

        void listOfColorsOfValuesTexts_TextChanged(object sender, EventArgs e)
        {
            selectedValueOfListOfColorsOfValuesTexts = listOfColorsOfValuesTexts.Text;
        }

        void listOfColorsOfBackgroundOfValues_TextChanged(object sender, EventArgs e)
        {
            selectedValueOfListOfColorsOfBackgroundOfValues = listOfColorsOfBackgroundOfValues.Text;
        }

        void buttonOfViewingOfReport_Click(object sender, EventArgs e)
        {
            SortMembersOfHierarchies();

            float marginSize;
            font = new Font(listOfFonts.SelectedValue, GetFontSizeFromTextBox());
            Dictionary<PaperSize, SizeF> paperSizeToSizeF = new Dictionary<PaperSize, SizeF>()
            {
                { PaperSize.A5, new SizeF(14.8f, 21) },
                { PaperSize.A4, new SizeF(21, 29.7f) },
                { PaperSize.A3, new SizeF(29.7f, 42) }
            };

            SizeF paperSize = paperSizeToSizeF[(PaperSize)listOfSizesOfPaper.SelectedIndex];

            if ((Orientation)listOfOrientations.SelectedIndex == Orientation.Horizontal)
            {
                float tmp = paperSize.Width;
                paperSize.Width = paperSize.Height;
                paperSize.Height = tmp;
            }

            try { marginSize = Convert.ToSingle(textBoxOfMarginSize.Text); }
            catch { marginSize = 1; }

            RdlGenerator rdlGenerator = new RdlGenerator(ReplacePolishCharacters(textBoxOfTitle.Text), CalculateColumnsWidths(), paperSize, marginSize, font, Color.FromName(listOfColorsOfCaptionsTexts.Text), Color.FromName(listOfColorsOfFirstBackgroundOfCaptions.Text), Color.FromName(listOfColorsOfSecondBackgroundOfCaptions.Text), Color.FromName(listOfColorsOfValuesTexts.Text), Color.FromName(listOfColorsOfBackgroundOfValues.Text));
            string reportDefinition = rdlGenerator.WriteReport(namesOfHierarchies, namesOfMeasures, rows);
            Session["reportDefinition"] = reportDefinition;
            Session["pDFDefinition"] = ConvertReportDefinitionToPDFDefinition(reportDefinition, font.Size * 2);

            selectedValueOfTitle = textBoxOfTitle.Text;
            selectedValueOfListOfFormats = listOfSizesOfPaper.SelectedValue;
            selectedValueOfListOfOrientations = listOfOrientations.SelectedValue;
            selectedValueOfTextBoxOfMarginSize = textBoxOfMarginSize.Text;
            selectedValueOfListOfFonts = listOfFonts.SelectedValue;
            selectedValueOfTextBoxOfFontSize = textBoxOfFontSize.Text;
            selectedValueOfListOfColorsOfCaptionsTexts = listOfColorsOfCaptionsTexts.Text;
            selectedValueOfListOfColorsOfFirstBackgroundOfCaptions = listOfColorsOfFirstBackgroundOfCaptions.Text;
            selectedValueOfListOfColorsOfSecondBackgroundOfCaptions = listOfColorsOfSecondBackgroundOfCaptions.Text;
            selectedValueOfListOfColorsOfValuesTexts = listOfColorsOfValuesTexts.Text;
            selectedValueOfListOfColorsOfBackgroundOfValues = listOfColorsOfBackgroundOfValues.Text;

            Response.Redirect("~/AdvancedAccess/Report.aspx");
        }
        #endregion
    }
}