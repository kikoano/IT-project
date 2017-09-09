<%@ page language="C#" autoeventwireup="true" codebehind="home.aspx.cs" inherits="IT_project.Home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            Welcome
    <asp:LoginName ID="LoginName1" runat="server" Font-Bold="true" />
            <br />
            <br />
            <asp:LoginStatus ID="LoginStatus1" runat="server" />
        </div>
    </form>
</body>
</html>
