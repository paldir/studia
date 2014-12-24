using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Presentation
{
    public static class SessionKeys
    {
        public static class Browser
        {
            public const string SelectedDimensions = "selectedDimensions";
            public const string SelectedMeasures = "selectedMeasures";
            public const string SelectedValueOfListOfDimensions = "selectedValueOfListOfDimensions";
            public const string TreeViewNodes = "treeViewNodes";
            public const string TreeViewDataSource = "treeViewDataSource";

            public static List<string> All
            {
                get
                {
                    return new List<string>()
                    {
                        SelectedDimensions,
                        SelectedMeasures,
                        SelectedValueOfListOfDimensions,
                        TreeViewNodes,
                        TreeViewDataSource
                    };
                }
            }
        }

        public static class ReportConfiguration
        {
            public const string NamesOfHierarchies = "namesOfHierarchies";
            public const string NamesOfMeasures = "namesOfMeasures";
            public const string Rows = "rows";
            public const string Title = "title";
            public const string ListOfFormats = "listOfFormats";
            public const string ListOfOrientations = "listOfOrientations";
            public const string TextBoxOfMarginSize = "textBoxOfMarginSize";
            public const string ListOfFonts = "listOfFonts";
            public const string TextBoxOfFontSize = "textBoxOfFontSize";
            public const string SelectedValueOfListOfColorsOfCaptionsTexts = "selectedValueOfListOfColorsOfCaptionsTexts";
            public const string SelectedValueOfListOfColorsOfValuesTexts = "selectedValueOfListOfColorsOfValuesTexts";
            public const string SelectedValueOfListOfColorsOfFirstBackgroundOfCaptions = "selectedValueOfListOfColorsOfFirstBackgroundOfCaptions";
            public const string SelectedValueOfListOfColorsOfSecondBackgroundOfCaptions = "selectedValueOfListOfColorsOfSecondBackgroundOfCaptions";
            public const string SelectedValueOfListOfColorsOfBackgroundOfValues = "selectedValueOfListOfColorsOfBackgroundOfValues";

            public static List<string> All
            {
                get
                {
                    return new List<string>()
                    {
                        Title,
                        ListOfFormats,
                        ListOfOrientations,
                        TextBoxOfMarginSize,
                        ListOfFonts,
                        TextBoxOfFontSize,
                        SelectedValueOfListOfColorsOfCaptionsTexts,
                        SelectedValueOfListOfColorsOfValuesTexts,
                        SelectedValueOfListOfColorsOfFirstBackgroundOfCaptions,
                        SelectedValueOfListOfColorsOfSecondBackgroundOfCaptions,
                        SelectedValueOfListOfColorsOfBackgroundOfValues
                    };
                }
            }
        }
    }
}