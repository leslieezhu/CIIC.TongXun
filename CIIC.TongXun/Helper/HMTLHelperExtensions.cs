using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Text.RegularExpressions;

namespace CIIC.TongXun
{
    public static class HMTLHelperExtensions
    {
        public static string Scrpit(this UrlHelper helper, string value)
        {
            string jsCssVersion = "";//CIICConstants.Js_CSS_Version; //ConfigurationManager.AppSettings["Js_CSS_Version"];
            if (string.IsNullOrEmpty(jsCssVersion))
            {
                return helper.Content(value);
            }
            else
            {
                return helper.Content(string.Format(value + "?_v={0}", jsCssVersion));
            }

        }

        /// <summary>
        /// Email有效性验证
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsVilateEmail(string email)
        {
            Regex r = new Regex("^\\s*([A-Za-z0-9_-]+(\\.\\w+)*@(\\w+\\.)+\\w{2,5})\\s*$");
            if (!r.IsMatch(email))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 当客户端提交数据用于了js的 escape()方法编码对应的解码方法
        /// </summary>
        /// <param name="help"></param>
        /// <param name="value"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string UrlDecode(this string value, System.Text.Encoding encoding = null)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (encoding == null)
                {
                    return HttpUtility.UrlDecode(value);
                }
                return HttpUtility.UrlDecode(value, encoding);
            }
            return value;

        }



    }
}
