<%@ Page Title="Generowanie dowolnego zapytania" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CustomQuery.aspx.cs" Inherits="WebApplication.CustomQuery" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContentPlaceHolder" runat="server">
    Treść zapytania w języku MDX:<br />
    <asp:TextBox runat="server" ID="queryText" Height="300px" Width="600px" TextMode="MultiLine"></asp:TextBox><br />
    <asp:Button runat="server" ID="executeQuery" Text="Wykonaj zapytanie" OnClick="executeQuery_Click"/><br />
    
</asp:Content>
