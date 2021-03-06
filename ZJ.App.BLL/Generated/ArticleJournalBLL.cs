﻿/**************************************************************
 * This file is part of SMP Project
 * Copyright (C)2019 Microsoft
 * 
 * Author      : Generated by CodeSmith(DAL_v3.cst)
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
    /// Data Access Layer class dbo.ArticleJournal.
    /// </summary>
    public partial class ArticleJournalBLL : BllBase
    {
        
		public void AddArticleJournalEntity(ArticleJournalEntity entity)
        { 
            ArticleJournalDAL dal = new ArticleJournalDAL();
            dal.Insert(entity);
        }

        public void UpdateArticleJournalEntity(ArticleJournalEntity entity)
        { 
            ArticleJournalDAL dal = new ArticleJournalDAL();
            dal.Update(entity);
        }

        public void DisableArticleJournalEntityById(object Id)
        { 
            ArticleJournalDAL dal = new ArticleJournalDAL();
            dal.Disabled(Id);
        }
        
        public void DeleteArticleJournalEntityById(ArticleJournalEntity entity)
        { 
            ArticleJournalDAL dal = new ArticleJournalDAL();
            dal.Delete(entity);
        }
           
        public ArticleJournalEntity GetArticleJournalEntityById(object Id)
        { 
            ArticleJournalDAL dal = new ArticleJournalDAL();
            return dal.Get(Id);
        }
        
        //返回全部
        public List<ArticleJournalEntity> GetAllArticleJournal(List<SqlDbParameter> parms)
        { 
            ArticleJournalDAL dal = new ArticleJournalDAL();
            return dal.GetAll(parms);
        }
        
        //返回单实体对象
        public ArticleJournalEntity GetArticleJournalEntity(List<SqlDbParameter> parms)
        { 
            ArticleJournalDAL dal = new ArticleJournalDAL();
			return dal.Get(parms);
        }
        
        public List<ArticleJournalEntity> GetAllArticleJournal(List<SqlDbParameter> parms, string orderBy)
        { 
            ArticleJournalDAL dal = new ArticleJournalDAL();
            return dal.GetAll(parms, orderBy);
        }
        
        public List<ArticleJournalEntity> GetArticleJournalPaged(List<SqlDbParameter> parms, string OrderBy, int PageSize, int PageIndex, out int RecordCount)
        { 
            ArticleJournalDAL dal = new ArticleJournalDAL();
            return dal.GetAll(parms, OrderBy, PageSize, PageIndex, out RecordCount);
        }
        
        
    }
}
