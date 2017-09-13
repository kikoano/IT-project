<%@ page title="" language="C#" masterpagefile="~/MasterMenu.Master" autoeventwireup="true" codebehind="index.aspx.cs" inherits="IT_project.Index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Мои АМ планови
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="Content/index.css" rel="stylesheet" />
    <script src="Scripts/index.js"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="contentBody" runat="server">
    <div class="row">
        <div runat="server" id="viewCalc1">
        </div>
        <div id="showData">
        </div>
    </div>
</asp:Content>
