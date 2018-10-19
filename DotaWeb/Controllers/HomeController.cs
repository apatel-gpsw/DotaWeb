using System.Web.Mvc;

namespace DotaWeb.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult GetMatchData(string matchID, string playerID)
		{

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