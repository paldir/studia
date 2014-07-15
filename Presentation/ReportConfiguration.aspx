<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportConfiguration.aspx.cs" Inherits="Presentation.ReportConfiguration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Konfiguracja raportu</title>
</head>
<body>
    <form id="form" runat="server">
    <asp:ScriptManager ID="scriptManager" runat="server" />
    <div>
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
        <br />
        <asp:Button ID="buttonOfViewingOfReport" Text="Zobacz raport" runat="server" />
    </div>
    </form>
</body>
</html>
