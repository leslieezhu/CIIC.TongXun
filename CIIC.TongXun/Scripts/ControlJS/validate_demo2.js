// only for demo purposes
//validation插件,当表单验证成功并提交时执行，存在此方法时,表单只能在此方法内部执行form.submit()才能提交，可理解成它替代了表单的onsubmit方法
$.validator.setDefaults({
    submitHandler: function () {
        alert("submitted! (skipping validation for cancel button)");
    }
});

$().ready(function () {
    $("#form1").validate({
        errorLabelContainer: $("#form1 div.error")//设置显示错误的信息的div
    });

    //form2
    var container = $('div.container');
    // validate the form when it is submitted
    var validator = $("#form2").validate({
        errorContainer: container,
        errorLabelContainer: $("ol", container),
        wrapper: 'li'
    });

    $(".cancel").click(function () {
        validator.resetForm();
    });

    //myForm  jquery.form
    // (jquery.form.js API) bind 'myForm' and provide a simple callback function, 绑定当表单正常提交后,即返回200时,执行的回调函数,正在执行ajax提交是执行,ajaxSubmit()
    $('#myForm').ajaxForm(function () {
        alert("Thank you for your comment!");
    });
});