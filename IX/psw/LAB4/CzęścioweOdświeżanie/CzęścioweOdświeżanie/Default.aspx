<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CzęścioweOdświeżanie.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<asp:ScriptManagerProxy runat="server"></asp:ScriptManagerProxy>--%>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:Label ID="etykieta" runat="server"></asp:Label>
            <br />
            <asp:Button ID="przycisk" Text="Aktualizuj czas" OnClick="przycisk_Click" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
