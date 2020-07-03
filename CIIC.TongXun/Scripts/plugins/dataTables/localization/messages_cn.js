var oLanguage = {
    "oAria": {
        "sSortAscending": ": 升序排列",
        "sSortDescending": ": 降序排列"
    },
    "oPaginate": {
        "sFirst": "首页",
        "sLast": "末页",
        "sNext": "下页",
        "sPrevious": "上页"
    },
    "sEmptyTable": "没有相关记录",
    "sInfo": "第 _START_ 到 _END_ 条记录，共 _TOTAL_ 条",
    "sInfoEmpty": "第 0 到 0 条记录，共 0 条",
    "sInfoFiltered": "(从 _MAX_ 条记录中检索)",
    "sInfoPostFix": "",
    "sDecimal": "",
    "sThousands": ",",
    "sLengthMenu": "每页显示条数: _MENU_",
    "sLoadingRecords": "正在载入...",
    "sProcessing": "正在载入...",
    "sSearch": "搜索:",
    "sSearchPlaceholder": "",
    "sUrl": "",
    "sZeroRecords": "没有相关记录"
}
//$.fn.dataTable.defaults.oLanguage=oLanguage;
$.extend($.fn.dataTable.defaults.oLanguage, oLanguage);

function iniDataTable(tableData) {
    if (tableData !== null) {
        tableData.lengthChange = false;
        tableData.searching = false;
        tableData.ordering = false;
        tableData.info = true;
        tableData.autoWidth = false;
        tableData.pageLength = 15;
        tableData.serverSide = true;
        tableData.destroy = true;
    }
}

//return (new Date(data)).Format("yyyy-MM-dd hh:mm:ss"); 
Date.prototype.Format = function (fmt) {
    var o = {
        "M+": this.getMonth() + 1,
        //月份

        "d+": this.getDate(),
        //日

        "h+": this.getHours(),
        //小时

        "m+": this.getMinutes(),
        //分

        "s+": this.getSeconds(),
        //秒

        "q+": Math.floor((this.getMonth() + 3) / 3),
        //季度

        "S": this.getMilliseconds() //毫秒

    };
    if (/(y+)/.test(fmt)) {
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    }
    for (var k in o) {
        if (new RegExp("(" + k + ")").test(fmt)) {
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
        }
    }
    return fmt;
}

