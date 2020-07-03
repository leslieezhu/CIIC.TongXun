(function () {
    bindArticleList();
    $("#btnSearch").click(bindArticleList);
})(window);

function bindArticleList()
{
    var search = GetFormInfo("article");

    var tableData = {
        "paging": true,
        "lengthChange": false,
        "autoWidth": false,
        "ordering": false,
        "serverSide": true,
        "pageLength": 15,
        "destroy": true,
        "searching": false,
        "info": true,
        "ajax": {
            "url": "/Admin/Article/GetList",
            "type": "POST",
            "data": search
        }
    };
    tableData.columns = [
        { "data": "NoOfJournal", title: "ID", width: "2%" },
        { "data": "NoOfCategory", title: "类别序号",width: "7%"},
        { "data": "ArticleTitle", title: "文章名", class:"wordwrap ellipsis" },
        { "data": "CategoryName", title: "类别", width: "9%" },
        { "data": "CreateTime", title: "录入时间", width: "7%" },
        { "data": "", title: "操作", width: "7%" }
    ];
    tableData.columnDefs = [
        {
            "render": function (data, type, row) {
                if (data !== null) {
                    return (new Date(data)).Format("yyyy-MM-dd hh:mm");
                }
                return null;
            },
            "targets": -2
        },
        {
            "render": function (data, type, row) {
                var tmp = '<a href="/Admin/Article/Edit/' + row["Id"] + '" title="编辑" ><i class="fa fa-edit" ></i></a>';
                tmp += '<a href="/Admin/Article/Perview/' + row["Id"] + '" title="预览" target="_blank"><i class="fa fa-search" ></i></a>';
                tmp += '<a href="/Admin/Article/StaticHtml/' + row["Id"] + '" title="生成静态" target="_blank"><i class="fa fa-files-o" ></i></a>';
                return tmp;
            },
            "targets": -1
        }
    ];
    var table = $('#articleList').DataTable(tableData);

}

function createArticle(journalId) {
    location.href = "/Admin/Article/Create?JournalId=" + journalId;
}