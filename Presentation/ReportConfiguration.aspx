<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportConfiguration.aspx.cs" Inherits="Presentation.ReportConfiguration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Konfiguracja raportu</title>
    <link href="StyleSheet.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form" runat="server">
    <asp:ScriptManager ID="scriptManager" runat="server" />
    <div>
        <div class="columns">
            <div class="column">
                Hierarchie wymiarów:<br />
                <asp:UpdatePanel ID="updatePanelOfListOfHierarchies" runat="server" UpdateMode="Conditional">
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
                <asp:UpdatePanel ID="updatePanelOfListOfMeasures" runat="server" UpdateMode="Conditional">
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
            </div>
            <div class="column">
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
                Rozmiar marginesu (cm):<br />
                <asp:TextBox ID="textBoxOfMarginSize" Text="1" Width="100" runat="server"></asp:TextBox>
            </div>
        </div>
        <asp:Button ID="buttonOfViewingOfReport" Text="Zobacz raport" runat="server" />
    </div>
    </form>
</body>
</html>
