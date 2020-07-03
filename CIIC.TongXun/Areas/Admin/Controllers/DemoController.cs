using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZJ.App.BLL;
using ZJ.App.Common;
using ZJ.App.Entity;

namespace CIIC.TongXun.Areas.Admin.Controllers
{
    public class DemoController : Controller
    {
        // GET: Admin/Demo
        public ActionResult Index()
        {
            //当前期刊
            string journalId = "5";
            var model = new ArticleJournalEntity();
            if (journalId != null)
            {
                ArticleJournalBLL articleJournalBLL = new ArticleJournalBLL();
                model = articleJournalBLL.GetArticleJournalEntityById(journalId);
            }

            //文章分类
            ArticleCategoryBLL articleCategoryBLL = new ArticleCategoryBLL();
            List<SqlDbParameter> parms = new List<SqlDbParameter>();
            List<ArticleCategoryEntity> articleCategory = articleCategoryBLL.GetAllArticleCategory(parms, "Id");
            articleCategory.Insert(0, new ArticleCategoryEntity() { CategoryName = "请选择" });
            var selectList = new SelectList(articleCategory, "Id", "CategoryName");
            ViewBag.ArticleCategory = selectList;
            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ViewResult DataTables()
        {
            return View("~/Areas/Admin/Views/Demo/DataTables3.cshtml");
        }
    }
}