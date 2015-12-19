<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Loty.aspx.cs" Inherits="kalendarz.Loty" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Calendar ID="kalendarz1" runat="server"></asp:Calendar>
    <asp:Calendar ID="kalendarz2" runat="server"></asp:Calendar>
    <asp:Button ID="przyciskDodawania" Text="Dodaj" OnClick="przyciskDodawania_Click" runat="server" />
</asp:Content>
