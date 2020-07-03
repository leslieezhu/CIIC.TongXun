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

    $("#btnSave").click(SaveArticle);

})(window);

function SaveArticle() {
    var articleObj = GetFormInfo("article");
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