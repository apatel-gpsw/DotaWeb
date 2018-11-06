using DotaWeb.Models;

namespace DotaWeb.Repository
{
	public class PlayerRepository
	{
		public PlayerModel GetPlayer()
		{
			return new PlayerModel { Id = 1, Name = "One" };
		}
	}
}