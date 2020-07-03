using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CIIC.TongXun
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();


            //html路径,需要到/Web.config里增加配置
            /*
            routes.MapRoute(
                  name: "Article",
                  url: "{htmlname}.html",
                  defaults: new { controller = "Article", action = "Index", id = UrlParameter.Optional },
                  namespaces: new[] { "CIIC.TongXun.Controllers" }
            );
            */
            
            //预览首页
            routes.MapRoute(
                  name: "HomePage",
                  url: "index.html",
                  defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                  namespaces: new[] { "CIIC.TongXun.Controllers" }
            );
            //集团新闻-jt{0}.html;公司新闻-gs{0}.html;yw{0}.html;国资要闻:jiaojuguoqi{0}.html
            routes.MapRoute(
                  name: "DetailPage",
                  url: @"{^jt|^gs|^yw|^jiaojuguoqi\d+}.html",//大括号里面的作为key
                  defaults: new { controller = "Home", action = "Detail", id = UrlParameter.Optional },
                  namespaces: new[] { "CIIC.TongXun.Controllers" }
            );

            //  /Common/UploadAttachment/
            routes.MapRoute(
                    name: "Common",
                    url: "Common/{action}",
                    defaults: new { controller = "Common", action = "Index", id = UrlParameter.Optional },
                    namespaces: new[] { "CIIC.TongXun.Controllers" }
              );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    //defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //    //defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional },
            //    defaults: new { controller = "Journal", action = "Index", id = UrlParameter.Optional }, //期刊列表
            //    namespaces: new string[] { "CIIC.TongXun.Areas.Admin" }
            //    ).DataTokens.Add("Area", "Admin");

        }
    }
}
