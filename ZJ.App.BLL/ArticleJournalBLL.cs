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
    public partial class ArticleJournalBLL
    {
        /// <summary>
        ///  返回期刊列表
        /// </summary>
        /// <param name="parms"></param>
        /// <param name="OrderBy"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="RecordCount"></param>
        /// <returns></returns>
        public DataTable GetJournalDataTablePage(List<SqlDbParameter> parms, string OrderBy, int PageSize, int PageIndex, out int RecordCount)
        {
            ArticleJournalDAL articleJournalDAL = new ArticleJournalDAL();
            DataTable dataTable = articleJournalDAL.GetJournalDataTablePage(parms, OrderBy, PageSize, PageIndex, out RecordCount);
            return dataTable;
        }

        /// <summary>
        /// 返回期刊
        /// </summary>
        /// <param name="parms"></param>
        /// <param name="OrderBy"></param>
        /// <param name="Top"></param>
        /// <returns></returns>
        public DataTable GetJournalDataTable(List<SqlDbParameter> parms, string OrderBy, string Top = null)
        {
            ArticleJournalDAL articleJournalDAL = new ArticleJournalDAL();
            DataTable dataTable = articleJournalDAL.GetJournalDataTable(parms, OrderBy, Top);
            return dataTable;
        }
    }
}
