using CIIC.TongXun.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CIIC.TongXun.Controllers
{
    public class CommonController : Controller
    {
        // GET: Common
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 上传附件
        /// </summary>
        /// <returns></returns>
        public JsonResult UploadAttachment()
        {
            if (Request.Files == null || Request.Files.Count == 0)
            {
                return Json(new { result = false, Message = "请选择要上传的文件！" }, JsonRequestBehavior.AllowGet);
            }
            HttpPostedFileBase fileData = Request.Files[0];
            UploadFileResult resultTemp = new UploadFileResult();

            if (fileData != null)
            {
                try
                {
                    int _limitedFileSize = 10000000;
                    int.TryParse(ConfigurationManager.AppSettings["LimitedFileSize"], out _limitedFileSize);
                    if (fileData.ContentLength > _limitedFileSize)
                    {
                        return Json(new { result = false, Message = "上传的文件过大！" }, JsonRequestBehavior.AllowGet);
                    }
                    // 文件上传后的保存路径
                    HttpContext context = System.Web.HttpContext.Current;
                    string savePath = context.Server.MapPath("~");
                    string filePath = savePath + ConfigurationManager.AppSettings["UploadTmp"];
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    string fileName = Path.GetFileName(fileData.FileName);// 原始文件名称
                    string fileExtension = Path.GetExtension(fileName); // 文件扩展名

                    if (CommentHelper.IsAllowUploadFile(fileExtension))
                    {
                        string saveName = Guid.NewGuid().ToString() + fileExtension; // TODO 保存文件名称
                        fileData.SaveAs(filePath + saveName);

                        string baseDirectory = filePath.Substring(filePath.LastIndexOf('\\', filePath.Length - 2)).Trim('\\');
                        resultTemp.url = "/" + baseDirectory + saveName; //baseDirectory 以 /结尾
                        resultTemp.saveName = saveName;
                        resultTemp.name = fileName;
                        List<UploadFileResult> array = new List<UploadFileResult>();
                        array.Add(resultTemp);
                        return Json(new { result = true, files = array });
                    }
                    else
                    {
                        return Json(new { result = false, Message = "上传的文件类型不符合！" }, JsonRequestBehavior.AllowGet);
                    }

                }
                catch (Exception ex)
                {
                    return Json(new { result = false, Message = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { result = false, Message = "请选择要上传的文件！" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}