using System.Web;
using System.Web.Optimization;

namespace RecipeWebRole
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            AddScriptBundles(bundles);
            AddStyleBundles(bundles);
        }

        private static void AddScriptBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery/jquery-1.*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                "~/Scripts/jquery-ui/jquery-ui*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery-unobstrusive/jquery.unobtrusive*",
                "~/Scripts/jquery-validate/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/recipe").Include(
                "~/Scripts/recipe/recipe*"));
        }

        private static void AddStyleBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                "~/Content/MatsStyleSheet.css",
                "~/Content/themes/ui-lightness/jquery-ui-1.8.21.custom.css"));

            bundles.Add(new StyleBundle("~/Content/recipeEditor").Include(
                "~/Content/MatsStyleSheet.css",
                "~/Content/Recipe.Ingredients.css",
                "~/Content/Recipe.Instructions.css"));
        }
    }
}