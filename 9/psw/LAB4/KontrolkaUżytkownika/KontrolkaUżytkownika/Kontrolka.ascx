<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Kontrolka.ascx.cs" Inherits="KontrolkaUżytkownika.Kontrolka" %>

<asp:TextBox ID="pole" runat="server"></asp:TextBox>
<asp:Button ID="przycisk" Text="Przepisz" OnClick="przycisk_Click" runat="server" /><br />
<asp:Label ID="etykieta" runat="server"></asp:Label>