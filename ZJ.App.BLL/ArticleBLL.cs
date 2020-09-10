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
    public partial class ArticleBLL
    {
        public DataTable GetArticleDataTablePage(List<SqlDbParameter> parms, string OrderBy, int PageSize, int PageIndex, out int RecordCount)
        {

            ArticleDAL articleDAL = new ArticleDAL();
            DataTable dataTable = articleDAL.GetArticleDataTablePage(parms, OrderBy, PageSize, PageIndex, out RecordCount);
            //TO DO Cache 
            ArticleCategoryDAL articleCategoryDAL = new ArticleCategoryDAL();
            parms = new List<SqlDbParameter>();
            List<ArticleCategoryEntity> articleCategory = articleCategoryDAL.GetAll(parms, "Id");

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                int _categoryId = 0;
                if (int.TryParse(dataTable.Rows[i]["CategoryId"].ToString(), out _categoryId))
                {
                    ArticleCategoryEntity articleCategoryEntity =  articleCategory.Find(t => t.Id == _categoryId);
                    dataTable.Rows[i]["CategoryName"] = articleCategoryEntity == null ? "" : articleCategoryEntity.CategoryName;
                }
            }
            return dataTable;
        }

        /// <summary>
        /// Article 单表列表查询
        /// </summary>
        /// <param name="parms"></param>
        /// <param name="OrderBy"></param>
        /// <returns></returns>
        public DataTable GetArticleDataTable(List<SqlDbParameter> parms, string OrderBy)
        {
            ArticleDAL articleDAL = new ArticleDAL();
            return articleDAL.GetArticleDataTable(parms, OrderBy);
        }
        /// <summary>
        ///  获取文章记录的Top版本方法,含期刊Id,文章类别信息
        /// </summary>
        /// <param name="parms"></param>
        /// <param name="OrderBy"></param>
        /// <param name="Top"></param>
        /// <returns></returns>
        public DataTable GetArticleDataTable(List<SqlDbParameter> parms, string OrderBy, string Top)
        {
            ArticleDAL articleDAL = new ArticleDAL();
            return articleDAL.GetArticleDataTable(parms, OrderBy, Top);
        }

        /// <summary>
        /// 获取详细内容
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public DataTable GetArticleDataTable(List<SqlDbParameter> parms)
        {
            ArticleDAL articleDAL = new ArticleDAL();
            return articleDAL.GetArticleDataLeftJournalAndCategoryTable(parms);
        }

        /// <summary>
        /// 统计期刊的文章数量  
        /// </summary>
        /// <param name="parms">期刊Id 或 类别Id (CategoryId=2 AND JournalId = 3)</param>
        /// <returns></returns>
        public DataTable GetArticleTotal(List<SqlDbParameter> parms)
        {
            ArticleDAL articleDAL = new ArticleDAL();
            return articleDAL.GetArticleTotal(parms);
        }

        public int UpdateArticlePropertyIdByID(int articlePropertyId, int articleId)
        {
            ArticleDAL articleDAL = new ArticleDAL();
            return articleDAL.UpdateArticlePropertyIdByID(articlePropertyId, articleId);

        }
    }
}
