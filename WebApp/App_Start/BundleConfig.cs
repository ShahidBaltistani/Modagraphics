using System.Web;
using System.Web.Optimization;

namespace WebApp
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/theme/js").Include(
                        "~/Theme/js/*.js"));

            bundles.Add(new StyleBundle("~/theme/css").Include(
                      "~/Theme/css/*.css"));
        }
    }
}
