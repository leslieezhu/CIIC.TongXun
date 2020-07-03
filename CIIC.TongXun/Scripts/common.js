var datetimepickerYearCfg = {
    language: 'zh-CN',
    startView: 4,//日期时间选择器打开之后首先显示的视图。 可接受的值：0 - 小时 视图，1 - 天 视图，2 - 月 视图，3 - 年 视图，4 - 十年 视图
    minView: 4,//Number, String. 默认值：0, ‘hour’，日期时间选择器所能够提供的最精确的时间选择视图
    autoclose: 1,//当选择一个日期之后是否立即关闭此日期时间选择器
    format: 'yyyy',//日期显示格式
    linkFormat: "yyyy"
}; 
//Image等比例缩放工具
var flag = false;
function DrawImage(iwidth, iheight, ImgD) {
    var image = new Image();
    image.src = ImgD.src;
    if (image.width > 0 && image.height > 0) {
        flag = true;
        //原图宽除高的值和定义宽除高的值的比较
        if (image.width / image.height >= iwidth / iheight) {
            // 原图的宽大于定义的宽
            if (image.width > iwidth) {
                ImgD.width = iwidth;
                //定义的宽度和原图的宽度的比例，同比缩小高度
                ImgD.height = (image.height * iwidth) / image.width;
            } else {
                ImgD.width = image.width;
                ImgD.height = image.height;
            }
            ImgD.alt = image.width + "×" + image.height;
        }
        else {
            // 原图的高大于定义的高
            if (image.height > iheight) {
                ImgD.height = iheight;
                //定义的高度和原图的高度的比例，同比缩小宽度
                ImgD.width = (image.width * iheight) / image.height;
            } else {
                ImgD.width = image.width;
                ImgD.height = image.height;
            }
            ImgD.alt = image.width + "×" + image.height;
        }
    }
}

//HtmlEncode和HtmlDecode转换工具
var HtmlUtil = {
    htmlEncode: function (html) {
        var temp = document.createElement("div");
        (temp.textContent != undefined) ? (temp.textContent = html) : (temp.innerText = html);
        var output = temp.innerHTML;
        temp = null;
        return output;
    },
    htmlDecode: function (text) {
        var temp = document.createElement("div");
        temp.innerHTML = text;
        var output = temp.innerText || temp.textContent;
        temp = null;
        return output;
    }
};


//获取url中的参数
function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
    var r = window.location.search.substr(1).match(reg);  //匹配目标参数
    if (r != null) return unescape(r[2]); return null; //返回参数值
}

function GetFormInfo(Type, IsGetEmpty, Filter) {
    if (IsGetEmpty == null) {
        IsGetEmpty = true;
    }
    var _filter = IsNull(Filter, "") == "" ? "" : (Filter + " ");
    var result = {};
    var temp_model = null;
    var temp_value = null;
    var temp_rc_list = "";
    $(_filter + "*[data-model^='" + Type + "']").each(function (index, item) {
        temp_model = item.attributes["data-model"].value.replace(Type + ".", "");
        temp_value = "";
        switch (item.tagName) {
            case "SELECT":
                temp_value = escape(item.value);
                break;
            case "TEXTAREA":
                var textarea_txt = item.value === '' ? item.innerText : item.value;
                temp_value = escape(textarea_txt);
                break;
            case "INPUT":
                switch (item.type) {
                    case "text":
                    case "hidden":
                        temp_value = escape(item.value.HTMLEncode());
                        break;
                    case "checkbox":
                        if (item.name != "") {
                            if (temp_rc_list.indexOf("[" + item.name + "]") == -1) {
                                temp_rc_list += "[" + item.name + "]";
                                if ($("input[type='checkbox'][name='" + item.name + "']:checked").length > 0) {
                                    var checkboxItem = $("input[type='checkbox'][name='" + item.name + "']:checked").length;
                                    if (checkboxItem == 1) {
                                        temp_value = item.value; //单选
                                    }
                                    else {
                                        $("input[type='checkbox'][name='" + item.name + "']:checked").each(function (index, item) {
                                            temp_value += item.value + ",";
                                        });
                                        temp_value = temp_value.TrimEnd(",");
                                    }
                                }
                                else {
                                    temp_value = false;
                                }
                            }
                            else {
                                return true;
                            }
                        }
                        else {
                            temp_value = item.checked ? "true" : "false";
                        }
                        break;
                    case "radio":
                        if (item.name != "") {
                            if (temp_rc_list.indexOf("[" + item.name + "]") == -1) {
                                temp_rc_list += "[" + item.name + "]";
                                temp_value = $("input[type='radio'][name='" + item.name + "']:checked").val();
                                if (typeof (temp_value) === "undefined") {
                                    temp_value = "";
                                }
                            }
                            else {
                                return true;
                            }
                        }
                        else {
                            temp_value = item.checked ? "true" : "false";
                        }
                        break;
                }
                break;
        }
        if (temp_value != "" || (temp_value == "" && IsGetEmpty == true)) {
            result[temp_model] = temp_value;
        }
    });
    return result;
}

function SetFormInfo(Type, Data, Filter) {
    if (typeof (Data) === "string" && Data != "") {
        Data = JSON.parse(Data);
    }
    if (Data == null) {
        Data = {};
    }
    var _filter = IsNull(Filter, "") == "" ? "" : (Filter + " ");
    var temp_rc_list = "";
    var temp_value = null;
    $(_filter + "*[data-model^='" + Type + "']").each(function (index, item) {
        temp_model = item.attributes["data-model"].value.replace(Type + ".", "");
        if (Data[temp_model] == null) {
            Data[temp_model] = "";
        }
        temp_value = "";
        switch (item.tagName) {
            case "SELECT":
                $(item).val(Data[temp_model]);
                break;
            case "TEXTAREA":
                item.innerText = Data[temp_model];
                break;
            case "INPUT":
                switch (item.type) {
                    case "text":
                    case "hidden":
                        item.value = Data[temp_model];
                        break;
                    case "checkbox":
                        var checkedVal = false;
                        if (Data[temp_model].toString().toLowerCase() == "true" || Data[temp_model].toString().toLowerCase() == "1") {
                            checkedVal = true;
                        }
                        item.checked = checkedVal;
                        break;
                }
                break;
        }
    });
}

function IsNull(WhenInfo, ThenInfo) {
    if (WhenInfo == null) {
        return ThenInfo == null ? "" : ThenInfo;
    }
    return WhenInfo;
}

String.prototype.HTMLEncode = function () {
    if (this == null || this == "") {
        return this;
    }
    var temp = document.createElement("div");
    (temp.textContent != undefined) ? (temp.textContent = this) : (temp.innerText = this);
    var output = temp.innerHTML;
    temp = null;
    return output
    //return this.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;").replace(/\'/g, "&#39;").replace(/\"/g, "&quot;");
}


