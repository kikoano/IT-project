$(function () {
    $("#modalChange").click(function () {
        $('#myModal').modal('show');
        $("#lblError").hide();
    });
    var pathname = window.location.pathname;
    $('.nav > li > a[href="' + pathname.substr(1) + '"]').parent().addClass('active');

    var docHeight = $(window).height();
    var footerHeight = $('#footer').height();
    var footerTop = $('#footer').position().top + footerHeight;

    if (footerTop < docHeight) {
        $('#footer').css('margin-top', 10 + (docHeight - footerTop) + 'px');
    }
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