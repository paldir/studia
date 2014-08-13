<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Presentation.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Logowanie</title>
</head>
<body>
    <form id="form" runat="server">
    <div>
        <asp:Login ID="login" runat="server">
            <LayoutTemplate>
                Poziom dostępu: <asp:DropDownList ID="UserName" runat="server">
                    <asp:ListItem>Podstawowy</asp:ListItem>
                    <asp:ListItem>Zaawansowany</asp:ListItem>
                </asp:DropDownList><br />
                Hasło: <asp:TextBox ID="Password" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ControlToValidate="Password" Text="*" runat="server"></asp:RequiredFieldValidator>
                <asp:Literal ID="FailureText" runat="server"></asp:Literal>
                <br />
                <asp:Button ID="Login" CommandName="Login" Text="Zaloguj" runat="server" />
            </LayoutTemplate>
        </asp:Login>
        <asp:LoginName runat="server" />
    </div>
    </form>
</body>
</html>
