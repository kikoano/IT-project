<%@ page language="C#" autoeventwireup="true" codebehind="login.aspx.cs" inherits="IT_project.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta charset="UTF-8">
    <title>Login</title>
    <link rel="stylesheet" href="Content/bootstrap.css">
    <script src="Scripts/jquery-1.9.1.js"></script>
    <script src="Scripts/bootstrap.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="row">
                <div class="col-md-6 col-md-offset-3 ">
                    <div class="panel panel-login">
                        <div class="panel text-center panel-heading">
                            Login
           
                        </div>
                        <div>
                            <form method="post" action="/login" class="form-group">
                                <div class="form-group">
                                    <input type="text" class="form-control" placeholder="User Name">
                                </div>
                                <div class="form-group">
                                    <input type="password" class="form-control" placeholder="Your Password">
                                </div>
                                <div class="form-group">
                                    <div class="row">
                                        <div class="col-xs-6">
                                            <button class="btn btn-primary">Login</button>
                                            <button class="btn btn-secondary">Reset</button>
                                        </div>
                                        <div class="col-xs-6">
                                            <a href="#" class="pull-right">Register Now!</a>
                                        </div>
                                    </div>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
    </form>
</body>
</html>
