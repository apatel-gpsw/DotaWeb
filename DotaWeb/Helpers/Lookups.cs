using System;
using System.Collections.Generic;
using System.ComponentModel;
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
		public static string BLANKIMG = @"https://upload.wikimedia.org/wikipedia/commons/4/48/BLANK_ICON.png";

		public enum Entity
		{
			heroes,
			items,
			abilities
		}

		public enum ImageSize
		{
			// For all 3 Entities
			[Description("Large Portrait")]
			lg,

			// For Hero Portraits
			[Description("Small Portrait")]
			sb,
			[Description("Full Quality Horizontal Portrait")]
			full,
			[Description("Full Quality Vertical Portrait")]
			vert,

			// For Abilities
			[Description("Medium Portrait")]
			md,
			hp1,
			hp2
		}

		public static Dictionary<int, string> LobbyType = new Dictionary<int, string>()
		{
			{-1, "Invalid"},
			{0, "Public Matchmaking"},
			{1, "Practice"},
			{2, "Tournament"},
			{3, "Tutorial"},
			{4, "Co-Op with Bots"},
			{5, "Team Match"},
			{6, "Solo Queue"},
			{7, "Ranked Public Matchmaking"},
			{8, "1v1 Practice Matchmaking"}
		};

		public static Dictionary<int, string> GameModes = new Dictionary<int, string>()
		{
			{0, "Unknown"},
			{1, "All Pick"},
			{2, "Captain’s Mode"},
			{3, "Random Draft"},
			{4, "Single Draft"},
			{5, "All Random"},
			{6, "Intro"},
			{7, "Diretide"},
			{8, "Reverse Captain’s Mode"},
			{9, "The Greeviling"},
			{10, "Tutorial"},
			{11, "Mid Only"},
			{12, "Least Played"},
			{13, "New Player Pool"},
			{14, "Compendium Matchmaking"},
			{15, "Custom"},
			{16, "Captains Draft"},
			{17, "Balanced Draft"},
			{18, "Ability Draft"},
			{19, "Event (?)"},
			{20, "All Random Death Match"},
			{21, "Solo Mid 1 vs 1"},
			{22, "Ranked All Pick"}
		};

		//public enum LobbyType
		//{
		//	Invalid = -1,
		//	[Description("Public Matchmaking")]
		//	PublicMatchmaking = 1,
		//	Practice = 2,
		//	Tournament = 3,
		//	Tutorial = 4,
		//	[Description("Co-Op with Bots")]
		//	CoOpwithBots = 5,
		//	[Description("Team Match")]
		//	TeamMatch = 6,
		//	[Description("Solo Queue")]
		//	SoloQueue = 7,
		//	[Description("Ranked Public Matchmaking")]
		//	RankedPublicMatchmaking = 8,
		//	[Description("1v1 Practice Matchmaking")]
		//	OneVOnePracticeMatchmaking = 9
		//}

		public static string GetImageURL(Enum entity, string name, Enum size)
		{
			switch (name)
			{
				case "vengeful_spirit":
					name = "vengefulspirit";
					break;
				case "zeus":
					name = "zuus";
					break;
				case "necrophos":
					name = "necrolyte";
					break;
				case "queen_of_pain":
					name = "queenofpain";
					break;
				case "wraith King":
					name = "skeleton_king";
					break;
				case "clockwerk":
					name = "rattletrap";
					break;
				case "lifestealer":
					name = "life_stealer";
					break;
				default:
					break;
			}
			return string.Concat(IMAGEURL, entity.ToString(), "/", name, "_", size.ToString(), ".png");
		}
	}
}
