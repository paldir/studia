<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Presentation.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Logowanie</title>
</head>
<body>
    <form id="form" runat="server">
    <div style="width: 250px; margin: auto">
        <asp:Login ID="login" runat="server">
            <LayoutTemplate>
                <table>
                    <tr>
                        <td>Poziom dostępu:</td>
                        <td>
                            <asp:DropDownList ID="UserName" Width="120" runat="server">
                                <asp:ListItem>Podstawowy</asp:ListItem>
                                <asp:ListItem>Zaawansowany</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right">Hasło:</td>
                        <td>
                            <asp:TextBox ID="Password" TextMode="Password" Width="120" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">                                                                            
                            <p style="text-align: right"><asp:Button ID="Login" CommandName="Login" Text="Zaloguj" runat="server" /></p>
                            <p style="text-align: center"><asp:Literal ID="FailureText" runat="server"></asp:Literal></p>
                        </td>
                    </tr>
                </table>
            </LayoutTemplate>
        </asp:Login>
    </div>
    </form>
</body>
</html>
