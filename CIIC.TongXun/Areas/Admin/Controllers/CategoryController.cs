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

namespace CIIC.TongXun.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Admin/Category
        public ActionResult Index()
        {
            return View();
        }

        public string GetList()
        {
            ArticleCategoryBLL articleCategoryBLL = new ArticleCategoryBLL();
            List<SqlDbParameter> parms = new List<SqlDbParameter>();
            SqlDbParameter parm = null;


            //int recordCount;
            int draw = Convert.ToInt32(Request["draw"]);
            //int start = Convert.ToInt32(Request["start"]);
            //int length = Convert.ToInt32(Request["length"]);
            //length = length == 0 ? 100 : length;
            //int page = length > 0 ? start / length : 1; //start 初始值0
            //DataTable dataTable = articleCategoryBLL.GetArticleCategoryDataTablePage(parms, "PId,[Order] ASC", length, page, out recordCount);

            DataTable dataTable = articleCategoryBLL.GetArticleCategoryDataTable(parms, "PId,[Order] ASC");
            IDictionary info = new Hashtable();
            info.Add("draw", draw);
            //info.Add("recordsTotal", recordCount);
            //info.Add("recordsFiltered", recordCount);
            info.Add("data", dataTable);
            return JsonConvert.SerializeObject(info);
        }

        [HttpPost]
        public string Edit(ArticleCategoryEntity articleCategoryUpdate)
        {

            ArticleCategoryBLL articleCategoryBLL = new ArticleCategoryBLL();
            ArticleCategoryEntity articleCategoryEntity = articleCategoryBLL.GetArticleCategoryEntityById(articleCategoryUpdate.Id);

            articleCategoryEntity.CategoryName = articleCategoryUpdate.CategoryName.UrlDecode(); //分类名
            articleCategoryEntity.Order = articleCategoryUpdate.Order;
            articleCategoryBLL.UpdateArticleCategoryEntity(articleCategoryEntity);

            return JsonConvert.SerializeObject(new { result = true, message = "", returnUrl = "/Admin/Journal/Index" });

            return JsonConvert.SerializeObject(new { result = false, message = "" });
        }
    }
}