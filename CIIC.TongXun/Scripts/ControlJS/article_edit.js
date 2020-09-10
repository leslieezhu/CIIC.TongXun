var select;
var image_index = 1;

(function () {
    KindEditor.ready(function (K) {
        window.editor = K.create('#ArticleContent', {
            langType: 'zh-CN',
            autoHeightMode: false,
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

    bindArticleSelect();
    $("#btnSave").click(SaveArticle);

    //注册图片上传插件
    uploadAttachment(); 
})(window);

$(document).ready(function () {
    ArticleImagesRender();
});


function SaveArticle() {
    var articleObj = GetFormInfo("article");
    articleObj.CategoryId = select.find(':selected')[0].value; //select2取值
    //图片处理
    articleObj.ImgFileList = [];
    $("#imgList input[type='hidden']").each(function () {
        var image = {
            "ImgFileName": $(this).attr("value"),
            "Id": $(this).attr("data-id")
        };
        articleObj.ImgFileList.push(image);
    });
    $.ajax({
        processData: false,
        url: "/Admin/Article/Edit",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(articleObj),
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

function bindArticleSelect() {
    $.ajax({
        processData: false,
        url: "/Admin/Demo/GetCategory",
        type: "POST",
        contentType: "application/json",

        success: function (data) {
            select = $('.js-example-data-array').select2({
                placeholder: "Select ...",
                data: JSON.parse(data).data
            });
            select.val($("#ArticleCategory").val()).trigger('change');//赋值
        },
        error: function () {

        }
    });
}

/*** 上传图片 挂载点(function(){})(window); */
function uploadAttachment() {
    /*jQuery-File-Upload*/
    var url = '/Common/UploadAttachment';
    $('#fileupload').fileupload({
        url: url,
        dataType: 'json',
        done: function (e, data) {
            if (data.result.result === true) {
                if (data.result.files.length > 0) {
                    var imgTpl = genrateTmp(image_index);//TO Review
                    $("div.media").append(imgTpl);
                    var curIndex = $(imgTpl).attr("data-index");
                    var file = data.result.files[0];
                    $("#preview_img_" + curIndex)[0].src = file.url;
                    $('#save_name_' + curIndex).val(file.saveName);
                    $('#save_name_' + curIndex).attr("data-id",file.Id);
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
        + '<input name = "save_name_{i}" type = "hidden" id = "save_name_{i}" value = "" data-id="" data-model="article.ImgFileList" />'
        + '<div id="files_{i}" class="files"></div><a onclick="DelImage(this)">删除</a>'
        + '<a class="pull-left thumbnail">'
        + '<img id="preview_img_{i}" class="media-object" src="" onload="DrawImage(200, 200, this)">'
        + '</a>'
        + '</div >';
    tpl = tpl.replace(/{i}/g, dataIndex);
    ++image_index;
    return tpl;
}

function DelImage(obj) {
    var _data = {};
    var id = $("input[type='hidden']", $(obj).parent()).attr("data-id");
    _data.articleImageId = id;
    if (id !== "") {
        $.ajax({
            url: "/Admin/Article/DelImage",
            type: "POST",
            data: _data,
            error: function () {

            }
        });
    }
    $(obj).parent().remove();
}

function ArticleImagesRender() {
    var _data = {};
    _data.articleId = $("#Id").val();
    $.ajax({
        url: "/Admin/Article/GetImages",
        type: "POST",
        //contentType: "application/json",//区别 dataType: "json"
        data: _data,
        success: function (data) {
            //debugger
            var objArray = JSON.parse(data).data;
            if (objArray !== null) {
                for (var i = 0; i < objArray.length; i++) {
                    var imgTpl = genrateTmp(image_index);
                    $("div.media").append(imgTpl);
                    var curIndex = $(imgTpl).attr("data-index");
                    var file = objArray[i];
                    $("#preview_img_" + curIndex)[0].src = file.url;
                    $('#save_name_' + curIndex).val(file.saveName);
                    $('#save_name_' + curIndex).attr("data-id", file.Id);
                    $('#files_' + curIndex).text(file.name);
                }
                //$("#beforeUpload").removeClass('disabled');
            }
        },
        error: function () {

        }
    });
}
