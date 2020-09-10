var select;
(function () {
    bindArticleSelect();
    $("#btnSearch").click(bindArticleList);
})(window);


$(document).ready(function () {
    bindArticleList();
    //$("div.titleDesc").dotdotdot({});
});

function bindArticleList()
{
    var search = GetFormInfo("article");
    if (select !== undefined)
    {
        search.CategoryId = select.find(':selected')[0].value; 
    }

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
       responsive: true,
       select: true,
       ajax: {
            "url": "/Admin/Article/GetList",
            "type": "POST",
            "data": search
        }
    };
    tableData.columns = [
        { "data": "NoOfJournal", title: "ID", "class": "td-id" },
        { "data": "NoOfCategory", title: "类别序号", "class": "td-category-no"},
        { "data": "ArticleTitle", title: "文章名" ,"class":"td-title"},
        { "data": "CategoryName", title: "类别", "class": "td-category" },
        { "data": "CreateTime", title: "录入时间", "class": "td-time" },
        { "data": "", title: "操作", "class": "td-op" }
    ];
    tableData.columnDefs = [
        {
            "render": function (data, type, row) {
                var tmp = '<div class="titleDesc">' + data + '</div>';
                return tmp;
            },
            "targets": -4
        },
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
  
    table.columns.adjust().draw();
}

function bindArticleSelect() {
    $.ajax({
        processData: false,
        url: "/Admin/Demo/GetCategory",
        type: "POST",
        contentType: "application/json",

        success: function (data) {
            select = $('.js-example-data-array').select2({
                //tags: false,
                //multiple: true,
                placeholder: "Select ...",
                data: JSON.parse(data).data
            });
        },
        error: function () {

        }
    });
}

function createArticle(journalId) {
    location.href = "/Admin/Article/Create?JournalId=" + journalId;
}

