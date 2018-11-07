using System.Web.Http;
using DotaWeb.Models;
using DotaWeb.Repository;

namespace DotaWeb.Controllers
{
	public class PlayerController : ApiController
	{
		private PlayerRepository playerRepo;

		public PlayerController()
		{
			playerRepo = new PlayerRepository();
		}

		// GET: api/Player
		//public PlayerModel Get()
		//{
		//	return playerRepo.GetPlayer();
		//}

		[HttpGet]
		public PlayerModel Player()
		{
			return playerRepo.GetPlayer();
		}

		// GET: api/Player/5
		public string Get(int id)
		{
			return "value";
		}

		// POST: api/Player
		public void Post([FromBody]string value)
		{
		}

		// PUT: api/Player/5
		public void Put(int id, [FromBody]string value)
		{
		}

		// DELETE: api/Player/5
		public void Delete(int id)
		{
		}
	}
}
