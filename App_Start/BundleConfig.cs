using System.Web;
using System.Web.Optimization;

namespace WebApplication2
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
            
            
            
            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            BundleTable.EnableOptimizations = true;

            bundles.Add(new ScriptBundle("~/bundles/angularJs").Include("~/Scripts/angular.js"));
            //ui-bootstrap
            bundles.Add(new ScriptBundle("~/bundles/uiBootstrap")
                .Include("~/Scripts/angular-ui/ui-bootstrap.js")
                .Include("~/Scripts/angular-ui/ui-bootstrap-tpls.js"));
            bundles.Add(new StyleBundle("~/MyContent/style").Include("~/MyContent/style.css"));
            bundles.Add(new ScriptBundle("~/bundles/appJs").Include(
                "~/Scripts/MyApp/app.js"));
            bundles.Add(new ScriptBundle("~/bundles/clientJs").Include(
                "~/Scripts/MyApp/client.js"));
            bundles.Add(new ScriptBundle("~/bundles/phoneJs").Include(
                "~/Scripts/MyApp/phone.js"));
            bundles.Add(new ScriptBundle("~/bundles/ReservationJs").Include(
               "~/Scripts/MyApp/reservation.js"));
            bundles.Add(new ScriptBundle("~/bundles/LoginJs").Include(
               "~/Scripts/MyApp/user.js"));
        }

    }

}
