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
    public partial class ArticleImageBLL
    {
        public int DelArticleImageByID(int Id)
        {
            ArticleImageDAL articleImageDAL = new ArticleImageDAL();
            return articleImageDAL.DelArticleImageByID(Id);
        }
    }
}
