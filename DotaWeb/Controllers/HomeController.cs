using System.Web.Mvc;
using DotaApi.Helpers;
using DotaWeb.Models;

namespace DotaWeb.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			if (TempData["userData"] == null)
			{
				ViewBag.ShowList = false;
				return View();
			}
			else
			{
				MatchDetailsModel lst = (MatchDetailsModel)TempData["userData"];
				ViewBag.ShowList = true;
				return View(lst);
			}
		}

		public ActionResult GetMatchData(string matchID, string playerID)
		{
			// 4169885095
			var a = HttpContext.Request.QueryString["Match ID"];

			long x = long.Parse(a); //Convert.ToInt64()
			var MatchDetailsModel = CommonExtensions.GetMatchDetail(x);
			ViewBag.ShowList = true;
			// ViewData["MatchDetailsModel"] = MatchDetailsModel;
			// return RedirectToAction("Index", MatchDetailsModel);
			TempData["userData"] = MatchDetailsModel;

			return RedirectToAction("Index");
		}

		public ActionResult About()
		{
			//ViewBag.Message = "About the author: https://github.com/apatel-gpsw";

			//return View();
			if (TempData["userData"] == null)
			{
				ViewBag.ShowList = false;
				return View();
			}
			else
			{
				MatchDetailsModel lst = (MatchDetailsModel)TempData["userData"];
				ViewBag.ShowList = true;
				return View(lst);
			}
		}

		public ActionResult GetMatchData1(string matchID, string playerID)
		{
			long x = long.Parse(matchID); //Convert.ToInt64()
			var MatchDetailsModel = CommonExtensions.GetMatchDetail(x);
			ViewBag.ShowList = true;

			TempData["userData"] = MatchDetailsModel;

			return RedirectToAction("About");
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}