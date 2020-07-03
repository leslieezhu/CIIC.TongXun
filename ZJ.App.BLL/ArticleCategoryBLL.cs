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
    public partial class ArticleCategoryBLL
    {

        public List<ArticleCategoryEntity> GetArticleCategoryEntitiesByPId(int PId, string orderBy)
        {
            List<SqlDbParameter> parmsCategory = new List<SqlDbParameter>();
            SqlDbParameter parm = new SqlDbParameter();
            parm.ColumnName = "PId";
            parm.ParameterName = "PId";
            parm.ColumnType = DbType.Int32;
            parm.QualificationType = SqlDbParameter.QualificationSymbol.Equal;
            parm.ParameterValue = PId; //3表示业务类别
            parmsCategory.Add(parm);

            ArticleCategoryDAL dal = new ArticleCategoryDAL();
            return dal.GetAll(parmsCategory, orderBy);
        }

  

    }
}
