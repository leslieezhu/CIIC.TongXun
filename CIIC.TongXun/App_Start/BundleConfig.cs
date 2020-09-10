using System.Web;
using System.Web.Optimization;

namespace CIIC.TongXun
{
    public class BundleConfig
    {
        // 有关捆绑的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery/3.3.1/jquery-{version}.js",
                        "~/Scripts/jquery.cookie.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jqueryvalidation/1.17.0/jquery.validate*",
                        "~/Scripts/jqueryvalidation/jquery.validate.unobtrusive.js"));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备就绪，请使用 https://modernizr.com 上的生成工具仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/Admin/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/Admin/site.css"));

            // Font Awesome icons 对此项目是必备的,列表上的按钮需要此样式,添加font-awesome下很多文件
            bundles.Add(new StyleBundle("~/font-awesome/css").Include(
                      "~/fonts/font-awesome/css/font-awesome.min.css", new CssRewriteUrlTransform()));

            // dataTables css styles
            bundles.Add(new StyleBundle("~/Content/dataTablesStyles").Include(
                      "~/Content/plugins/dataTables/jquery.datatables.min.css",
                      "~/Content/plugins/dataTables/select.dataTables.min.css"));

            

            // dataTables 
            bundles.Add(new ScriptBundle("~/bundles/dataTables").Include(
                      "~/Scripts/plugins/dataTables/1.10.20/jquery.datatables.min.js",
                      "~/Scripts/plugins/dataTables/dataTables.select.min.js",
                      "~/Scripts/plugins/dataTables/localization/messages_cn.js"));

            // jQuery-File-Upload Css
            bundles.Add(new StyleBundle("~/Content/jQueryFileUploadCss").Include(
                      "~/Content/plugins/jQuery-File-Upload/jquery.fileupload.css"));

            //jQuery-File-Upload
            bundles.Add(new ScriptBundle("~/bundles/jQueryFileUpload").Include(
                      "~/Scripts/plugins/jQuery-File-Upload/jquery.ui.widget.js",
                      "~/Scripts/plugins/jQuery-File-Upload/jquery.iframe-transport.js",
                      "~/Scripts/plugins/jQuery-File-Upload/jquery.fileupload.js",
                      "~/Scripts/plugins/jQuery-File-Upload/jquery.fileupload-process.js",
                      "~/Scripts/plugins/jQuery-File-Upload/jquery.fileupload-validate.js"));

            // select2 css style
            bundles.Add(new StyleBundle("~/Content/select2Styles").Include(
                      "~/Content/plugins/select2/select2.min.css", new CssRewriteUrlTransform()));
            // select2 
            bundles.Add(new ScriptBundle("~/bundles/select2").Include(
                      "~/Scripts/plugins/select2/select2.full.min.js"));

        }
    }
}
