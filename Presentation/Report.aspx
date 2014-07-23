<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="Presentation.ReportGeneration" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Raport</title>
</head>
<body>
    <form id="form" runat="server">
    <asp:ScriptManager runat="server"></asp:ScriptManager>
    <div>
        <asp:Button ID="buttonOfExportingToPDF" Text="Pobierz jako PDF" runat="server" />
        <rsweb:ReportViewer ID="reportViewer" ShowExportControls="False" runat="server" ></rsweb:ReportViewer>
    </div>
    </form>
</body>
</html>
