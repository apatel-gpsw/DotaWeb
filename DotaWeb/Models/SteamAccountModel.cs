namespace DotaWeb.Models
{
	public class SteamAccountPlayerModel
	{
		public string SteamID { get; set; }
		public int CommunityVisibilityState { get; set; }
		public int ProfileState { get; set; }
		public string PersonaName { get; set; }
		public int LastLogOff { get; set; }
		public string ProfileUrl { get; set; }
		public string Avatar { get; set; }
		public string AvatarMedium { get; set; }
		public string AvatarFull { get; set; }
		public int CurrentStatus { get; set; }
	}
}