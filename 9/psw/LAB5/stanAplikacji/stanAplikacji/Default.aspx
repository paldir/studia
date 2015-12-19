<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="stanAplikacji.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            Aplikacja: <% =Application["app"] %>
            Sesja: <% =Session["session"] %>
        </div>
        <asp:Button ID="przyciskInkrementacji" Text="Inkrementuj" OnClick="przyciskInkrementacji_Click" runat="server" />
        <asp:Button ID="przyciskCiasteczka" Text="Usuń ciasteczko" OnClick="przyciskCiasteczka_Click" runat="server" />
    </form>
</body>
</html>
