<%@ Page Title="Raport" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="Presentation.AdvancedAccess.Report" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <br /><asp:Button ID="buttonOfExportingToPDF" Text="Pobierz jako PDF" runat="server" />
    <rsweb:ReportViewer ID="reportViewer" ShowExportControls="False" runat="server" ></rsweb:ReportViewer>
</asp:Content>
