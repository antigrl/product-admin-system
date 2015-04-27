using System.Web;
using System.Web.Optimization;

namespace PAS
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/npr").Include(
                        "~/Content/bower_components/jquery/dist/jquery.js",
                        "~/Scripts/jquery-ui-{version}.js",
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*",
                        "~/Content/bower_components/datatables/media/js/jquery.dataTables.js",
                        "~/Content/bower_components/angular/angular.js",
                        "~/Scripts/jquery.throttledresize.js",
                        "~/Content/bower_components/angular-datatables/dist/angular-datatables.js",
                        "~/Scripts/NPR.js",
                        "~/Content/bower_components/angular-datatables/dist/angular-datatables.min.js",
                        "~/Content/bower_components/angular-datatables/dist/plugins/scroller/angular-datatables.scroller.js",
                        "~/Content/bower_components/tether/tether.js",
                        "~/Content/bower_components/angular-animate/angular-animate.js",
                        "~/Scripts/angular-textarea-fit.js",
                        "~/Content/bower_components/angular-material/angular-material.js",
                        "~/Scripts/angular-aria.js",
                        "~/Scripts/nprApp.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/style.css",
                        "~/Content/print.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));
        }
    }
}