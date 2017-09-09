$(function () {
    $('#login-form-link').click(function (e) {
        $("#lblError2").hide();
        $("#login-form").delay(100).fadeIn(100);
        $("#register-form").fadeOut(100);
        $('#register-form-link').removeClass('active');
        $(this).addClass('active');
        e.preventDefault();
    });
    $('#register-form-link').click(function (e) {  
        $("#lblActivate").hide();
        $("#lblError").hide();
        $("#register-form").delay(100).fadeIn(100);
        $("#login-form").fadeOut(100);
        $('#login-form-link').removeClass('active');
        $(this).addClass('active');
        e.preventDefault();
    });
    $('#login').click(function (e) {
        if (!$.trim($("#txtUsername").val()) || !$.trim($("#txtPassword").val())) {
            $("#lblError").text("Enter Username and Password!");
            $("#lblError").show();
            e.preventDefault();
        }
    });
    $('#register').click(function (e) {
        if (!$.trim($("#txtRusername").val()) || !$.trim($("#txtRpassword").val()) || !$.trim($("#txtRemail").val()) || !$.trim($("#confirmpassword").val())) {
            $("#lblError2").text("Enter all fields!");
            $("#lblError2").show();
            e.preventDefault();
        }
        else if ($("#txtRpassword").val() !== $("#confirmpassword").val()) {
            $("#lblError2").text("Password doesnt match!");
            $("#lblError2").show();
            e.preventDefault();
        }
    });
});
/*function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}
$(document).ready(function () {
   
    if (getParameterByName("link") === "register") {
        $('#register-form-link').click();
        $("#lblError2").text(getParameterByName("message"));
        $("#lblError2").show();
    }
});*/

