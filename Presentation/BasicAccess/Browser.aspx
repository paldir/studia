<%@ Page Title="Przeglądarka kostki" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Browser.aspx.cs" Inherits="Presentation.BasicAccess.Browser" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function postBackFromDimensionTreeView() {
            var o = window.event.srcElement;
            if (o.tagName == "INPUT" && o.type == "checkbox") {
                __doPostBack("<%= postBackButtonOfDimensionTreeView.UniqueID %>", "");
            }
        }

        function postBackFromMeasuresTreeView() {
            var o = window.event.srcElement;
            if (o.tagName == "INPUT" && o.type == "checkbox") {
                __doPostBack("<%= postBackButtonOfMeasuresTreeView.UniqueID %>", "");
            }
        }
   </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
<div class="columns">
    <div id="leftColumn" class="column">
        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div id="placeOfListOfDimensions" runat="server"></div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="column">
        <asp:UpdatePanel ID="dimensionTreeViewUpdatePanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="placeOfDimensionTreeView" runat="server"></div>
                <asp:LinkButton ID="postBackButtonOfDimensionTreeView" Text="" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="centralColumn" class="column">
        <asp:UpdatePanel ID="measuresTreeViewUpdatePanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="placeOfMeasuresTreeView" runat="server"></div>
                <asp:LinkButton ID="postBackButtonOfMeasuresTreeView" Text="" runat="server"></asp:LinkButton>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="rightColumn" class="column">
        <asp:UpdatePanel ID="tableOfResultsUpdatePanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="placeOfTableOfResults" runat="server"></div>
                <asp:Button ID="buttonOfReportGeneration" Enabled="false" Text="Generuj raport" runat="server"></asp:Button>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>
</asp:Content>
