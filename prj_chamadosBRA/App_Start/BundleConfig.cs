using System.Web;
using System.Web.Optimization;

namespace prj_chamadosBRA
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/moment.js",
                      "~/Scripts/bootstrap-datepicker.js",
                      "~/Scripts/bootstrap-datetimepicker.js",
                      "~/Scripts/locales/bootstrap-datepicker.pt-BR.js",
                      "~/Scripts/jquery.maskedinput.js",
                      "~/Scripts/jquery.validate.js",
                      "~/Scripts/jquery.validate.unobtrusive.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/mvcfoolproof").Include(
                    "~/Scripts/MicrosoftAjax.js",
                    "~/Scripts/MicrosoftMvcAjax.js",
                    "~/Scripts/MicrosoftMvcValidation.js",
                    "~/Scripts/MvcFoolproofJQueryValidation.min.js",
                    "~/Scripts/MvcFoolproofValidation.min.js",
                    "~/Scripts/mvcfoolproof.unobtrusive.min.js"
                    ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-datepicker3.css",
                      "~/Content/bootstrap-datetimepicker.min.css",
                      "~/Content/datepicker.css",
                      "~/Content/site.css"));
        }
    }
}
