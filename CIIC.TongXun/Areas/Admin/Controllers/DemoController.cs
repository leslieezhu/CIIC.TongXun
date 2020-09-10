using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
            return View("~/Areas/Admin/Views/Demo/ValidateDemo.cshtml");
            //当前期刊
            string journalId = "5";
            var model = new ArticleJournalEntity();
            if (journalId != null)
            {
                ArticleJournalBLL articleJournalBLL = new ArticleJournalBLL();
                model = articleJournalBLL.GetArticleJournalEntityById(journalId);
            }

            //select options => 文章分类
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
            return View("~/Areas/Admin/Views/Demo/DataTables5.cshtml");
        }

        public List<string> lstProvince = new List<string>() { "北京市", "天津市", "重庆市", "上海市", "河北省", "山西省", "辽宁省", "吉林省", "黑龙江省", "江苏省", "浙江省", "安徽省", "福建省", "江西省", "山东省", "河南省", "湖北省", "湖南省", "广东省", "海南省", "四川省", "贵州省", "云南省", "陕西省", "甘肃省", "青海省", "台湾省", "内蒙古自治区", "广西壮族自治区", "西藏自治区", "宁夏回族自治区", "新疆维吾尔自治区", "香港特别行政区", "澳门特别行政区" };


       

        public string GetCategory()
        {
            ArticleCategoryBLL articleCategoryBLL = new ArticleCategoryBLL();
            List<SqlDbParameter> parms = new List<SqlDbParameter>();
            List<ArticleCategoryEntity> articleCategoryList = articleCategoryBLL.GetAllArticleCategory(parms, "Id");
            //info.Add("data", articleCategoryList);

            IDictionary info = new Hashtable();
            ArrayList selectOptionList = new ArrayList() { };
            selectOptionList.Add(new SelectOption { text = "请选择类别" });
            foreach (ArticleCategoryEntity articleCategory in articleCategoryList)
            {
                if (articleCategory.Level == 1)
                {
                    SelectOption option = new SelectOption { id = articleCategory.Id, text = articleCategory.CategoryName };
                    List<ArticleCategoryEntity> subCategory = articleCategoryList.Where(t => t.PId == articleCategory.Id && t.Level.Value == 2).OrderBy(t => t.Order).ToList();
                    if (subCategory.Count > 0)
                    {
                        List<SelectOption> children = new List<SelectOption>();
                        foreach (var item in subCategory)
                        {
                            SelectOption subOption = new SelectOption { id = item.Id, text = item.CategoryName };
                            children.Add(subOption);
                        }
                        option.children = children;
                    }
                    selectOptionList.Add(option);
                }
            }
            /*
            List<SelectOption> children = new List<SelectOption>();
            SelectOption option = new SelectOption { id = 11, text = "Grapefruit" };
            children.Add(option);
            option = new SelectOption { id = 12, text = "Orange" };
            children.Add(option);
            option = new SelectOption { id = 13, text = "Lemon" };
            children.Add(option);
            option = new SelectOption { id = 14, text = "Lime" };
            children.Add(option);
            selectOptionList.Add(new SelectOption { id = 1, text = "Citrus" , children= children });

            children = new List<SelectOption>();
            option = new SelectOption { id = 21, text = "Apple" };
            children.Add(option);
            option = new SelectOption { id = 22, text = "Mango" };
            children.Add(option);
            option = new SelectOption { id = 23, text = "Banana" };
            children.Add(option);
            selectOptionList.Add(new SelectOption { id = 2, text = "Other", children = children });

            selectOptionList.Add(new SelectOption { id = 3, text = "Demo" });
            */
            info.Add("data", selectOptionList);
            return JsonConvert.SerializeObject(info);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public ActionResult ArticleCategorySelect()
        {
            object area;
            Request.RequestContext.RouteData.DataTokens.TryGetValue("area", out area);

            ArticleCategoryBLL articleCategoryBLL = new ArticleCategoryBLL();
            List<SqlDbParameter> parms = new List<SqlDbParameter>();
            List<ArticleCategoryEntity> articleCategory = articleCategoryBLL.GetAllArticleCategory(parms, "Id");
            return PartialView(articleCategory);
        }

        public string DataTable6() {
            IDictionary info = new Hashtable();
            DataTable dt = new DataTable();
            dt.Columns.Add("DT_RowId", typeof(string));
            dt.Columns.Add("title", typeof(string));
            dt.Columns.Add("author", typeof(string));
            dt.Columns.Add("duration", typeof(string));
            dt.Columns.Add("readingOrder", typeof(string));

            dt.Rows.Add("row_1", "The Final Empire: Mistborn", "Brandon Sanderson", "1479","1");
            dt.Rows.Add("row_2", "The Name of the Wind", "Patrick Rothfuss", "983", "2");
            dt.Rows.Add("row_3", "The Blade Itself: The First Law", "Brandon Sanderson", "1479", "3");
            dt.Rows.Add("row_4", "The Heroes", "Joe Abercrombie", "1390", "4");
            dt.Rows.Add("row_5", "Assassin's Apprentice: The Farseer Trilogy", "Robin Hobb", "1043", "5");
            dt.Rows.Add("row_6", "The Eye of the World: Wheel of Time", "Robert Jordan", "1802", "6");


            info.Add("options", "[]");
            info.Add("files", "[]");
           
            info.Add("data", dt);
            return JsonConvert.SerializeObject(info);
        }
    }
}