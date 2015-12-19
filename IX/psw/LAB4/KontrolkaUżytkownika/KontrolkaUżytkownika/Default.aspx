<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="KontrolkaUżytkownika.Default" %>

<%@ Register TagPrefix="asp" TagName="Przepisywacz" Src="~/Kontrolka.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Przepisywacz runat="server" />
        </div>
    </form>
</body>
</html>
