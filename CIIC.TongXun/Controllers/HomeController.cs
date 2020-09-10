using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ZJ.App.BLL;
using ZJ.App.Common;
using ZJ.App.Common.Constants;
using ZJ.App.Entity;

namespace CIIC.TongXun.Controllers
{
    public class HomeController : BaseController
    {
        /// <summary>
        ///  首页含页面静态化处理功能,包括邮件页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //1.BaseController 获取期刊数据
            List<SqlDbParameter> parms = new List<SqlDbParameter>();
            SqlDbParameter parm;

            //默认条件IsDelete!=1,软删标准
            parm = new SqlDbParameter();
            parm.ColumnName = "IsDelete";
            parm.ParameterName = "IsDelete";
            parm.QualificationType = SqlDbParameter.QualificationSymbol.IsNull;
            parm.ColumnType = DbType.Int32;
            parms.Add(parm);

            //获取某一期的首页文章列表
            parm = new SqlDbParameter();
            parm.ColumnName = "JournalId";
            parm.ParameterName = "JournalId";
            parm.ParameterValue = currentJournalEntity.JournalId; //当前期刊Id
            parm.QualificationType = SqlDbParameter.QualificationSymbol.Equal;
            parms.Add(parm);

            List<ArticleEntity> resultList = new List<ArticleEntity>();
            ArticleBLL articleBLL = new ArticleBLL();
            int[] categoryIds = { 1, 2, 6,13 }; //新闻类别:1-集团新闻;6-外企服务公司; 13-人才顾问公司 TODO
            string top = "3";
            parm = new SqlDbParameter();
            parm.ColumnName = "CategoryId";
            parm.ParameterName = "CategoryId";
            parm.QualificationType = SqlDbParameter.QualificationSymbol.Equal;
            parms.Add(parm);
            foreach (var categoryId in categoryIds)
            {
                parm.ParameterValue = categoryId;
                DataTable dt = articleBLL.GetArticleDataTable(parms, "NoOfJournal, IsTop DESC", top);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ArticleEntity article = new ArticleEntity();
                    article.Id = int.Parse(dt.Rows[i]["Id"].ToString());
                    article.ArticleTitle = dt.Rows[i]["ArticleTitle"].ToString();
                    article.ArticleTitleAlias = dt.Rows[i]["ArticleTitleAlias"].ToString();
                    //ArticleTitleForShow 有逻辑写作 Entity
                    article.CategoryId = int.Parse(dt.Rows[i]["CategoryId"].ToString()); //列表ID
                    short _noOfCategory = 1;
                    short.TryParse(dt.Rows[i]["NoOfCategory"].ToString(), out _noOfCategory);
                    article.NoOfCategory = _noOfCategory;
                    article.HrefTpl = dt.Rows[i]["HrefTpl"].ToString();
                    //article.Href = string.Format(dt.Rows[i]["HrefTpl"].ToString(), dt.Rows[i]["NoOfCategory"].ToString().PadLeft(2, '0'));
                    resultList.Add(article);
                }
            }

            //静态持久化
            string viewPath = @"~/Views/Home/Index.cshtml";
            if (Request.QueryString["static"] == "1")
            {
                string html = CommentHelper.RenderViewToString(this.ControllerContext, viewPath, resultList);
                string outputDir = HttpContext.Server.MapPath(ConfigurationManager.AppSettings["HtmlOutput"]) + "\\" + currentJournalEntity.JournalName + "\\";
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
                string curURL = base.currentURL == "/" ? "/index.html" : base.currentURL;
                System.IO.File.WriteAllText(outputDir + curURL, html);
                //2.邮件模板页面
                viewPath = @"~/Views/Home/IndexEmail.cshtml";
                html = CommentHelper.RenderViewToString(this.ControllerContext, viewPath, resultList);
                html = string.Format(html, ConfigurationManager.AppSettings["ProductDiretory"]);//处理模板中的本期目录

                //邮件页面保存目录
                outputDir += "\\mail\\";
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
                curURL = "index.html"; //输出保存的文件名

                System.IO.File.WriteAllText(outputDir + curURL, html);
                //3.HomePage截图
                HtmlImageCapture();
            }

            return View(resultList);
        }

        /// <summary>
        /// 类别文章列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("news_list_{type}.html")]
        public ActionResult ListIndex(string type)
        {
            List<ArticleCategoryEntity> categoryList = new List<ArticleCategoryEntity>();  //文章分类信息
            Dictionary<string, string> listImage = new Dictionary<string, string>
            {
                { "jt","tx_25.png" }, //集团新闻
                { "gs","tx_33.png" }, //公司新闻
                { "yw","tx_35.png"}, //业务动态
                { "jiaojuguoqi","jujiaoguoqi.png"}//聚焦国企
            };

            ViewBag.ListIndexImg = listImage[type];//类别图片
            List<SqlDbParameter> parms;
            SqlDbParameter parm;

            //获取某一期的首页文章列表
            parms = new List<SqlDbParameter>();
            parm = new SqlDbParameter();
            parm.ColumnName = "JournalId";
            parm.ParameterName = "JournalId";
            parm.ParameterValue = currentJournalEntity.JournalId;//当前期刊Id
            parm.QualificationType = SqlDbParameter.QualificationSymbol.Equal;
            parms.Add(parm);

            //默认条件IsDelete!=1,软删标准
            parm = new SqlDbParameter();
            parm.ColumnName = "IsDelete";
            parm.ParameterName = "IsDelete";
            parm.QualificationType = SqlDbParameter.QualificationSymbol.IsNull;
            parm.ColumnType = DbType.Int32;
            parms.Add(parm);

            List<ArticleEntity> resultList = new List<ArticleEntity>();
            ArticleBLL articleBLL = new ArticleBLL();

            int categoryId = Constants.ChannelToCategory[type];
            string top = "20";
            DataTable dt = null;
            if (categoryId == 1 || categoryId == 2) //根据类别获取对应文章,集团新闻-1;公司新闻-2;
            {
                ArticleCategoryBLL articleCategoryBLL = new ArticleCategoryBLL();
                List<SqlDbParameter> categoryParam = new List<SqlDbParameter>();
                parm = new SqlDbParameter();
                parm.ColumnName = "Id";
                parm.ParameterName = "Id";
                parm.ColumnType = DbType.Int32;
                parm.QualificationType = SqlDbParameter.QualificationSymbol.Equal;
                parm.ParameterValue = categoryId;
                categoryParam.Add(parm);
                categoryList = articleCategoryBLL.GetAllArticleCategory(categoryParam);  //集团新闻无子类别,目前就一条分类记录
                
                parm = new SqlDbParameter();
                parm.ColumnName = "CategoryId";
                parm.ParameterName = "CategoryId";
                parm.QualificationType = SqlDbParameter.QualificationSymbol.Equal;
                parm.ParameterValue = categoryId;
                parms.Add(parm);
                dt = articleBLL.GetArticleDataTable(parms, "NoOfJournal", top);
            }
            else if (categoryId == 3) //业务动态
            {
                //int[] categoryArray = { 6, 7, 8, 9,12,13,14,15};  //外企服务公司-6;关爱通公司-7,培训部-8,工会联合会-9;人才顾问公司-12//TODO 增加文章类别,13:科创公司,14党委,15法律事务部
                //获取"业务动态"子类别,即根据PId获取类别记录
                ArticleCategoryBLL articleCategoryBLL = new ArticleCategoryBLL();
                categoryList = articleCategoryBLL.GetArticleCategoryEntitiesByPId(3, "[Order]");  //获取业务动态下的子类别,3表示业务动态

                List<SqlDbParameter> orParms = new List<SqlDbParameter>();
                for (int i = 0; i < categoryList.Count; i++)
                {
                    SqlDbParameter orParameter = new SqlDbParameter();
                    orParameter.ColumnName = "CategoryId";
                    orParameter.ParameterName = "CategoryId_" + i;
                    orParameter.ColumnType = DbType.Int32;
                    orParameter.ParameterValue = categoryList[i].Id;
                    orParms.Add(orParameter);
                }
                parm = new SqlDbParameter();
                parm.QualificationType = SqlDbParameter.QualificationSymbol.Or;
                parm.SqlDbParameters = orParms;
                parm.ParameterName = "CategoryId";
                parm.ParameterValue = 0; //必须设置
                parms.Add(parm);
                dt = articleBLL.GetArticleDataTable(parms, "NoOfJournal", top);
            }
            else if (categoryId == 4)//聚焦国企  逻辑和"业务动态"一样
            {
                //int[] categoryArray = {10, 11 };  //国资要闻-10; 改革前沿-11
                ArticleCategoryBLL articleCategoryBLL = new ArticleCategoryBLL();
                categoryList = articleCategoryBLL.GetArticleCategoryEntitiesByPId(4, "Id");  //聚焦国企有2个子类别

                List<SqlDbParameter> orParms = new List<SqlDbParameter>();
                for (int i = 0; i < categoryList.Count; i++)
                {
                    SqlDbParameter orParameter = new SqlDbParameter();
                    orParameter.ColumnName = "CategoryId";
                    orParameter.ParameterName = "CategoryId_" + i;
                    orParameter.ColumnType = DbType.Int32;
                    orParameter.ParameterValue = categoryList[i].Id;
                    orParms.Add(orParameter);
                }
                parm = new SqlDbParameter();
                parm.QualificationType = SqlDbParameter.QualificationSymbol.Or;
                parm.SqlDbParameters = orParms;
                parm.ParameterName = "CategoryId";
                parm.ParameterValue = 0; //必须设置
                parms.Add(parm);
                dt = articleBLL.GetArticleDataTable(parms, "NoOfJournal", top); //文章类别信息中包含文章静态URL模板
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ArticleEntity article = new ArticleEntity();
                article.Id = int.Parse(dt.Rows[i]["Id"].ToString());
                article.ArticleTitle = dt.Rows[i]["ArticleTitle"].ToString();
                article.CategoryId = int.Parse(dt.Rows[i]["CategoryId"].ToString());
                article.NoOfCategory = short.Parse(dt.Rows[i]["NoOfCategory"].ToString());
                article.HrefTpl = dt.Rows[i]["HrefTpl"].ToString();
                ArticleCategoryEntity categoryEntity = categoryList.First(t => t.Id == article.CategoryId);//拼接文章分类信息
                article.CategoryName = categoryEntity==null ? "未分类" : categoryEntity.CategoryName;
                //article.Href = string.Format(dt.Rows[i]["HrefTpl"].ToString(), dt.Rows[i]["NoOfCategory"].ToString().PadLeft(2, '0'));
                resultList.Add(article);
            }

            //静态持久化
            string viewPath = @"~/Views/Home/ListIndex.cshtml"; //其它频道文章列表页
            if (type == "yw" || type == "jiaojuguoqi")
            {
                viewPath = "~/Views/Home/ListIndexYW.cshtml";
            }
            if (Request.QueryString["static"] == "1")
            {
                string html = CommentHelper.RenderViewToString(this.ControllerContext, viewPath, new ChannlModel(categoryList, resultList));
                string outputDir = HttpContext.Server.MapPath(ConfigurationManager.AppSettings["HtmlOutput"]) + "\\" + currentJournalEntity.JournalName + "\\";
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
                System.IO.File.WriteAllText(outputDir + base.currentURL, html);
            }
            if (type == "yw" || type == "jiaojuguoqi")
            {
                return View(viewPath, new ChannlModel(categoryList, resultList));
            }
            return View(new ChannlModel(categoryList, resultList));
            return View(resultList);
        }

        /// <summary>
        /// 文章详细页(前台)
        /// </summary>
        /// <returns></returns>
        public ActionResult Detail()
        {

            var routeData = Request.RequestContext.RouteData.Values;
            //string url = System.Web.HttpContext.Current.Request.Path;
            
            string routeMapValue = string.Empty;  //路由匹配的值,这里为主文件名
            int i = 0;
            foreach (KeyValuePair<string, object> pair in routeData)
            {
                if (i == 0)
                {
                    //regexStr = pair.Key;
                    routeMapValue = pair.Value.ToString();
                }
                ++i;
            }
            //string url = System.Web.HttpContext.Current.Request.Url.AbsolutePath;//获取当前url,以/开头

            string categoryKey = string.Empty; //类别
            string noOfCategoryKey = string.Empty;//类别序数

            string regexStr = @"\D+";
            Regex regex = new Regex(regexStr,RegexOptions.IgnoreCase);//抓取文章类别标识
            MatchCollection matchs= regex.Matches(routeMapValue);
            foreach (Match match in matchs)
            {
                categoryKey = match.Groups[0].Value;
            }

            regexStr = @"\d+";
            regex = new Regex(regexStr);//
            matchs = regex.Matches(routeMapValue);
            foreach (Match match in matchs)
            {
                noOfCategoryKey = match.Groups[0].Value; //如果文章不属于任何类别,为文章主键
            }
            //期刊Id JournalId:如果请求中带期刊Id就使用,如果没有提供,则使用默认当前期刊Id
            string defaultJournalId = Request.QueryString["JournalId"];
            if (string.IsNullOrEmpty(defaultJournalId)) {
                defaultJournalId = currentJournalEntity.JournalId.ToString();
            }

            ArticleBLL articleBLL = new ArticleBLL();
            List<SqlDbParameter> parms = new List<SqlDbParameter>();
            SqlDbParameter parm = new SqlDbParameter();
            
            if (categoryKey == "nocategory")
            {
                parm.ColumnName = "A.Id";
                parm.ParameterName = "Id";
                parm.ParameterValue = noOfCategoryKey;//当前文章Id
                parm.QualificationType = SqlDbParameter.QualificationSymbol.Equal;
                parms.Add(parm);
            }
            else //通过期刊ID,类别ID, 以及该类别下的序号获取文章(有局限性,即当文章没有类别时,无法通过此逻返回文章)
            {
                parm.ColumnName = "JournalId";
                parm.ParameterName = "JournalId";
                parm.ParameterValue = defaultJournalId;//当前期刊Id
                parm.QualificationType = SqlDbParameter.QualificationSymbol.Equal;
                parms.Add(parm);

                parm = new SqlDbParameter();
                parm.ColumnName = "CategoryId";
                parm.ParameterName = "CategoryId";
                parm.ParameterValue = Constants.ChannelToCategory[categoryKey]; ;//文章类别
                parm.QualificationType = SqlDbParameter.QualificationSymbol.Equal;
                parms.Add(parm);

                int _noOfCategory = 0;
                int.TryParse(noOfCategoryKey, out _noOfCategory);
                parm = new SqlDbParameter();
                parm.ColumnName = "NoOfCategory";
                parm.ParameterName = "NoOfCategory";
                parm.ParameterValue = _noOfCategory;//该类别序号
                parm.QualificationType = SqlDbParameter.QualificationSymbol.Equal;
                parms.Add(parm);
            }

            DataTable dt = articleBLL.GetArticleDataTable(parms);
            ArticleEntity article = new ArticleEntity();
            if (dt.Rows.Count > 0)
            {
                article.ArticleTitle = dt.Rows[0]["ArticleTitle"].ToString();
                article.ArticleContent = dt.Rows[0]["ArticleContent"].ToString().Replace("<p>", "<p class=\"cntp\">");
                article.Id = int.Parse(dt.Rows[0]["Id"].ToString());
                //获取文章关联的图片
                ArticleImageBLL articleImageBLL = new ArticleImageBLL();
                parms = new List<SqlDbParameter>();
                parm = new SqlDbParameter();
                parm.ColumnName = "ArticleId";
                parm.ParameterName = "ArticleId";
                parm.ParameterValue = article.Id;
                parms.Add(parm);

                parm = new SqlDbParameter();
                parm.QualificationType = SqlDbParameter.QualificationSymbol.IsNull;
                parm.ColumnName = "IsDelete";
                parm.ParameterName = "IsDelete";
                parms.Add(parm);

                List<ArticleImageEntity> articleList =  articleImageBLL.GetAllArticleImage(parms);
                //TODO Config
                string imageDictory = ConfigurationManager.AppSettings["AriticleImagePath"];
                article.ImgFileArray = new ArrayList();
                foreach (var item in articleList)
                {
                    article.ImgFileArray.Add(imageDictory + item.ImageFileName);
                }
            }

            //1.详细页上的图标
            ViewBag.DetailImg = Constants.ListImage[categoryKey];//类别图片

            //2.详细页之返回各类别新闻列表页的url
            string detailReturnURL = "news_list_yw.html";
            if (categoryKey == "jt" || categoryKey == "gs")
            {
                detailReturnURL = string.Format("news_list_{0}.html", categoryKey); //TODO
            }
            else if (categoryKey.IndexOf("yw") > -1 || categoryKey.IndexOf("jiaojuguoqi") > -1)
            {
                string[] categoryList = categoryKey.Split('_');
                detailReturnURL = string.Format("news_list_{0}.html", categoryList[0]);
            }
            ViewBag.DetailReturnURL = detailReturnURL; 

            //静态持久化
            if (Request.QueryString["static"] == "1")
            {
                string html = CommentHelper.RenderViewToString(this.ControllerContext, @"~/Views/Home/Detail.cshtml", article);
                string outputDir = HttpContext.Server.MapPath(ConfigurationManager.AppSettings["HtmlOutput"]) + "\\" + currentJournalEntity.JournalName + "\\";
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
                System.IO.File.WriteAllText(outputDir + base.currentURL, html);
            }
            return View(article);
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        /// <summary>
        /// 抓取HomePage图片
        /// </summary>
        public void HtmlImageCapture()
        {
            string url = System.Web.HttpContext.Current.Request.Url.AbsoluteUri; //AbsoluteUri = "http://localhost:58321/Journal/StaticHtml"
            int start = url.IndexOf("//");
            int end = url.IndexOf("/", start + 2);
            string urlHead = url.Substring(0, end + 1);
            //返回的页面截屏二进制对象
            Bitmap m_Bitmap = WebSiteThumbnail.GetWebSiteThumbnail(urlHead, 800, 960, 800, 960); //http://localhost:58321/
            //m_Bitmap.Save("D:/XXX/" + "text.png", System.Drawing.Imaging.ImageFormat.Png);

            string outputDir = HttpContext.Server.MapPath(ConfigurationManager.AppSettings["HtmlOutput"]) + "\\" + currentJournalEntity.JournalName + "\\mail\\images\\";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }
            string targetFilePath = outputDir + "index.jpg";
            //在这个页面上再绘制banner图片
            using (Graphics gp = Graphics.FromImage(m_Bitmap))
            {
                string bannerImg = HttpContext.Server.MapPath("~") + "\\images\\banner_4.jpg";    //banner_1~4.jpg 可以定期更换
                gp.DrawImage(Image.FromFile(bannerImg), 47, 131, 702, 184); //必须设置比例,否则会失真
            }

            using (FileStream fs = new FileStream(targetFilePath, FileMode.Create))
            {
                ImageCodecInfo imageCodeInfo = ImageOperation.GetEncoderInfo("image/jpeg");
                System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                EncoderParameters myEncoderParameters = new EncoderParameters(1);
                myEncoderParameters.Param[0] = new EncoderParameter(myEncoder, 85L);
                m_Bitmap.Save(fs, imageCodeInfo, myEncoderParameters);
            }

            //MemoryStream ms = new MemoryStream();
            //m_Bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);//JPG、GIF、PNG等均可
            //byte[] buff = ms.ToArray();
            //Response.BinaryWrite(buff);

        }




    }
}