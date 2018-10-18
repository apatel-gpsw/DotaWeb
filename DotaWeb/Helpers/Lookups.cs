using System;
using System.Configuration;

namespace DotaApi.Helpers
{
	public static class Lookups
	{
		// This should be the only value that you need to change, obtain an API
		// key from steam and replace below.
		public static string API = ConfigurationManager.AppSettings["APIKey"].ToString();

		public static string ITEM_FILE = @"items.txt";
		public static string ABILITY_FILE = @"npc_abilities.txt";

		// steam urls to get json data
		public static string MATCHHISTORYURL = @"https://api.steampowered.com/IDOTA2Match_570/GetMatchHistory/V001/?key=";
		public static string ITEMSURL = @"http://api.steampowered.com/IEconDOTA2_570/GetGameItems/v0001/?key=";
		public static string HEROSURL = @"https://api.steampowered.com/IEconDOTA2_570/GetHeroes/v0001/?key=";
		public static string MATCHDETAILSURL = @"https://api.steampowered.com/IDOTA2Match_570/GetMatchDetails/V001/?&key=";
		public static string STEAMACCOUNTURL = @"http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key=";
		public static string MATCHHISTORYBYSEQURL = @"https://api.steampowered.com/IDOTA2Match_570/GetMatchHistoryBySequenceNum/v0001/?key=";
		public static string IMAGEURL = @"http://cdn.dota2.com/apps/dota2/images/";

		public enum Entity
		{
			heroes,
			items,
			abilities
		}

		public enum ImageSize
		{
			// For all 3 Entities
			lg,     // Large horizontal portrait

			// For Hero Portraits
			sb,     // Small horizontal portrait
			full,   // Full quality horizontal portrait
			vert,    // Full quality vertical portrait

			// For Abilities
			md,
			hp1,
			hp2
		}

		public static string GetImageURL(Enum entity, string name, Enum size)
		{
			return string.Concat(IMAGEURL, entity.ToString(), "/", name, "_", size.ToString(), ".png");
		}
	}
}
