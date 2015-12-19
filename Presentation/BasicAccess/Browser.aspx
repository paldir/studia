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
    <table class="mainTable">
        <thead>
            <tr>
                <th colspan="2">Wymiary</th>
                <th rowspan="2">Miary</th>
                <th rowspan="2">Wyniki zapytania</th>
            </tr>
            <tr>
                <th>Dostępne wymiary</th>
                <th>Struktura wybranego wymiaru</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td class="mainTd">
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <div id="placeOfListOfDimensions" runat="server"></div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td class="mainTd">
                    <asp:UpdatePanel ID="dimensionTreeViewUpdatePanel" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div id="placeOfDimensionTreeView" runat="server"></div>
                            <asp:LinkButton ID="postBackButtonOfDimensionTreeView" Text="" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td class="mainTd">
                    <asp:UpdatePanel ID="measuresTreeViewUpdatePanel" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div id="placeOfMeasuresTreeView" runat="server"></div>
                            <asp:LinkButton ID="postBackButtonOfMeasuresTreeView" Text="" runat="server"></asp:LinkButton>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td class="mainTd">
                    <asp:UpdatePanel ID="tableOfResultsUpdatePanel" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div id="placeOfTableOfResults" runat="server"></div>
                            <asp:Button ID="buttonOfReportGeneration" Enabled="false" Text="Generuj raport" CssClass="simpleButton" runat="server"></asp:Button>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>
