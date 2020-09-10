using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CIIC.TongXun
{
    public class CommentHelper
    {

        /// <summary>
        ///  允许上传文件扩展名
        /// </summary>
        /// <param name="fileExtension"></param>
        /// <returns></returns>
        public static bool IsAllowUploadFile(string fileExtension)
        {
            if (fileExtension.StartsWith("."))
            {
                fileExtension = fileExtension.TrimStart('.');
            }
            string allowFileExtensStr = ConfigurationManager.AppSettings["AllowUpLoadFile"];    //"jpg,jpeg,gif,png,bmp,doc,docx,pdf,zip,rar";

            if (allowFileExtensStr.IndexOf(fileExtension) == -1)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// DataTable转集合对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tbl"></param>
        /// <returns></returns>
        public static List<T> CreateListFromTable<T>(DataTable tbl) where T : new()
        {
            List<T> lst = new List<T>();
            foreach (DataRow r in tbl.Rows)
            {
                lst.Add(CreateItemFromRow<T>(r));
            }
            return lst;
        }

        public static T CreateItemFromRow<T>(DataRow row) where T : new()
        {
            Type objType = typeof(T);
            PropertyInfo[] properties = objType.GetProperties();
            T item = new T();
            foreach (PropertyInfo property in properties)
            {
                //object[] colAttr = property.GetCustomAttributes(typeof(Encrypt), false);
                //if (colAttr.Length > 0)
                //{
                //    string ID = row[property.Name.Substring(6)].ToString();
                //    string ecryptID = SecurityHelper.Encrypt(ID);
                //    property.SetValue(item, ecryptID, null);
                //    continue;
                //}
                if (!row.Table.Columns.Contains(property.Name) || row[property.Name] == null || row[property.Name] == DBNull.Value)
                {
                    continue;
                }
                else
                {
                    property.SetValue(item, row[property.Name], null);
                }
            }
            return item;
        }

        #region 生成静态内容
        /// <summary>
        /// 生成静态内容
        /// </summary>
        /// <param name="context">当前Controller的ControllerContext对象</param>
        /// <param name="viewPath">视图模板路径</param>
        /// <param name="model"></param>
        /// <returns>返回已绚烂好的string类型的视图结果</returns>
        public static string RenderViewToString(ControllerContext context, string viewPath, object model = null)
        {
            ViewEngineResult viewEngineResult = ViewEngines.Engines.FindView(context, viewPath, null);
            if (viewEngineResult == null)
                throw new FileNotFoundException("View" + viewPath + "cannot be found.");
            var view = viewEngineResult.View;
            context.Controller.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var ctx = new ViewContext(context, view,
                                context.Controller.ViewData,
                                context.Controller.TempData,
                                sw);
                view.Render(ctx, sw);
                return sw.ToString();
            }
        }
        #endregion

        /// <summary>
        /// 模拟发送get请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        public static void HttpWebRequestGet(string url, string param=null)
        {
            if (param != null)
            {
                param = param.IndexOf('?') > -1 ? (param) : ("?" + param);
            }
            

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url + param);
            Encoding encoding = Encoding.UTF8;
            string responseData = String.Empty;
            req.Method = "GET";
            using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
            {
                
            }
        }


    }
}