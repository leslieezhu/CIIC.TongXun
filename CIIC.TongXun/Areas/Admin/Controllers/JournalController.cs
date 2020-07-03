using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CIIC.TongXun.Controllers;
using Newtonsoft.Json;
using ZJ.App.BLL;
using ZJ.App.Common;
using ZJ.App.Entity;

namespace CIIC.TongXun.Areas.Admin
{
    public class JournalController : Controller
    {
        public ActionResult Index()
        {

            return View();
        }

        /// <summary>
        /// 期刊列表
        /// </summary>
        /// <returns></returns>
        public string GetList()
        {
            ArticleJournalBLL articleJournalBLL = new ArticleJournalBLL();
            List<SqlDbParameter> parms = new List<SqlDbParameter>();
            SqlDbParameter parm = null;


            int recordCount;
            int draw = Convert.ToInt32(Request["draw"]);
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            int page = start / length; //start 初始值0
            DataTable dataTable = articleJournalBLL.GetJournalDataTablePage(parms, "JournalId DESC", length, page, out recordCount);
            IDictionary info = new Hashtable();
            info.Add("draw", draw);
            info.Add("recordsTotal", recordCount);
            info.Add("recordsFiltered", recordCount);
            info.Add("data", dataTable);
            return JsonConvert.SerializeObject(info);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public string Create(ArticleJournalEntity journal)
        {
            if (ModelState.IsValid)
            {
                ArticleJournalBLL articleJournalBLL = new ArticleJournalBLL();
                journal.JournalName = journal.JournalName.UrlDecode();//期刊名
                //journal.ArticleContent = article.ArticleContent.UrlDecode();//文章内容
                articleJournalBLL.AddArticleJournalEntity(journal);
                return JsonConvert.SerializeObject(new { result = true, message = "", returnUrl = "Index" });
            }
            return JsonConvert.SerializeObject(new { result = false, message = "" });
        }

        public ActionResult Edit(int id)
        {
            ArticleJournalBLL articleJournalBLL = new ArticleJournalBLL();
            ArticleJournalEntity journalEntity = articleJournalBLL.GetArticleJournalEntityById(id);

            return View(journalEntity);
        }

        [HttpPost]
        public string Edit(ArticleJournalEntity journalUpdate)
        {
            if (ModelState.IsValid)
            {
                ArticleJournalBLL articleJournalBLL = new ArticleJournalBLL();
                ArticleJournalEntity journalEntity = articleJournalBLL.GetArticleJournalEntityById(journalUpdate.JournalId);

                journalEntity.JournalName = journalUpdate.JournalName.UrlDecode();//期刊名称
                journalEntity.PropertyName = journalUpdate.PropertyName.UrlDecode();//期刊编号
                articleJournalBLL.UpdateArticleJournalEntity(journalEntity);

                return JsonConvert.SerializeObject(new { result = true, message = "", returnUrl = "/Admin/Journal/Index" });
            }
            return JsonConvert.SerializeObject(new { result = false, message = "" });
        }

        /// <summary>
        /// 实现整个期刊静态化处理
        /// </summary>
        public void StaticHtml()
        {
            //var otherController = DependencyResolver.Current.GetService< typeof(CIIC.TongXun.Controllers.HomeController) > ();
            //var result = otherController.另一个动作方法();
            // Get a PostAuthenticateRequestProvider and use this to apply a 
            // correctly configured principal to the current http request
            //var provider = (HomeController)DependencyResolver.Current.GetService(typeof(CIIC.TongXun.Controllers.HomeController));
            //provider.Index();
            string JournalId = Request.QueryString["JournalId"];//期刊ID
            string[] staticFileList = {
                 "index.html",            //HomeController Index
                 "news_list_jt.html",     //HomeController ListIndex
                 "news_list_gs.html",
                 "news_list_yw.html",
                 "news_list_jiaojuguoqi.html"
            };
            //staticFileList = new string[] { };

            string url = System.Web.HttpContext.Current.Request.Url.AbsoluteUri; //AbsoluteUri = "http://localhost:58321/Journal/StaticHtml"
            int start = url.IndexOf("//");
            int end = url.IndexOf("/", start + 2);
            string urlHead = url.Substring(0,end + 1);
            string param = "static=1&journalId="+ JournalId;

            for (int i = 0; i < staticFileList.Length; i++)
            {
                string reRewriteUrl = urlHead + staticFileList[i];
                CommentHelper.HttpWebRequestGet(reRewriteUrl, param);
                Response.Write(reRewriteUrl);
                Response.Write("<br />");
            }

            //获取期刊下的文章
            ArticleBLL articleBLL = new ArticleBLL();
            List<SqlDbParameter> parms = new List<SqlDbParameter>();
            SqlDbParameter parm = null;
            if (!string.IsNullOrEmpty(JournalId))
            {
                parm = new SqlDbParameter();
                parm.ColumnName = "JournalId";
                parm.ParameterName = "JournalId";
                parm.ParameterValue = JournalId;
                parm.ColumnType = DbType.Int32;
                parms.Add(parm);
            }
            DataTable dt = articleBLL.GetArticleDataTable(parms, "NoOfJournal");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ArticleEntity article = new ArticleEntity();
                article.Id = int.Parse(dt.Rows[i]["Id"].ToString());
                article.CategoryId = int.Parse(dt.Rows[i]["CategoryId"].ToString()); //列表ID
                article.NoOfCategory = short.Parse(dt.Rows[i]["NoOfCategory"].ToString());
                article.HrefTpl = dt.Rows[i]["HrefTpl"].ToString();
                string reRewriteUrl = urlHead + article.Href;
                CommentHelper.HttpWebRequestGet(reRewriteUrl, param);
                Response.Write(reRewriteUrl);
                Response.Write("<br />");
            }

        }

        //protected override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    var request = filterContext.HttpContext.Request;
        //    string url = request.Url.Authority;
        //    string functionurl = request.RawUrl;
        //    base.OnActionExecuting(filterContext);
        //    filterContext.HttpContext.Response.Write("url:" + url + functionurl);
        //}



    }
}