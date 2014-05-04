<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Presentation.Default" %>
<asp:Content ID="headContent" ContentPlaceHolderID="head" runat="server">
    <link href="StyleSheet.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="contentPlaceHolder" runat="server">
    <asp:ScriptManager ID="scriptManager" runat="server" />
    <div id="columns">
        <div id="leftColumn" class="column">
            <div id="dimensionsListPlace" runat="server"></div>
            <asp:UpdatePanel ID="dimensionTreeViewUpdatePanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div id="dimensionTreeViewPlace" runat="server"></div>
                    <asp:Button ID="dimensionTreeViewPostBackButton" Text="Aktualizuj wymiary" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="centralColumn" class="column">
            <div id="measuresListPlace" runat="server"></div>
        </div>
        <div id="rightColumn" class="column">
            <asp:UpdatePanel ID="selectedItemsUpdatePanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <b>Wymiary:</b><br />
                    <div id="selectedDimensionsPlace" runat="server"></div>
                    <b>Miary:</b><br />
                    <div id="selectedMeasuresPlace" runat="server"></div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
