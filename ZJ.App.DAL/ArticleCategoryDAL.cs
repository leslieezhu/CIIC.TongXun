using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ZJ.App.Common;

namespace ZJ.App.DAL
{
    public partial class ArticleCategoryDAL
    {
        public DataTable GetArticleCategory(List<SqlDbParameter> parms)
        {

            return null;
        }

        public DataTable GetArticleCategoryDataTablePage(List<SqlDbParameter> parms, string OrderBy, int PageSize, int PageIndex, out int RecordCount)
        {
            RecordCount = 0;
            string sql = @" SELECT {0} * FROM (
SELECT [Id]
      ,[PId]
      ,[CategoryName]
      ,[Level]
      ,[HrefTpl]
      ,[Order]
  FROM [ArticleCategory]
) t WHERE 1 = 1 {1} ";
            string sqlString = SqlDbParameter.BuildSqlString(sql, parms);
            DataTable dt = this.GetAll(sqlString, parms, OrderBy, PageSize, PageIndex, out RecordCount);
            return dt;
        }

        /// <summary>
        ///  Order BY A.[Order]
        /// </summary>
        /// <param name="parms"></param>
        /// <param name="OrderBy"></param>
        /// <returns></returns>
        public DataTable GetArticleCategoryDataTable(List<SqlDbParameter> parms, string OrderBy)
        {
            string sql = @" SELECT {0} * FROM (
  SELECT A.[Id]
      ,A.[PId]
      ,B.[CategoryName] AS PCategoryName
      ,A.[CategoryName]
      ,A.[Level]
      ,A.[HrefTpl]
      ,A.[Order]
  FROM [ArticleCategory] A LEFT JOIN [ArticleCategory] B
	ON A.PID = B.ID
) t WHERE 1 = 1 {1} ORDER BY " + OrderBy;
            string sqlString = SqlDbParameter.BuildSqlString(sql, parms);
            DataTable dt = this.GetSqlData(sqlString, parms);
            return dt;
        }
    }
}
