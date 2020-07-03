using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ZJ.App.Common;

namespace ZJ.App.DAL
{
    public partial class ArticleJournalDAL
    {
        /// <summary>
        /// 返回期刊列表
        /// </summary>
        /// <param name="parms"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        public DataTable GetJournalDataTablePage(List<SqlDbParameter> parms, string OrderBy, int PageSize, int PageIndex, out int RecordCount)
        {
            RecordCount = 0;
            string sql = @" SELECT {0} * FROM (
                SELECT [JournalId]
      ,[JournalName]
      ,[PropertyName]
  FROM [ArticleJournal]
) t WHERE 1 = 1 {1} ";
            string sqlString = SqlDbParameter.BuildSqlString(sql, parms);
            DataTable dt = this.GetAll(sqlString, parms, OrderBy, PageSize, PageIndex, out RecordCount);
            return dt;
        }

        public DataTable GetJournalDataTable(List<SqlDbParameter> parms, string OrderBy, string Top = null)
        {
            string selectStr = Top == null ? "SELECT " : "SELECT TOP(" + Top + ") ";
            string sql =
 selectStr + @"
      [JournalId]
      ,[JournalName]
      ,[PropertyName]
  FROM [ArticleJournal]
WHERE 1 = 1 {1} ORDER BY " + OrderBy;
            string sqlString = SqlDbParameter.BuildSqlString(sql, parms);
            return this.GetSqlData(sqlString, parms);
        }
    }
}
