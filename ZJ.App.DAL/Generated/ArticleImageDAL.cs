﻿/**************************************************************
 * This file is part of SMP Project
 * Copyright (C)2020 Microsoft
 * 
 * Author      : Generated by CodeSmith(DAL_v3.cst)
 * Mail        : 
 * Create Date : 2020/9/3 15:18:41
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

namespace ZJ.App.DAL
{
    /// <summary>
    /// Data Access Layer class dbo.ArticleImage.
    /// </summary>
    public partial class ArticleImageDAL : DalBase<ArticleImageEntity>
    {
        #region 构造函数
        
		public ArticleImageDAL(): base()
        { }

        public ArticleImageDAL(string DbName): base(DbName)
        { }

        public ArticleImageDAL(DbTransaction tran): base(tran)
        { }
        
        #endregion
        
        #region public method
        
        public void BulkInsert(List<ArticleImageEntity> list)
        {
            foreach (ArticleImageEntity item in list)
            {
                this.Insert(item);
            }
            /*
            SqlBulkCopy bulkCopy;
            if (System.Transactions.Transaction.Current != null)
            {
                using (System.Transactions.TransactionScope t = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Suppress))
                {
                    bulkCopy = new SqlBulkCopy(CurrentDatabase.ConnectionString, SqlBulkCopyOptions.CheckConstraints);
                    bulkCopy.BulkCopyTimeout = 360;
                    bulkCopy.DestinationTableName = "ArticleImage";
                    DataTable dataTable =ConvertToDataTable(list);
                    bulkCopy.ColumnMappings.Add("Id", "Id");
                    bulkCopy.ColumnMappings.Add("ArticleId", "ArticleId");
                    bulkCopy.ColumnMappings.Add("ImageFileName", "ImageFileName");
                    bulkCopy.ColumnMappings.Add("IsDelete", "IsDelete");
                    
                    bulkCopy.WriteToServer(dataTable);
                    t.Complete();
                    bulkCopy.Close();
                }
            }
            else
            {
                bulkCopy = new SqlBulkCopy(CurrentDatabase.ConnectionString, SqlBulkCopyOptions.CheckConstraints);
                bulkCopy.BulkCopyTimeout = 360;
                bulkCopy.DestinationTableName = "ArticleImage";
                DataTable dataTable =ConvertToDataTable(list);
                bulkCopy.ColumnMappings.Add("Id", "Id");
                bulkCopy.ColumnMappings.Add("ArticleId", "ArticleId");
                bulkCopy.ColumnMappings.Add("ImageFileName", "ImageFileName");
                bulkCopy.ColumnMappings.Add("IsDelete", "IsDelete");
                
                bulkCopy.WriteToServer(dataTable);
                bulkCopy.Close();
            }
             */
        }
        #endregion
        
        #region help method
        public DataTable ConvertToDataTable(List<ArticleImageEntity> list)
        {
            DataTable table = new DataTable("ArticleImage");
            table.Columns.Add("Id", typeof(System.Int32));
            table.Columns.Add("ArticleId", typeof(System.Int32));
            table.Columns.Add("ImageFileName", typeof(System.String));
            table.Columns.Add("IsDelete", typeof(System.Int32));
            
            if (list != null && list.Count > 0)
            {
                foreach (ArticleImageEntity entity in list)
                {
                    table.Rows.Add(
                        entity.Id,
                        entity.ArticleId,
                        entity.ImageFileName,
                        entity.IsDelete
                        );
                }
            }
            return table;
        }
        #endregion
    }
}
