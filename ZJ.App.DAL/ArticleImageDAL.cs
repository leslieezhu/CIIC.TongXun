using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using ZJ.App.Common;

namespace ZJ.App.DAL
{
    public partial class ArticleImageDAL
    {
        public int DelArticleImageByID(int Id)
        {

            string sql = @"UPDATE ArticleImage SET IsDelete=@IsDelete WHERE Id=@Id";
            DbCommand dbCommand = CurrentDatabase.GetSqlStringCommand(sql);

            CurrentDatabase.AddInParameter(dbCommand, "@IsDelete", DbType.Int32, 1);
            CurrentDatabase.AddInParameter(dbCommand, "@Id", DbType.Int32, Id);
            return CurrentDatabase.ExecuteNonQuery(dbCommand);
        }
    }
}
