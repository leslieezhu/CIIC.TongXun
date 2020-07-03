using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIIC.TongXun.Models
{
    /// <summary>
    /// 附件上传结果数据封装模型
    /// </summary>
    public class UploadFileResult
    {
        public string error;
        public string name;
        public string saveName;
        public string url;
        public string talentData;
        public string realFileName;
    }

    public class Img
    {
        public string name;
        public int size;
        public string url;
        public string thumbnailUrl;
        public string deleteType;
        public string deleteUrl;
    }
}