using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using ZJ.App.Common;

namespace ZJ.App.DAL
{
    /// <summary>
    /// LEFT JOIN ArticleJournal C ON	B.JournalId=C.JournalId,	  ,C.JournalName --期刊名
    /// </summary>
    public partial class ArticleDAL
    {
        public DataTable GetArticleDataTablePage(List<SqlDbParameter> parms, string OrderBy, int PageSize, int PageIndex, out int RecordCount)
        {
            RecordCount = 0;
            string sql = @" SELECT {0} * FROM (
SELECT A.[Id]
      ,[ArticleTitle]
      ,[IsPublic]
      ,[CreateTime]
      ,[CreateBy]
      ,[IsDelete]
      ,[NoOfJournal]
      ,[NoOfCategory]
      ,[ArticlePropertyId]
      ,[CategoryId] --文章类别
      ,'' CategoryName
	  ,B.JournalId
  FROM [Article] A LEFT JOIN JournalArticleRelation B ON A.Id=B.ArticleID 
) t WHERE 1 = 1 {1} ";
            string sqlString = SqlDbParameter.BuildSqlString(sql, parms);
            DataTable dt = this.GetAll(sqlString, parms, OrderBy, PageSize, PageIndex, out RecordCount);
            return dt;
        }

        /// <summary>
        /// 获取文章记录的Top版本方法,含期刊Id,文章类别信息
        /// </summary>
        /// <param name="parms"></param>
        /// <param name="OrderBy"></param>
        /// <param name="Top"></param>
        /// <returns></returns>
        public DataTable GetArticleDataTable(List<SqlDbParameter> parms, string OrderBy, string Top = null)
        {
            string selectStr = Top == null ? "SELECT " :"SELECT TOP(" + Top + ") ";
            string sql =
 selectStr + @"
      A.[Id]
      ,[ArticleTitle]
      ,[ArticleTitleAlias]
      ,[IsPublic]
      ,[CreateTime]
      ,[CreateBy]
      ,[IsDelete]
      ,[NoOfJournal]
      ,[NoOfCategory] --该分类下的序号
      ,[ArticlePropertyId]
      ,[CategoryId] --文章类别
      ,'' CategoryName
	  ,B.JournalId
      ,C.HrefTpl
  FROM [Article] A LEFT JOIN JournalArticleRelation B ON A.Id=B.ArticleID 
  LEFT JOIN ArticleCategory C ON A.CategoryId = C.Id
WHERE 1 = 1 {1} ORDER BY " + OrderBy ;
            string sqlString = SqlDbParameter.BuildSqlString(sql, parms);
            return this.GetSqlData(sqlString, parms);
        }

        //获取详细内容[ArticleContent]
        public DataTable GetArticleDataLeftJournalAndCategoryTable(List<SqlDbParameter> parms)
        {
            string sql =
 @"SELECT A.[Id]
      ,[ArticleTitle]
      ,[ArticleContent]
      ,[IsPublic]
      ,[CreateTime]
      ,[CreateBy]
      ,[IsDelete]
      ,[NoOfJournal]
      ,[NoOfCategory]
      ,[ArticlePropertyId]
      ,[CategoryId] --文章类别
	  ,B.JournalId
      ,C.HrefTpl
  FROM [Article] A LEFT JOIN JournalArticleRelation B ON A.Id=B.ArticleID 
  LEFT JOIN ArticleCategory C ON A.CategoryId = C.Id
WHERE 1 = 1 {1} ";
            string sqlString = SqlDbParameter.BuildSqlString(sql, parms);
            return this.GetSqlData(sqlString, parms);
        }


        /// <summary>
        /// 统计期刊的文章数量  
        /// </summary>
        /// <param name="parms">期刊Id 或 类别Id (CategoryId=2 AND JournalId = 3)</param>
        /// <returns></returns>
        public DataTable GetArticleTotal(List<SqlDbParameter> parms)
        {
            string sql = @"SELECT
ISNULL(COUNT(1),0) TOTAL
FROM Article A LEFT JOIN JournalArticleRelation B 
ON A.Id = B.ArticleID
WHERE 1 = 1 {1} ";
            sql = SqlDbParameter.BuildSqlString(sql, parms);
            return this.GetSqlData(sql, parms);
        }

        public int UpdateArticlePropertyIdByID(int articlePropertyId, int articleId)
        {

            string sql = @"UPDATE Article SET ArticlePropertyId=@articlePropertyId WHERE Id=@articleId";
            DbCommand dbCommand = CurrentDatabase.GetSqlStringCommand(sql);

            CurrentDatabase.AddInParameter(dbCommand, "@articlePropertyId", DbType.Int32, articlePropertyId);
            CurrentDatabase.AddInParameter(dbCommand, "@articleId", DbType.Int32, articleId);
            return CurrentDatabase.ExecuteNonQuery(dbCommand);
        }

        

    }
}
