<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="własneSekcjeKonfigu.Default" %>

<%@ Import Namespace="własneSekcjeKonfigu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <%
                MobileSection mobile = System.Configuration.ConfigurationManager.GetSection("IsMobile/Mobile") as MobileSection;
            %>
            Baza danych: <%=mobile.Database.Name %>
        </div>
    </form>
</body>
</html>
