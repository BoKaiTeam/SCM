using System.Web.Optimization;

namespace CRM
{
    public class BundleConfig
    {
        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/base").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/underscore.js",
                "~/Scripts/backbone.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/require.js",
                "~/Scripts/main.js"
                ));
            bundles.Add(new StyleBundle("~/bundles/css").Include(
                "~/Content/css/bootstrap-theme.css",
                "~/Content/css/bootstrap.css"
                ));
            bundles.Add(new StyleBundle("~/bundles/signcss").Include(
                "~/Content/css/sign.css"
                ));
            BundleTable.EnableOptimizations = true;
        }
    }
}