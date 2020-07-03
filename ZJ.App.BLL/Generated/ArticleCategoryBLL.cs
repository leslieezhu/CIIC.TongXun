﻿/**************************************************************
 * This file is part of SMP Project
 * Copyright (C)2019 Microsoft
 * 
 * Author      : Generated by CodeSmith(DAL_v3.cst)
 * Mail        : 
 * Create Date : 2019/11/8 14:04:02
 * Summary     : this file was auto generated by tool . do not modify
 * 
 * 
 * Modified By : 
 * Date        : 
 * Mail        : 
 * Comment     :   
 * *************************************************************/
using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections.Generic;
using ZJ.App.Common;
using ZJ.App.Entity;
using ZJ.App.DAL;

namespace ZJ.App.BLL
{
    /// <summary>
    /// Data Access Layer class dbo.ArticleCategory.
    /// </summary>
    public partial class ArticleCategoryBLL : BllBase
    {
        
		public void AddArticleCategoryEntity(ArticleCategoryEntity entity)
        { 
            ArticleCategoryDAL dal = new ArticleCategoryDAL();
            dal.Insert(entity);
        }

        public void UpdateArticleCategoryEntity(ArticleCategoryEntity entity)
        { 
            ArticleCategoryDAL dal = new ArticleCategoryDAL();
            dal.Update(entity);
        }

        public void DisableArticleCategoryEntityById(object Id)
        { 
            ArticleCategoryDAL dal = new ArticleCategoryDAL();
            dal.Disabled(Id);
        }
        
        public void DeleteArticleCategoryEntityById(ArticleCategoryEntity entity)
        { 
            ArticleCategoryDAL dal = new ArticleCategoryDAL();
            dal.Delete(entity);
        }
           
        public ArticleCategoryEntity GetArticleCategoryEntityById(object Id)
        { 
            ArticleCategoryDAL dal = new ArticleCategoryDAL();
            return dal.Get(Id);
        }
        
        //返回全部
        public List<ArticleCategoryEntity> GetAllArticleCategory(List<SqlDbParameter> parms)
        { 
            ArticleCategoryDAL dal = new ArticleCategoryDAL();
            return dal.GetAll(parms);
        }
        
        //返回单实体对象
        public ArticleCategoryEntity GetArticleCategoryEntity(List<SqlDbParameter> parms)
        { 
            ArticleCategoryDAL dal = new ArticleCategoryDAL();
			return dal.Get(parms);
        }
        
        public List<ArticleCategoryEntity> GetAllArticleCategory(List<SqlDbParameter> parms, string orderBy)
        { 
            ArticleCategoryDAL dal = new ArticleCategoryDAL();
            return dal.GetAll(parms, orderBy);
        }
        
        public List<ArticleCategoryEntity> GetArticleCategoryPaged(List<SqlDbParameter> parms, string OrderBy, int PageSize, int PageIndex, out int RecordCount)
        { 
            ArticleCategoryDAL dal = new ArticleCategoryDAL();
            return dal.GetAll(parms, OrderBy, PageSize, PageIndex, out RecordCount);
        }
        
        
    }
}