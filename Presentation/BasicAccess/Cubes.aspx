<%@ Page Title="Wybór kostki" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Cubes.aspx.cs" Inherits="Presentation.BasicAccess.Cubes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <div style="text-align: center;">
        <b>Wybierz kostkę:</b><br />
        <br />
        <div id="placeOfListOfCubes" runat="server"></div>
        <asp:Button ID="buttonOfBrowsing" Text="Przeglądaj" CssClass="simpleButton" runat="server" />
    </div>
</asp:Content>
