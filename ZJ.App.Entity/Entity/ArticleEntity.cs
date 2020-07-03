using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Runtime.Serialization;
using ZJ.App.Common;
using System.Collections;

namespace ZJ.App.Entity
{
    public partial class ArticleEntity
    {


        #region JournalId
        ///<summary>
        ///
        ///</summary>
        public const string FieldName_JournalId = "JournalId";
        private int? _JournalId;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        [NoColumn]
        public int? JournalId
        {
            get { return _JournalId; }
            set
            {
                if (_IsQueryTemplate)
                {
                    this.RegisterQueryCondition(FieldName_JournalId, value);
                }

                _JournalId = value;
            }
        }

        #endregion

        #region JournalName
        ///<summary>
        ///
        ///</summary>
        public const string FieldName_JournalName = "JournalName";
        private string _JournalName;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        [NoColumn]
        public string JournalName
        {
            get { return _JournalName; }
            set
            {
                if (_IsQueryTemplate)
                {
                    this.RegisterQueryCondition(FieldName_JournalName, value);
                }

                _JournalName = value;
            }
        }

        #endregion

        //
        #region PropertyName
        ///<summary>
        ///
        ///</summary>
        public const string FieldName_PropertyName = "PropertyName";
        private string _PropertyName;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        [NoColumn]
        public string PropertyName
        {
            get { return _PropertyName; }
            set
            {
                if (_IsQueryTemplate)
                {
                    this.RegisterQueryCondition(FieldName_PropertyName, value);
                }

                _PropertyName = value;
            }
        }

        #endregion

        #region HrefTpl
        ///<summary>
        /// 文章静态URL模板
        ///</summary>
        public const string FieldName_HrefTpl = "HrefTpl";
        private string _HrefTpl;
        ///<summary>
        /// 文章静态URL模板
        ///</summary>
        [DataMember]
        [NoColumn]
        public string HrefTpl
        {
            get { return _HrefTpl; }
            set
            {
                if (_IsQueryTemplate)
                {
                    this.RegisterQueryCondition(FieldName_PropertyName, value);
                }

                _HrefTpl = value;
            }
        }

        #endregion

        #region Href
        ///<summary>
        ///
        ///</summary>
        public const string FieldName_Href = "Href";
        private string _Href;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        [NoColumn]
        public string Href
        {
            get
            {
                return string.Format(this.HrefTpl, this.NoOfCategory.ToString().PadLeft(2, '0'));
            }
            set
            {
                _Href = value;
            }
        }

        #endregion


        #region ImgFileList
        ///<summary>
        /// 文章的相关图片
        ///</summary>
        public const string FieldName_ImgFileName = "ImgFileList";
        private IList<MyImage> _ImgFileList;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        [NoColumn]
        public IList<MyImage> ImgFileList
        {
            get { return _ImgFileList; }
            set
            {
                if (_IsQueryTemplate)
                {
                    this.RegisterQueryCondition(FieldName_PropertyName, value);
                }

                _ImgFileList = value;
            }
        }

        #endregion


        #region ImgFileArray
        ///<summary>
        /// 文章的相关图片
        ///</summary>
        public const string FieldName_ImgFileArray = "ImgFileArray";
        private ArrayList _ImgFileArray;
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        [NoColumn]
        public ArrayList ImgFileArray
        {
            get { return _ImgFileArray; }
            set
            {
                if (_IsQueryTemplate)
                {
                    this.RegisterQueryCondition(FieldName_PropertyName, value);
                }

                _ImgFileArray = value;
            }
        }

        #endregion

        #region IsTopValue
        ///<summary>
        /// 是否首页显示 1-显示, 0-无效
        ///</summary>
        public const string FieldName_IsTopValue = "IsTopValue";
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        [NoColumn]
        public bool IsTopValue
        {
            get { return IsTop == 1; }
            set
            {
               IsTop = value ? 1 : 0;
            }
        }

        #endregion

        #region ArticleTitleForShow
        ///<summary>
        /// 在列表上显示的文章标题,如果有自定义显示别名优先显示文字别名
        ///</summary>
        public const string FieldName_ArticleTitleForShow = "ArticleTitleForShow";
        ///<summary>
        ///
        ///</summary>
        [DataMember]
        [NoColumn]
        public string ArticleTitleForShow
        {
            get
            {
                if (string.IsNullOrEmpty(ArticleTitleAlias))
                    return ArticleTitle;
                else
                    return ArticleTitleAlias;
            }
    
        }

        #endregion


        public class MyImage
        {
            public int ImageID { get; set; }
            public string ImgFileName { get; set; }
        }



        #region CategoryName
        ///<summary>
        /// 文章所属分类名
        ///</summary>
        public const string FieldName_CategoryName = "CategoryName";
        private string _CategoryName;
        ///<summary>
        /// 文章所属分类名
        ///</summary>
        [DataMember]
        [NoColumn]
        public string CategoryName
        {
            get { return _CategoryName; }
            set
            {
                if (_IsQueryTemplate)
                {
                    this.RegisterQueryCondition(FieldName_PropertyName, value);
                }

                _CategoryName = value;
            }
        }

        #endregion

    }
}
