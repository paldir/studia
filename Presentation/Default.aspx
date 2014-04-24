<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Presentation.Default" %>
<asp:Content ID="headContent" ContentPlaceHolderID="head" runat="server">
    <link href="StyleSheet.css" rel="stylesheet" type="text/css" />
    <script src="JavaScript.js"></script>
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="contentPlaceHolder" runat="server">
        <div id="columns">
            <div id="leftColumn" class="column" runat="server"></div>
            <div id="centralColumn" class="column" runat="server"></div>
            <div id="rightColumn" class="column" runat="server"></div>
        </div>
</asp:Content>
