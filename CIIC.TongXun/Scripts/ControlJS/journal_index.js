(function () {
    $("#btnSearch").click(bindArticleList);
})(window);

$(document).ready(function () {
    bindArticleList();
});

function bindArticleList()
{
    //var search = GetFormInfo("article");
    var search = {};
    var tableData = {
        paging: true,
        lengthChange: false,
        autoWidth: false,
        ordering: false,
        serverSide: true,
        pageLength: 15,
        destroy: true,
        searching: false,
        info: true,
        columns: [
            { data: "", width: "2%" },
            { data: "JournalId", title: "ID", width: "2%" },
            { data: "JournalName", title: "期刊名" },
            { data: "PropertyName", title: "总期数" },
            { data: "", title: "操作", width: "7%" }
        ],
        columnDefs: [{
            targets: 0,
            defaultContent: '',
            orderable: false,
            className: 'select-checkbox'
        }],
        select: {
            style: 'multi',
            selector: 'td:first-child'
        },
        ajax: {
            url: "/Admin/Journal/GetList",
            type: "POST",
            data: search
        }
    };

    tableData.columnDefs.push(
        {
            render: function (data, type, row) {
                var tmp = '<a href="/Admin/Journal/Edit/' + row["JournalId"] + '" title="编辑" ><i class="fa fa-edit" ></i></a>';
                tmp += '<a href="/Admin/Article/Index?JournalId=' + row["JournalId"] + '" title="查看文章" target="_blank" ><i class="fa fa-search" ></i></a>';
                tmp += '<a href="/Admin/Journal/StaticHtml?JournalId=' + row["JournalId"] + '" title="生成静态" ><i class="fa fa-files-o" ></i></a>';
                return tmp;
            },
            targets: -1
        }
    );  

    var table = $('#journalList').DataTable(tableData);
    table.columns.adjust().draw();
    $('#journalList').on('click', 'th.select-checkbox', function () {
        $(this).parent().toggleClass('checked');

        if ($(this).parent().is(".checked")) {
            table.rows({ page: 'current' }).select();
        } else {
            table.rows({ page: 'current' }).deselect();
        }
    });

}