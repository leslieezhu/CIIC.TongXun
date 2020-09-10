(function () {
    $("#btnSearch").click(bindArticleList);
    
})(window);


var select;

$(document).ready(function () {
    //$("#sel_menu2").select2({
    //    tags: true, //是否可以定义tag
    //    maximumSelectionLength: 1  //最多能够选择的个数
    //});
    //bindArticleList();
    bindArticleSelect();
    //赋值demo

    //var test2 = $('.js-example-data-array').select2("val");
    //var data = [{
    //    id: 'all',
    //    text: 'All',
    //    children: [{
    //        id: 'item1',
    //        text: 'item1'
    //    }, {
    //        id: 'item2',
    //        text: 'item2'
    //    }]
    //}];
    //var FRUIT_GROUPS = [
    //    {
    //        id: '1',
    //        text: 'Citrus',
    //        children: [
    //            { id: 'c1', text: 'Grapefruit' },
    //            { id: 'c2', text: 'Orange' },
    //            { id: 'c3', text: 'Lemon' },
    //            { id: 'c4', text: 'Lime' }]
    //    },
    //    {
    //        id: '2',
    //        text: 'Other',
    //        children: [
    //            { id: 'o1', text: 'Apple' },
    //            { id: 'o2', text: 'Mango' },
    //            { id: 'o3', text: 'Banana' }]
    //    },
    //    {
    //        id: '3',
    //        text:'Demo'
    //    }
    //];

    ////$('.js-example-data-array').select2({ data: data });
    //$('.js-example-data-array').select2({
    //    multiple: true,
    //    placeholder: "Select ...",
    //    data: FRUIT_GROUPS,
    //    query: function (options) {
    //        var selectedIds = options.element.select2('val');
    //        var selectableGroups = $.map(this.data, function (group) {
    //            var areChildrenAllSelected = true;
    //            $.each(group.children, function (i, child) {
    //                if (selectedIds.indexOf(child.id) < 0) {
    //                    areChildrenAllSelected = false;
    //                    return false; // Short-circuit $.each()
    //                }
    //            });
    //            return !areChildrenAllSelected ? group : null;
    //        });
    //        options.callback({ results: selectableGroups });
    //    }
    //}).on('select2-selecting', function (e) {
    //    var $select = $(this);
    //    if (e.val == '') { // Assume only groups have an empty id
    //        e.preventDefault();
    //        $select.select2('data', $select.select2('data').concat(e.choice.children));
    //        $select.select2('close');
    //    }
    //});

});

$.validator.setDefaults({
    submitHandler: function () {
        alert("submitted!");
    }
});

$().ready(function () {
    // validate the comment form when it is submitted
    $("#commentForm").validate();

    // validate signup form on keyup and submit
    $("#signupForm").validate({
        rules: {
            firstname: "required",
            lastname: "required",
            username: {
                required: true,
                minlength: 2
            },
            password: {
                required: true,
                minlength: 5
            },
            confirm_password: {
                required: true,
                minlength: 5,
                equalTo: "#password"
            },
            email: {
                required: true,
                email: true
            },
            topic: {
                required: "#newsletter:checked",
                minlength: 2
            },
            agree: "required"
        },
        messages: {
            firstname: "Please enter your firstname",
            lastname: "Please enter your lastname",
            username: {
                required: "Please enter a username",
                minlength: "Your username must consist of at least 2 characters"
            },
            password: {
                required: "Please provide a password",
                minlength: "Your password must be at least 5 characters long"
            },
            confirm_password: {
                required: "Please provide a password",
                minlength: "Your password must be at least 5 characters long",
                equalTo: "Please enter the same password as above"
            },
            email: "Please enter a valid email address",
            agree: "Please accept our policy",
            topic: "Please select at least 2 topics"
        }
    });

    // propose username by combining first- and lastname
    $("#username").focus(function () {
        var firstname = $("#firstname").val();
        var lastname = $("#lastname").val();
        if (firstname && lastname && !this.value) {
            this.value = firstname + "." + lastname;
        }
    });

    //code to hide topic selection, disable for demo
    var newsletter = $("#newsletter");
    // newsletter topics are optional, hide at first
    var inital = newsletter.is(":checked");
    var topics = $("#newsletter_topics")[inital ? "removeClass" : "addClass"]("gray");
    var topicInputs = topics.find("input").attr("disabled", !inital);
    // show when newsletter is checked
    newsletter.click(function () {
        topics[this.checked ? "removeClass" : "addClass"]("gray");
        topicInputs.attr("disabled", !this.checked);
    });
});

//END Validate Demo

function bindArticleSelect()
{
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


function bindArticleList() {
    //var search = GetFormInfo("article");
    var selectVal = $('.js-example-data-array').select2('data')[0].id;//select2取值 方式一
    var selectVal2 = $('.js-example-data-array').find(':selected')[0].value;//select2取值 方式二

    $('.js-example-data-array').select2().val(2).trigger('change');//赋值

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