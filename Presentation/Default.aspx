<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Presentation.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Przeglądarka kostki</title>
    <link href="StyleSheet.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form" runat="server">
    <div>
        <asp:ScriptManager ID="scriptManager" runat="server"></asp:ScriptManager>
        <div class="columns">
            <div id="leftColumn" class="column">
                <div id="placeOfListOfDimensions" runat="server"></div>
                <asp:UpdatePanel ID="dimensionTreeViewUpdatePanel" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div id="placeOfDimensionTreeView" runat="server"></div>
                        <asp:Button ID="postBackButtonOfDimensionTreeView" Text="Aktualizuj wymiary" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div id="centralColumn" class="column">
                <asp:UpdatePanel ID="listOfMeasuresUpdatePanel" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div id="placeOfListOfMeasures" runat="server"></div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div id="rightColumn" class="column">
                <asp:UpdatePanel ID="selectedItemsUpdatePanel" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <b>Wymiary:</b><br />
                        <div id="placeOfListOfSelectedDimensions" runat="server"></div>
                        <b>Miary:</b><br />
                        <div id="placeOfListOfSelectedMeasures" runat="server"></div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="tableOfResultsUpdatePanel" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div id="placeOfTableOfResults" runat="server"></div>
                        <asp:Button ID="buttonOfReportGeneration" Enabled="false" Text="Generuj raport" runat="server"></asp:Button>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
