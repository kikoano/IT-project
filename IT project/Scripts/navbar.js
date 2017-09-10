$(function () {
    $("#modalChange").click(function () {
        $('.modal').modal('show');
        $("#lblError").hide();
    });
});
function validatePage() {
    if (!$.trim($("#password1").val()) || !$.trim($("#password2").val())) {
        $("#lblError").text("Enter all fields!");
        $("#lblError").show();
        return false;
    }
    else if ($("#password1").val() !== $("#password2").val()) {
        $("#lblError").text("Password doesnt match!");
        $("#lblError").show();
        return false;
    }
    return true;
}