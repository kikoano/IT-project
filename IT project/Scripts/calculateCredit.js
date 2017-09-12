function validate2() {
    if ( !$.trim($("#IznozDo").val()) || !$.trim($("#rokMeseciDo").val())) {
        $("#lblError3").text("Внеси сите * полиња!");
        $("#lblError3").show();
        return false;
    }
    return true;
}