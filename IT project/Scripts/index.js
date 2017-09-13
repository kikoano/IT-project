$(function () {
    $(".myView").click(function () {
        $.ajax({
            type: "post",
            contentType: "application/json; charset=utf-8",
            url: "index.aspx/GetCalculationsData",
            data: "{'id':'" + $(this).attr("value") + "'}",
            dataType: "json",
            success: function (data) {
                $("#showData").html(data.d);
            }
        });
    });
    $(".myDelete").click(function () {
        $.ajax({
            type: "post",
            contentType: "application/json; charset=utf-8",
            url: "index.aspx/DeleteCalculationsData",
            data: "{'id':'" + $(this).attr("value") + "'}",
            dataType: "json",
            success: function () {
                location.reload();
            }
        });
    });
});
