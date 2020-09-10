using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.IO;
using ZJ.App.BLL;
using ZJ.App.Common;
using ZJ.App.Entity;
using System.Configuration;
using ZJ.App.Common.Constants;
using System.Text.RegularExpressions;

namespace CIIC.TongXun.Areas.Admin
{
    public class ArticleController : Controller
    {
        // GET: Article
        public ActionResult Index()
        {
            //当前期刊
            string journalId = Request.QueryString["JournalId"];
            var model = new ArticleJournalEntity();
            if (journalId != null)
            {
                ArticleJournalBLL articleJournalBLL = new ArticleJournalBLL();
                model = articleJournalBLL.GetArticleJournalEntityById(journalId);
            }
            //ArticleJournalBLL articleJournalBLL = new ArticleJournalBLL();//1.获取期刊
            //List<SqlDbParameter> parms = new List<SqlDbParameter>();
            //SqlDbParameter para = new SqlDbParameter();
            //List<ArticleJournalEntity> journalList = articleJournalBLL.GetAllArticleJournal(parms);
            //journalList.Insert(0, new ArticleJournalEntity() { JournalName ="请选择"});
            //ViewBag.ArticleProperty = new SelectList(journalList, "JournalId", "JournalName"); //期刊ID

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
        /// 文章列表
        /// </summary>
        /// <returns></returns>
        public string GetList()
        {
            ArticleBLL articleBLL = new ArticleBLL();
            List<SqlDbParameter> parms = new List<SqlDbParameter>();
            SqlDbParameter parm = null;
            if (!string.IsNullOrEmpty(Request.Form["JournalId"]))
            {
                parm = new SqlDbParameter();
                parm.ColumnName = "JournalId";
                parm.ParameterName = "JournalId";
                parm.ParameterValue = Request.Form["JournalId"];
                parm.ColumnType = DbType.Int32;
                parms.Add(parm);
            }
            if (!string.IsNullOrEmpty(Request.Form["CategoryId"]) && Request.Form["CategoryId"] != "0") 
            {
                parm = new SqlDbParameter();
                parm.ColumnName = "CategoryId";
                parm.ParameterName = "CategoryId";
                parm.ParameterValue = Request.Form["CategoryId"];
                parm.ColumnType = DbType.Int32;
                parms.Add(parm);
            }

            //默认条件IsDelete!=1,软删标准
            //parm = new SqlDbParameter();
            //parm.ColumnName = "IsDelete";
            //parm.ParameterName = "IsDelete";
            //parm.QualificationType = SqlDbParameter.QualificationSymbol.IsNull;
            //parm.ColumnType = DbType.Int32;
            //parms.Add(parm);

            int recordCount;
            int draw = Convert.ToInt32(Request["draw"]);
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            int page = start / length; //start 初始值0
            DataTable dataTable = articleBLL.GetArticleDataTablePage(parms, "NoOfJournal DESC", length, page, out recordCount);
            IDictionary info = new Hashtable();
            info.Add("draw", draw);
            info.Add("recordsTotal", recordCount);
            info.Add("recordsFiltered", recordCount);
            info.Add("data", dataTable);
            return JsonConvert.SerializeObject(info);

        }

        public ActionResult Create()
        {
            //当前期刊
            string journalId = Request.QueryString["JournalId"];//期刊Id
            var model = new ArticleJournalEntity();
            if (journalId != null)
            {
                ArticleJournalBLL articleJournalBLL = new ArticleJournalBLL();
                model = articleJournalBLL.GetArticleJournalEntityById(journalId);
            }

            //ArticleJournalBLL articleJournalBLL = new ArticleJournalBLL();//1.获取期刊
            //List<SqlDbParameter> parms = new List<SqlDbParameter>();
            //SqlDbParameter para = new SqlDbParameter();
            //List<ArticleJournalEntity> journalList = articleJournalBLL.GetAllArticleJournal(parms);
            //ViewBag.ArticleProperty = new SelectList(journalList, "JournalId", "JournalName");
            //EnumDescription[] articleCategory = EnumDescription.GetFieldTexts(typeof(Enumerator.ArticleType)); //文章分类
            //文章分类 TODO
            ArticleCategoryBLL articleCategoryBLL = new ArticleCategoryBLL();
            List<SqlDbParameter>  parms = new List<SqlDbParameter>();
            List<ArticleCategoryEntity> articleCategory = articleCategoryBLL.GetAllArticleCategory(parms, "Id");
            articleCategory.Insert(0, new ArticleCategoryEntity() { CategoryName = "请选择" });
            var selectList = new SelectList(articleCategory, "Id", "CategoryName");
            ViewBag.ArticleCategory = selectList;// articleCategoryList;
            //ViewBag.ArticleCategory = new SelectList(articleCategory, "EnumValue", "CNText");
            return View(model);
        }

        [HttpPost]
        public string Create(ArticleEntity article)
        {
            if (ModelState.IsValid)
            {
                ArticleBLL articleBLL = new ArticleBLL();
                article.ArticleTitle = article.ArticleTitle.UrlDecode();//文章标题
                article.ArticleTitleAlias = article.ArticleTitleAlias.UrlDecode();
                article.ArticleContent = article.ArticleContent.UrlDecode();//文章内容
                article.CreateTime = DateTime.Now;
                articleBLL.AddArticleEntity(article);
                //Step2 维护文章和期刊的关系
                if (article.JournalId.HasValue)
                {
                    JournalArticleRelationEntity journalArticleRelationEntity = new JournalArticleRelationEntity();
                    journalArticleRelationEntity.JournalId = article.JournalId;//期刊Id
                    journalArticleRelationEntity.ArticleId = article.Id;
                    JournalArticleRelationBLL journalArticleRelationBLL = new JournalArticleRelationBLL();
                    journalArticleRelationBLL.AddJournalArticleRelationEntity(journalArticleRelationEntity);
                    articleBLL.UpdateArticlePropertyIdByID(journalArticleRelationEntity.Id, article.Id);
                }
                //Step3 维护照片关系
                this.ArticleImageSave(article);
                
                //if (article.ImgFileList != null)
                //{
                //    ArticleImageBLL articleImageBLL = new ArticleImageBLL();
                //    int j = 1;
                //    for (int i = 0; i < article.ImgFileList.Count; i++)
                //    {
                //        string savePath = System.Web.HttpContext.Current.Server.MapPath("~");
                //        string fromPath = savePath + ConfigurationManager.AppSettings["UploadTmp"] + article.ImgFileList[i].ImgFileName;
                //        string fileExtension = Path.GetExtension(fromPath); // 文件扩展名

                //        string categoryFix = "other";
                //        //根据文件类别+分类下序数命名新图片名
                //        var firstKey = Constants.ChannelToCategory.FirstOrDefault(q => q.Value == article.CategoryId.Value).Key;
                //        if (!string.IsNullOrEmpty(firstKey))
                //        {
                //            categoryFix = firstKey;
                //        }
                //        string newFileName = categoryFix + article.NoOfCategory.ToString().PadLeft(2, '0') + "_" + j + fileExtension; // 文件扩展名  //jt01_1.jpg
                //        ++j;
                //        string toPath = savePath + ConfigurationManager.AppSettings["AriticleImagePath"] + newFileName;
                //        if (!Directory.Exists(savePath + ConfigurationManager.AppSettings["AriticleImagePath"]))
                //        {
                //            Directory.CreateDirectory(savePath + ConfigurationManager.AppSettings["AriticleImagePath"]);
                //        }
                //        System.IO.File.Copy(fromPath, toPath);
                //        //TODO Insert ArticleImage
                //        ArticleImageEntity articleImage = new ArticleImageEntity();
                //        articleImage.ArticleId = article.Id;
                //        articleImage.ImageFileName = newFileName;
                //        articleImageBLL.AddArticleImageEntity(articleImage);
                //    }
                //}
                
                return JsonConvert.SerializeObject(new { result = true, message = "", returnUrl = "/Admin/Article/Index?JournalId="+ article.JournalId });
            }
            return JsonConvert.SerializeObject(new { result = false, message = "" });
        }

        /// <summary>
        /// 创建文章初始化处理
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CreateInit()
        {
            int nextNoOfJournal = 0;
            int nextNoOfCategory = 0;

            if (!string.IsNullOrEmpty(Request.Form["journalId"]) && Request.Form["journalId"] != "0")//查询当前期刊下已有文章数量
            {
                ArticleBLL articleBLL = new ArticleBLL();
                List<SqlDbParameter> parms = new List<SqlDbParameter>();
                SqlDbParameter parm = null;
                parm = new SqlDbParameter();
                parm.ColumnName = "JournalId";
                parm.ParameterName = "JournalId";
                parm.ParameterValue = Request.Form["journalId"];
                parm.ColumnType = DbType.Int32;
                parms.Add(parm);
                DataTable dt = articleBLL.GetArticleTotal(parms); 
                //TODO 如果有文章被删除,就不能用这样的取号处理逻辑
                if (dt.Rows.Count > 0)
                {
                    nextNoOfJournal = int.Parse(dt.Rows[0]["TOTAL"].ToString()) + 1;
                }

                if (!string.IsNullOrEmpty(Request.Form["categoryId"]) && Request.Form["categoryId"] != "0") //查询当前期刊下当前类别的文章数量
                {
                    parm = new SqlDbParameter();
                    parm.ColumnName = "CategoryId";
                    parm.ParameterName = "CategoryId";
                    parm.ParameterValue = Request.Form["categoryId"];
                    parm.ColumnType = DbType.Int32;
                    parms.Add(parm);
                    DataTable dt2 = articleBLL.GetArticleTotal(parms);
                    if (dt2.Rows.Count > 0)
                    {
                        nextNoOfCategory = int.Parse(dt2.Rows[0]["TOTAL"].ToString()) + 1;
                    }
                }
            }
            if (nextNoOfJournal == 0)
            {
                nextNoOfJournal += 1;
            }
            if (nextNoOfCategory == 0)
            {
                nextNoOfCategory += 1;
            }
            return Json(new { result = false, noOfJournal = nextNoOfJournal, noOfCategory= nextNoOfCategory }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Edit(int id)
        {
            ArticleBLL articleBLL = new ArticleBLL();
            ArticleEntity articleEntity = articleBLL.GetArticleEntityById(id);

            //查文章对应的期刊
            if (articleEntity.ArticlePropertyId.HasValue)
            {
                JournalArticleRelationBLL journalArticleRelationBLL = new JournalArticleRelationBLL();
                JournalArticleRelationEntity journalArticleRelationEntity = journalArticleRelationBLL.GetJournalArticleRelationEntityById(articleEntity.ArticlePropertyId);
                ArticleJournalBLL articleJournalBLL = new ArticleJournalBLL();
                ArticleJournalEntity articleJournalEntity = articleJournalBLL.GetArticleJournalEntityById(journalArticleRelationEntity.JournalId);
                articleEntity.JournalName = articleJournalEntity.JournalName;
                articleEntity.PropertyName = articleJournalEntity.PropertyName;
                articleEntity.JournalId = articleJournalEntity.JournalId;//文章所属期刊Id
            }
            /*
            ArticleJournalBLL articleJournalBLL = new ArticleJournalBLL();//1.获取期刊
            List<SqlDbParameter> parms = new List<SqlDbParameter>();
            SqlDbParameter para = new SqlDbParameter();
            List<ArticleJournalEntity> journalList = articleJournalBLL.GetAllArticleJournal(parms);
            ViewBag.ArticleProperty = new SelectList(journalList, "JournalId", "JournalName", articleEntity.JournalId);
            */
            //文章分类 TODO
            ArticleCategoryBLL articleCategoryBLL = new ArticleCategoryBLL();
            List<SqlDbParameter> parms = new List<SqlDbParameter>();
            List<ArticleCategoryEntity> articleCategory = articleCategoryBLL.GetAllArticleCategory(parms, "Id");
            articleCategory.Insert(0, new ArticleCategoryEntity() { CategoryName = "请选择" });
            var selectList = new SelectList(articleCategory, "Id", "CategoryName", articleEntity.CategoryId);

            ViewBag.ArticleCategory = selectList;
            return View(articleEntity);
        }

        [HttpPost]
        public string Edit(ArticleEntity articleUpdate)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).ToList();//查找具体出错的模型字段
            //ModelState.Remove("Id");

            if (ModelState.IsValid)
            {
                ArticleBLL articleBLL = new ArticleBLL();
                ArticleEntity  article =  articleBLL.GetArticleEntityById(articleUpdate.Id);

                article.ArticleTitle = articleUpdate.ArticleTitle.UrlDecode();//文章标题
                article.ArticleTitleAlias = articleUpdate.ArticleTitleAlias.UrlDecode();
                article.ArticleContent = articleUpdate.ArticleContent.UrlDecode();//文章内容
                article.CategoryId = articleUpdate.CategoryId;
                article.NoOfJournal = articleUpdate.NoOfJournal;
                article.NoOfCategory = articleUpdate.NoOfCategory;
                article.IsPublic = articleUpdate.IsPublic;
                //article.IsTop = articleUpdate.IsTop;

                if (article.ArticlePropertyId.HasValue)
                {
                    JournalArticleRelationBLL journalArticleRelationBLL = new JournalArticleRelationBLL();
                    JournalArticleRelationEntity journalArticleRelationEntity = journalArticleRelationBLL.GetJournalArticleRelationEntityById(article.ArticlePropertyId);
                    ArticleJournalBLL articleJournalBLL = new ArticleJournalBLL();
                    ArticleJournalEntity articleJournalEntity = articleJournalBLL.GetArticleJournalEntityById(journalArticleRelationEntity.JournalId);
                    article.JournalId = articleJournalEntity.JournalId;
                }

                articleBLL.UpdateArticleEntity(article);
                //TODO Step3 维护照片关系
                this.ArticleImageSave(articleUpdate);

                string returnUrl = article.JournalId.HasValue ? "/Admin/Article/Index?JournalId=" + article.JournalId : "/Admin/Article/Index";
                return JsonConvert.SerializeObject(new { result = true, message = "", returnUrl = returnUrl });
            }
            return JsonConvert.SerializeObject(new { result = false, message = "" });
        }

        /// <summary>
        /// 保存文章的相关图片
        /// </summary>
        /// <param name="article"></param>
        public void ArticleImageSave(ArticleEntity article)
        {
            if (article.ImgFileList != null)
            {
                ArticleImageBLL articleImageBLL = new ArticleImageBLL();
                //为图片命名获取参数
               
                List<SqlDbParameter> parms = new List<SqlDbParameter>();
                SqlDbParameter parm = null;
                parm = new SqlDbParameter();
                parm.ColumnName = "ArticleId";
                parm.ParameterName = "ArticleId";
                parm.ParameterValue = article.Id;
                parm.ColumnType = DbType.Int32;
                parms.Add(parm);

                

                List<ArticleImageEntity> articleList = articleImageBLL.GetAllArticleImage(parms, "Id DESC");
                int j = 1;
                if (articleList.Count > 0) {

                    Match numMatch = Regex.Match(articleList[0].ImageFileName, @"_(\d+)\."); //获取最大的编号
                    string num = numMatch.Groups[1].Value;
                    int _j = 0;
                    if (int.TryParse(num, out _j)) {
                        j = ++_j;
                    }
                }

                for (int i = 0; i < article.ImgFileList.Count; i++)
                {
                    if (article.ImgFileList[i].Id != null) //New Add Picture
                    {
                        continue;
                    }
                    string savePath = System.Web.HttpContext.Current.Server.MapPath("~");
                    string fromPath = savePath + ConfigurationManager.AppSettings["UploadTmp"] + article.ImgFileList[i].ImgFileName;
                    string fileExtension = Path.GetExtension(fromPath); // 文件扩展名

                    string categoryFix = "other";
                    //根据文件类别+分类下序数命名新图片名
                    var firstKey = Constants.ChannelToCategory.FirstOrDefault(q => q.Value == article.CategoryId.Value).Key;
                    if (!string.IsNullOrEmpty(firstKey))
                    {
                        categoryFix = firstKey;
                    }
                    string newFileName = categoryFix + article.NoOfCategory.ToString().PadLeft(2, '0') + "_" + j + fileExtension; // 文件扩展名  //jt01_1.jpg
                    ++j;
                    string toPath = savePath + ConfigurationManager.AppSettings["AriticleImagePath"] + newFileName;
                    if (!Directory.Exists(savePath + ConfigurationManager.AppSettings["AriticleImagePath"]))
                    {
                        Directory.CreateDirectory(savePath + ConfigurationManager.AppSettings["AriticleImagePath"]);
                    }
                    System.IO.File.Copy(fromPath, toPath);
                    //TODO Insert ArticleImage
                    ArticleImageEntity articleImage = new ArticleImageEntity();
                    articleImage.ArticleId = article.Id;
                    articleImage.ImageFileName = newFileName;
                    articleImageBLL.AddArticleImageEntity(articleImage);
                }
            }
        }

        /// <summary>
        /// 文章预览
        /// </summary>
        /// <param name="id">文章Id</param>
        /// <returns></returns>
        public ActionResult Perview(int id)
        {
            ArticleBLL articleBLL = new ArticleBLL();
            ArticleEntity articleEntity = articleBLL.GetArticleEntityById(id);
            string staticURL = "/";

            //生成文章静态URL
            ArticleCategoryBLL articleCategoryBLL = new ArticleCategoryBLL();
            ArticleCategoryEntity categoryEntity = articleCategoryBLL.GetArticleCategoryEntityById(articleEntity.CategoryId);
            //TODO 判断文章没有任何分类的情况
            articleEntity.HrefTpl = categoryEntity != null ? categoryEntity.HrefTpl : "nocategory{0}.html";//文章没有类别时,详细页URL

            if (!string.IsNullOrEmpty(articleEntity.HrefTpl))
            {
                //查文章对应的期刊
                if (articleEntity.ArticlePropertyId.HasValue)
                {
                    JournalArticleRelationBLL journalArticleRelationBLL = new JournalArticleRelationBLL();
                    JournalArticleRelationEntity journalArticleRelationEntity = journalArticleRelationBLL.GetJournalArticleRelationEntityById(articleEntity.ArticlePropertyId);
                    ArticleJournalBLL articleJournalBLL = new ArticleJournalBLL();
                    ArticleJournalEntity articleJournalEntity = articleJournalBLL.GetArticleJournalEntityById(journalArticleRelationEntity.JournalId);
                    articleEntity.JournalId = articleJournalEntity.JournalId;//期刊Id
                    articleEntity.JournalName = articleJournalEntity.JournalName;//期刊名
                    articleEntity.PropertyName = articleJournalEntity.PropertyName;//期刊总序号
                    staticURL += articleEntity.Href + "?JournalId=" + articleEntity.JournalId;
                    return Redirect(staticURL);
                }
                staticURL += articleEntity.Href;
                return Redirect(staticURL);
            }
            return new EmptyResult();
        }

        /// <summary>
        /// 后台的文章静态处理
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult StaticHtml(int id)
        {
            ArticleBLL articleBLL = new ArticleBLL();
            ArticleEntity articleEntity = articleBLL.GetArticleEntityById(id);
            string staticURL = "/";

            //生成文章静态URL
            ArticleCategoryBLL articleCategoryBLL = new ArticleCategoryBLL();
            ArticleCategoryEntity categoryEntity = articleCategoryBLL.GetArticleCategoryEntityById(articleEntity.CategoryId);
            articleEntity.HrefTpl = categoryEntity.HrefTpl;

            if (!string.IsNullOrEmpty(categoryEntity.HrefTpl))
            {
                //查文章对应的期刊
                if (articleEntity.ArticlePropertyId.HasValue)
                {
                    JournalArticleRelationBLL journalArticleRelationBLL = new JournalArticleRelationBLL();
                    JournalArticleRelationEntity journalArticleRelationEntity = journalArticleRelationBLL.GetJournalArticleRelationEntityById(articleEntity.ArticlePropertyId);
                    ArticleJournalBLL articleJournalBLL = new ArticleJournalBLL();
                    ArticleJournalEntity articleJournalEntity = articleJournalBLL.GetArticleJournalEntityById(journalArticleRelationEntity.JournalId);
                    articleEntity.JournalId = articleJournalEntity.JournalId;//期刊Id
                    articleEntity.JournalName = articleJournalEntity.JournalName;//期刊名
                    articleEntity.PropertyName = articleJournalEntity.PropertyName;//期刊总序号
                    staticURL += articleEntity.Href + "?JournalId=" + articleEntity.JournalId+ "&static=1";
                    return Redirect(staticURL);
                }
                staticURL += articleEntity.Href + "&static=1";
                return Redirect(staticURL);
            }
            return new EmptyResult();
        }

        public string GetImages(string articleId)
        {
            //获取文章关联的图片
            ArticleImageBLL articleImageBLL = new ArticleImageBLL();
            List<SqlDbParameter> parms = new List<SqlDbParameter>();
            SqlDbParameter parm = new SqlDbParameter();
            parm.ColumnName = "ArticleId";
            parm.ParameterName = "ArticleId";
            parm.ParameterValue = articleId;
            parms.Add(parm);

            parm = new SqlDbParameter();
            parm.QualificationType = SqlDbParameter.QualificationSymbol.IsNull;
            parm.ColumnName = "IsDelete";
            parm.ParameterName = "IsDelete";
            parms.Add(parm);

            List<ArticleImageEntity> imageList = articleImageBLL.GetAllArticleImage(parms);

            IDictionary info = new Hashtable();
            if (imageList.Count == 0)
            {
                info.Add("data", null);
                return JsonConvert.SerializeObject(info);
            }
            //封装属性url,saveName, name
            foreach (var image in imageList)
            {
                image.url = "/" + ConfigurationManager.AppSettings["AriticleImagePath"] + image.ImageFileName;
            }
            info.Add("data", imageList);
            return JsonConvert.SerializeObject(info);

        }

        public string DelImage(string articleImageId)
        {
            int _articleImageId = 0;
            int.TryParse(articleImageId, out _articleImageId);
            ArticleImageBLL articleImageBLL = new ArticleImageBLL();
            int result = articleImageBLL.DelArticleImageByID(_articleImageId);
            return JsonConvert.SerializeObject(result);
        }

    }
}