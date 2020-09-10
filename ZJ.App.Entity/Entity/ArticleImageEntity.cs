using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Runtime.Serialization;
using ZJ.App.Common;
using System.Collections;

namespace ZJ.App.Entity
{
    public partial class ArticleImageEntity
    {
        #region url
        ///<summary>
        /// URL模板
        ///</summary>
        public const string FieldName_urlTpl = "url";
        private string _url;
        ///<summary>
        /// 图片URL
        ///</summary>
        [DataMember]
        [NoColumn]
        public string url
        {
            get { return _url; }
            set
            {
                _url = value;
            }
        }
        #endregion

        #region saveName
        ///<summary>
        /// 保存在表中的图片名
        ///</summary>
        public const string FieldName_saveName = "saveName";
        private string _saveName;
        ///<summary>
        /// 保存在表中的图片名
        ///</summary>
        [DataMember]
        [NoColumn]
        public string saveName
        {
            get { return _saveName; }
            set
            {
                _saveName = value;
            }
        }
        #endregion


        #region name
        ///<summary>
        /// 图片名随机名,用图片服务端上传时的临时文件名
        ///</summary>
        public const string FieldName_name = "name";
        private string _name;
        ///<summary>
        /// 图片名随机名,用图片服务端上传时的临时文件名
        ///</summary>
        [DataMember]
        [NoColumn]
        public string name
        {
            get { return _name; }
            set
            {
                _name = value;
            }
        }
        #endregion
    }
}
