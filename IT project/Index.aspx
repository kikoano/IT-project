<%@ page title="" language="C#" masterpagefile="~/MasterMenu.Master" autoeventwireup="true" codebehind="index.aspx.cs" inherits="IT_project.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentBody" runat="server">
    <div class="row">
        Welcome
        <asp:LoginName ID="LoginName1" runat="server" Font-Bold="true" />
    </div>
</asp:Content>
