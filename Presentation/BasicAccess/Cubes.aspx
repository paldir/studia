<%@ Page Title="Wybór kostki" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Cubes.aspx.cs" Inherits="Presentation.BasicAccess.Cubes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div id="placeOfListOfCubes" runat="server"></div>
    <asp:Button ID="buttonOfBrowsing" Text="Przeglądaj" runat="server" />
</asp:Content>
