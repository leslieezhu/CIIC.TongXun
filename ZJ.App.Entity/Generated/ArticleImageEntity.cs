﻿/**************************************************************
 * This file is part of  Project  
 * Copyright (C)2019 Microsoft
 * 
 * Author      : Generated by CodeSmith(Entity_v2.cst)
 * Mail        : 
 * Create Date : 2019/11/7 10:01:57
 * Summary     : this file was auto generated by tool . do not modify
 * 
 * 
 * Modified By : 
 * Date        : 
 * Mail        :
 * Comment     :   
 * *************************************************************/

using System; 
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Runtime.Serialization;
using ZJ.App.Common;

namespace ZJ.App.Entity
{
	/// <summary>
	/// 表ArticleImage数据实体
	/// </summary>
	[Serializable]
    [DataContract]
	[TableName("ArticleImage")]
	public partial class ArticleImageEntity : EntityBase
	{ 
		
		#region 构造函数
		///<summary>
		///
		///</summary>
		public ArticleImageEntity():base()
		{
			
		}

        public ArticleImageEntity(bool IsQueryTemplate) : base(IsQueryTemplate)
        {

        }
		 
		#endregion 
		
		#region 公共属性
        #region Id
        ///<summary>
        ///
        ///</summary>
        public const string FieldName_Id = "Id";
        private int _Id;
		///<summary>
		///
		///</summary>
        [DataMember]
		[PrimaryKey]
		public int Id
		{
			get{return _Id;}
            set
            {
                if (_IsQueryTemplate)
                {
                    this.RegisterQueryCondition(FieldName_Id, value);
                }
                
                _Id = value;
            }
		}
        
	    #endregion

        #region ArticleId
        ///<summary>
        ///
        ///</summary>
        public const string FieldName_ArticleId = "ArticleId";
        private int? _ArticleId;
		///<summary>
		///
		///</summary>
        [DataMember]
        [Column]
		public int? ArticleId
		{
			get{return _ArticleId;}
            set
            {
                if (_IsQueryTemplate)
                {
                    this.RegisterQueryCondition(FieldName_ArticleId, value);
                }
                
                _ArticleId = value;
            }
		}
        
	    #endregion

        #region ImageFileName
        ///<summary>
        ///
        ///</summary>
        public const string FieldName_ImageFileName = "ImageFileName";
        private string _ImageFileName;
		///<summary>
		///
		///</summary>
        [DataMember]
        [Column]
		public string ImageFileName
		{
			get{return _ImageFileName;}
            set
            {
                if (_IsQueryTemplate)
                {
                    this.RegisterQueryCondition(FieldName_ImageFileName, value);
                }
                
                _ImageFileName = value;
            }
		}
        
	    #endregion
		#endregion
	}
}

