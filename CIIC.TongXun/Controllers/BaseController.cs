using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ZJ.App.BLL;
using ZJ.App.Common;
using ZJ.App.Entity;

namespace CIIC.TongXun.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// 当前期刊
        /// </summary>
        public ArticleJournalEntity currentJournalEntity;

        /// <summary>
        /// 当前URL,这里已经是静态路径了
        /// </summary>
        public string currentURL;

        // GET: Base
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            //TO DO
            string controller = (string)requestContext.RouteData.Values["controller"];
            string action = (string)requestContext.RouteData.Values["action"];
            currentURL = System.Web.HttpContext.Current.Request.Url.AbsolutePath;//获取当前url,以/开头

            string JournalId = Request.QueryString["JournalId"];//期刊ID

            //默认获取最新期刊
            //TODO Cache
            ArticleJournalBLL articleJournalBLL = new ArticleJournalBLL();
            List<SqlDbParameter> parms = new List<SqlDbParameter>();
            SqlDbParameter parm;
            if (!string.IsNullOrEmpty(JournalId))
            {
                parm = new SqlDbParameter();
                parm.ColumnName = "JournalId";
                parm.ParameterName = "JournalId";
                parm.ParameterValue = JournalId;
                parm.ColumnType = DbType.Int32;
                parms.Add(parm);
                currentJournalEntity = articleJournalBLL.GetArticleJournalEntity(parms);
            }
            else {
                DataTable dt = articleJournalBLL.GetJournalDataTable(parms, "JournalId DESC","1"); //返回最新的期刊
                currentJournalEntity = new ArticleJournalEntity();
                currentJournalEntity.JournalId = int.Parse(dt.Rows[0]["JournalId"].ToString());
                currentJournalEntity.JournalName = dt.Rows[0]["JournalName"].ToString();
                currentJournalEntity.PropertyName = dt.Rows[0]["PropertyName"].ToString();
            }

            ViewBag.JournalName = currentJournalEntity.JournalName;
            ViewBag.JournalNo = currentJournalEntity.PropertyName;

        }

        public ArticleJournalEntity CurJournalEntity
        {
            get {
                ArticleJournalBLL articleJournalBLL = new ArticleJournalBLL();
                List<SqlDbParameter> parms = new List<SqlDbParameter>();
                SqlDbParameter parm = parm = new SqlDbParameter();
                parm.ColumnName = "JournalId";
                parm.ParameterName = "JournalId";
                parm.ParameterValue = 3;
                parm.ColumnType = DbType.Int32;
                parms.Add(parm);
                return articleJournalBLL.GetArticleJournalEntity(parms);
            }
        }
    }
}