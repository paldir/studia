<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportConfiguration.aspx.cs" Inherits="Presentation.ReportConfiguration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Konfiguracja raportu</title>
</head>
<body>
    <form id="form" runat="server">
    <div>
        Hierarchie wymiarów:<br />
        <asp:ListBox ID="listOfHierarchies" runat="server"></asp:ListBox>
        <br />
        <br />
        Miary:<br />
        <asp:ListBox ID="listOfMeasures" runat="server"></asp:ListBox>
        <br />
        <asp:Button ID="buttonOfViewingOfReport" Text="Zobacz raport" runat="server" />
    </div>
    </form>
</body>
</html>
