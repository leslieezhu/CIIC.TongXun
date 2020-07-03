var image_index = 1;
(function () {
    KindEditor.ready(function (K) {
        window.editor = K.create('#ArticleContent', {
            langType: 'zh-CN',
            autoHeightMode: true,
            uploadJson: '/Common/UploadJsonKindEditor/',
            allowFileManager: true,//浏览图片空间
            afterBlur: function () { this.sync(); }, //编辑器失去焦点(blur)时执行的回调函数（将编辑器的HTML数据同步到textarea）
            afterCreate: function () {  //KindEditor 生成后执行
                var self = this;
                K.ctrl(document, 13, function () {
                    self.sync();
                    K('form[name=form1]')[0].submit();
                });
                K.ctrl(self.edit.doc, 13, function () {
                    self.sync();
                    K('form[name=form1]')[0].submit();
                });
            }
        });

    });
    $("#ArticleCategory").on("change", function () { IniArticle(); });
    $("#btnSave").click(SaveArticle);
    
    uploadAttachment(); //@common.js
    //初始化上次文章类别
    if ($.cookie('ArticleCategory') != undefined) {
        $("#ArticleCategory").val($.cookie('ArticleCategory'));
    }

})(window);

$(document).ready(function () {
    IniArticle();
});

function SaveArticle() {
    var articleObj = GetFormInfo("article");
    articleObj.ImgFileList = [];
    $("#imgList input[type='hidden']").each(function () {
        var image = { "ImgFileName": $(this).attr("value") };
        articleObj.ImgFileList.push(image);
    });

    $.ajax({
        processData: false,
        url: "/Admin/Article/Create",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(articleObj),
        success: function (data) {
            var obj = JSON.parse(data);
            $.cookie('ArticleCategory', articleObj.CategoryId);

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

//初始化文章录入时序号和类别序号
function IniArticle() {
    var _data = {};
    _data.journalId = $("#JournalId").val();
    _data.categoryId = $("#ArticleCategory").val();

    $.ajax({
        url: "/Admin/Article/CreateInit",
        type: "POST",
        async: false,
        dataType: "json",
        data: _data,
        success: function (data) {
            $("#NoOfJournal").val(data.noOfJournal);
            $("#NoOfCategory").val(data.noOfCategory);
        },
        error: function () {
            //swal("操作失败!", "服务器错误", "error");
        }
    });
}

//上传图片 article_create.js  article_edit.js
function uploadAttachment() {
    /*jQuery-File-Upload*/
    var url = '/Common/UploadAttachment';
    $('#fileupload').fileupload({
        url: url,
        dataType: 'json',
        done: function (e, data) {
            if (data.result.result === true) {
                if (data.result.files.length > 0) {
                    var imgTpl = genrateTmp(image_index);
                    $("div.media").append(imgTpl);
                    var curIndex = $(imgTpl).attr("data-index");
                    var file = data.result.files[0];
                    $("#preview_img_" + curIndex)[0].src = file.url;
                    $('#save_name_' + curIndex).val(file.saveName);
                    $('#files_' + curIndex).text(file.name);
                    //$("#beforeUpload").removeClass('disabled');
                }
            }
            else {
                alert(data.result.Message);
            }
        }
    });
}

function genrateTmp(dataIndex) {
    var tpl = '<div class="media-body" data-index="{i}">'
        + '<input name = "save_name_{i}" type = "hidden" id = "save_name_{i}" value = "" data-model="article.ImgFileList" />'
        + '<div id="files_{i}" class="files"></div><a onclick="DelImage(this)">删除</a>'
        + '<a class="pull-left thumbnail">'
        + '<img id="preview_img_{i}" class="media-object" src="" onload="DrawImage(200, 200, this)">'
        + '</a>'
        + '</div >';
    tpl = tpl.replace(/{i}/g, dataIndex);
    ++image_index;
    return tpl;
}

function DelImage(obj)
{
    $(obj).parent().remove();
}