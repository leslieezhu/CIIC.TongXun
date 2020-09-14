var table;

$(document).ready(function () {
    var search = {};
    table = $('#example').DataTable({
        paging: false,
        lengthChange: false,
        autoWidth: false,
        ordering: false,
        serverSide: true,
        destroy: true,
        searching: false,
        info: false,
        responsive: true,
        ajax: {
            "url": "/Admin/Category/GetList",
            "type": "POST",
            "data": search
        },
        columns: [
            { data: 'Id' },
            { data: 'PCategoryName' },
            { data: 'CategoryName' },
            { data: 'Order' }
        ],
        columnDefs: [
            { orderable: false, targets: [1, 2, 3] }
        ],
        rowReorder: {
            dataSrc: 'Order'
        },
        select: true

    });

    table.on('select', function (e, dt, type, indexes) {
        if (type === 'row') {
            var currentRowData = table.rows(indexes, { order: 'index' }).data();
            $("#categoryId").html(currentRowData[0].Id);
            $("#pcategoryName").html(currentRowData[0].PCategoryName);
            $("#hdCategoryId").val(currentRowData[0].Id);
            $("#category").val(currentRowData[0].CategoryName);
            $("#order").val(currentRowData[0].Order);
            $("#form div.error").html("");
        }
    });

    var validator = $("#signupForm").validate({
        rules: {
            category: "required",
            order: {
                required: true,
                number:true
            }        
        },
        //messages: {
        //    category: "必填",
        //    order: {
        //        required: "必填",
        //        number: "无效输入"
        //    }
        //},
        errorLabelContainer: $("#form div.error"),
        submitHandler: function (form) {
            save();
        }
    });

    //$("#btnCreate").click(save);
});

function save() {
    var categoryObj = GetFormInfo("category");
    categoryObj.Id = $("#hdCategoryId").val();
    var urlTarget = "/Admin/Category/Edit";

    $.ajax({
        //processData: false,//可选项
        url: urlTarget,
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(categoryObj),//必须设置
        success: function (data) {
            $("#categoryId").html("");
            $("#pcategoryName").html("");
            $("#hdCategoryId").val("");
            $("#category").val("");
            $("#order").val("");

            var obj = JSON.parse(data);
            table.ajax.reload();
            //location.href = obj.returnUrl;
        },
        error: function () {
            //swal("操作失败!", "服务器错误", "error");
        }
    });
}