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
	/// 表JournalArticleRelation数据实体
	/// </summary>
	[Serializable]
    [DataContract]
	[TableName("JournalArticleRelation")]
	public partial class JournalArticleRelationEntity : EntityBase
	{ 
		
		#region 构造函数
		///<summary>
		///
		///</summary>
		public JournalArticleRelationEntity():base()
		{
			
		}

        public JournalArticleRelationEntity(bool IsQueryTemplate) : base(IsQueryTemplate)
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
        [Column]
		public int? JournalId
		{
			get{return _JournalId;}
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
		#endregion
	}
}

