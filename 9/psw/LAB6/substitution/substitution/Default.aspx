<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="substitution.Default" %>

<%@ OutputCache Duration="5" VaryByParam="None" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            Niesubstitution: <% =DateTime.Now %><br />
            Substitution:
            <asp:Substitution MethodName="PobierzCzas" runat="server" />
        </div>
    </form>
</body>
</html>