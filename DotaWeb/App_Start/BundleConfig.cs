using System.Web.Optimization;
using System.Web.Optimization.React;

namespace DotaWeb
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

			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
					  "~/Scripts/bootstrap.js"));

			bundles.Add(new StyleBundle("~/Content/css").Include(
					  "~/Content/bootstrap.css",
					  "~/Content/site.css"));

			bundles.Add(new ScriptBundle("~/bundles/transition", "https://unpkg.com/react-transition-group@2.5.0/dist/react-transition-group.js").Include(
						"~/Scripts/react-transition-group.js"));

			bundles.Add(new BabelBundle("~/bundles/main").Include(
					 "~/Scripts/React/SearchBar.jsx",
					 "~/Scripts/React/MatchDetails.jsx"));

			//var reactTransLoc = "https://unpkg.com/react-transition-group@2.5.0/dist/react-transition-group.js";
			//var reactTransBundle = new ScriptBundle("~/bundles/transition", reactTransLoc).Include("~/Scripts/react-transition-group.js");
			//bundles.Add(reactTransBundle);

			BundleTable.EnableOptimizations = true;
		}
	}
}
