(function () {

    $("#btnSave").click(SaveJournal);

})(window);

function SaveJournal() {
    var journalObj = GetFormInfo("journal");

    $.ajax({
        processData: false,
        url: "/Admin/Journal/Edit",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(journalObj),
        success: function (data) {
            var obj = JSON.parse(data);
            if (obj.result == false) {
                alert(obj.message);
                //swal("操作失败!", obj.message, "error");
                return;
            }
            location.href = obj.returnUrl;
        },
        error: function () {
            //swal("操作失败!", "服务器错误", "error");
        }
    });

}