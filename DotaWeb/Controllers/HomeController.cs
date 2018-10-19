using System.Web.Mvc;
using DotaApi.Helpers;

namespace DotaWeb.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			ViewBag.ShowList = false;
			return View();
		}

		public ActionResult GetMatchData(string matchID, string playerID)
		{
			// 4169885095
			long x = long.Parse(matchID); //Convert.ToInt64()
			var MatchDetailsModel = CommonExtensions.GetMatchDetail(x);
			ViewBag.ShowList = true;
			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "About the author: https://github.com/apatel-gpsw";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}