using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CIIC.TongXun
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        
        protected void Application_BeginRequest()
        {
            HttpContext context = HttpContext.Current;
            string requestHtmlPath = context.Request.Path;
            //如果请求中带有html的后缀，需要进行处理
            if (requestHtmlPath.EndsWith("/"))
            {
                context.RewritePath("~");
            }
        }
        
    }
}
