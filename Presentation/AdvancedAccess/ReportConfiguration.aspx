<%@ Page Title="Konfiguracja raportu" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportConfiguration.aspx.cs" Inherits="Presentation.AdvancedAccess.ReportConfiguration" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div class="columns">
        <div class="column">
            <asp:Panel  GroupingText="Ogólne" runat="server">
                Tytuł raportu:<br />
                <asp:TextBox ID="textBoxOfTitle" Text="" runat="server"></asp:TextBox>
                <br />
                Hierarchie wymiarów:<br />
                <asp:UpdatePanel ChildrenAsTriggers="true" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td>
                                    <div id="placeOfListOfHierarchies" runat="server"></div>
                                </td>
                                <td>
                                    <asp:Button ID="buttonOfMovingItemOfListOfHierarchiesUp" Text="W górę" runat="server" /><br />
                                    <asp:Button ID="buttonOfMovingItemOfListOfHierarchiesDown" Text="W dół" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                Miary:<br />
                <asp:UpdatePanel ChildrenAsTriggers="true" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td>
                                    <div id="placeOfListOfMeasures" runat="server"></div>
                                </td>
                                <td>
                                    <asp:Button ID="buttonOfMovingItemOfListOfMeasuresUp" Text="W górę" runat="server" /><br />
                                    <asp:Button ID="buttonOfMovingItemOfListOfMeasuresDown" Text="W dół" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </div>
        <div class="column">
            <asp:Panel GroupingText="Ustawienia wydruku" runat="server">
                Format:<br />
                <asp:DropDownList ID="listOfSizesOfPaper" runat="server">
                    <asp:ListItem>A5</asp:ListItem>
                    <asp:ListItem Selected="True">A4</asp:ListItem>
                    <asp:ListItem>A3</asp:ListItem>
                </asp:DropDownList><br />
                <br />
                Orientacja:<br />
                <asp:DropDownList ID="listOfOrientations" runat="server">
                    <asp:ListItem Value="vertical" Selected="True">Pionowa</asp:ListItem>
                    <asp:ListItem Value="horizontal">Pozioma</asp:ListItem>
                </asp:DropDownList><br />
                <br />
                Rozmiar marginesów (cm):<br />
                <asp:TextBox ID="textBoxOfMarginSize" Text="1" Width="50" runat="server"></asp:TextBox>
            </asp:Panel>
        </div>
        <div class="column">
            <asp:Panel GroupingText="Tekst" runat="server">
                <asp:UpdatePanel ChildrenAsTriggers="true" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        Czcionka:
                        <div id="placeOfListOfFonts" runat="server"></div>
                        <br />
                        Rozmiar czcionki:<br />
                        <asp:TextBox ID="textBoxOfFontSize" Text="10" Width="50" AutoPostBack="true" runat="server"></asp:TextBox><br />
                        <br />
                        <div id="placeOfLabelOfTextExample" runat="server"></div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </div>
        <div class="column">
            <asp:Panel ID="Panel4" GroupingText="Kolory" runat="server">
                <asp:UpdatePanel ChildrenAsTriggers="true" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        Kolor tekstu pól z nagłówkami:
                        <div class="columns">
                            <div id="placeOfListOfColorsOfCaptionsTexts" class="column" runat="server"></div><div id="placeOfLabelOfColorsOfCaptionsTexts" class="column" runat="server"></div>
                        </div>
                        <br />
                        Kolory teł pól z nagłówkami:
                        <div class="columns">
                            <div id="placeOfListOfColorsOfFirstBackgroundOfCaptions" class="column" runat="server"></div><div id="placeOfLabelOfColorsOfFirstBackgroundOfCaptions" class="column" runat="server"></div>
                        </div>
                        <div class="columns">
                            <div id="placeOfListOfColorsOfSecondBackgroundOfCaptions" class="column" runat="server"></div><div id="placeOfLabelOfColorsOfSecondBackgroundOfCaptions" class="column" runat="server"></div>
                        </div>
                        <br />
                        Kolor tekstu pól z wartościami:
                        <div class="columns">
                            <div id="placeOfListOfColorsOfValuesTexts" class="column" runat="server"></div><div id="placeOfLabelOfColorsOfValuesTexts" class="column" runat="server"></div>
                        </div>
                        <br />
                        Kolor tła pól z wartościami:
                        <div class="columns">
                            <div id="placeOfListOfColorsOfBackgroundOfValues" class="column" runat="server"></div><div id="placeOfLabelOfColorsOfBackgroundOfValues" class="column" runat="server"></div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </div>
    </div>
    <asp:Button ID="buttonOfViewingOfReport" Text="Zobacz raport" runat="server" />
</asp:Content>
