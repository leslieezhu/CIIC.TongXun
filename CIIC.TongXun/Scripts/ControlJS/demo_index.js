(function () {
    $("#btnSearch").click(bindArticleList);
})(window);

$(document).ready(function () {
    bindArticleList();
});

function bindArticleList() {
    //var search = GetFormInfo("article");
    var search = {};
    var tableData = {
        paging : true,
        lengthChange : false,
        autoWidth : false,
        ordering : false,
        serverSide : true,
        pageLength : 15,
        destroy : true,
        searching : false,
        info: true,
        columns: [
            { data: "" },
            { data: "NoOfJournal", title: "ID"},
            { data: "NoOfCategory", title: "类序", width: "7%"},
            { data: "ArticleTitle", title: "文章名"},
            { data: "CategoryName", title: "类别" },
            { data: "CreateTime", title: "录入时间"},
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
            url: "/Admin/Article/GetList",
            type: "POST",
            data: search
        }
    };

    tableData.columnDefs.push( 
        {
            render: function (data, type, row) {
                var tmp = '<a href="/Admin/Article/Edit/' + row["Id"] + '" title="编辑" ><i class="fa fa-edit" ></i></a>';
                tmp += '<a href="/Admin/Article/Perview/' + row["Id"] + '" title="预览" target="_blank"><i class="fa fa-search" ></i></a>';
                tmp += '<a href="/Admin/Article/StaticHtml/' + row["Id"] + '" title="生成静态" target="_blank"><i class="fa fa-files-o" ></i></a>';
                return tmp;
            },
            targets : -1
        }
    );
    var table = $('#articleList').DataTable(tableData);

}

function createArticle(journalId) {
    location.href = "/Admin/Article/Create?JournalId=" + journalId;
}