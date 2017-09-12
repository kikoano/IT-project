function validate2() {
    if (!$.trim($("#productName").val()) || !$.trim($("#IznozDo").val()) || !$.trim($("#rokMeseciDo").val()) || !$.trim($("#kamStapka").val()) || $("#vidOtplata").val() == 0) {
        $("#lblError3").text("Внеси сите * полиња!");
        $("#lblError3").show();
        return false;
    }
    return true;
}