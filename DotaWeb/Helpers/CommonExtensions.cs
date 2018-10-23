using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using DotaApi.Model;
using DotaWeb.Models;
using Newtonsoft.Json;
using static DotaApi.Helpers.Lookups;
using static DotaApi.Model.Heroes;
using static DotaApi.Model.Item;

namespace DotaApi.Helpers
{
	public static class CommonExtensions
	{
		public static Dictionary<int, Item> Items = GetGameItems();
		public static Dictionary<int, Hero> Heroes = GetHeroes();
		public static Dictionary<int, Ability> Abilities = ParseAbilityText();

		#region MatchDetails

		/// <summary>
		/// Gets match details for a single match, this includes player builds and details. Requires "MatchClass".
		/// </summary>
		public static MatchDetailsModel GetMatchDetail(long matchid)
		{
			string response = DownloadSteamAPIString(MATCHDETAILSURL, API + "&match_id=" + matchid);

			var detail = JsonConvert.DeserializeObject<MatchDetailsRootObject>(response);
			MatchDetailsModel match = detail.Result;

			match.StartTime = StringManipulation.UnixTimeStampToDateTime(match.Start_Time);
			TimeSpan time = TimeSpan.FromSeconds(match.Duration);
			string gameDuration = time.ToString(@"hh\:mm\:ss");

			if (LobbyType.TryGetValue(match.Lobby_Type, out string lobby))
			{
				match.Lobbytype = lobby;
			}

			foreach (var player in match.Players)
			{
				player.Name = ConvertIDtoName(player.Hero_ID, Heroes);
				player.HeroImage = ConvertIDtoImageUrl(player.Hero_ID, Heroes);
				player.Steamid64 = StringManipulation.SteamIDConverter(player.Account_ID);
				player.Steamid32 = StringManipulation.SteamIDConverter64to32(player.Steamid64);

				SteamAccountPlayerModel steamaccount = GetSteamAccount(player.Account_ID);
				player.PlayerName = string.IsNullOrEmpty(steamaccount.PersonaName) ? "Anonymous" : steamaccount.PersonaName;

				player.Tower_Damage = player.Tower_Damage == 0 ? 0 : player.Tower_Damage;

				// getting item names based on the id number
				player.Item0 = player.Item_0 > 0 ? ConvertIDtoName(player.Item_0, Items) : null;
				player.Item0Image = player.Item_0 > 0 ? ConvertIDtoImageUrl(player.Item_0, Items) : "https://upload.wikimedia.org/wikipedia/commons/4/48/BLANK_ICON.png";

				player.Item1 = player.Item_1 > 0 ? ConvertIDtoName(player.Item_1, Items) : null;
				player.Item1Image = player.Item_1 > 0 ? ConvertIDtoImageUrl(player.Item_1, Items) : "https://upload.wikimedia.org/wikipedia/commons/4/48/BLANK_ICON.png";

				player.Item2 = player.Item_2 > 0 ? ConvertIDtoName(player.Item_2, Items) : null;
				player.Item2Image = player.Item_2 > 0 ? ConvertIDtoImageUrl(player.Item_2, Items) : "https://upload.wikimedia.org/wikipedia/commons/4/48/BLANK_ICON.png";

				player.Item3 = player.Item_3 > 0 ? ConvertIDtoName(player.Item_3, Items) : null;
				player.Item3Image = player.Item_3 > 0 ? ConvertIDtoImageUrl(player.Item_3, Items) : "https://upload.wikimedia.org/wikipedia/commons/4/48/BLANK_ICON.png";

				player.Item4 = player.Item_4 > 0 ? ConvertIDtoName(player.Item_4, Items) : null;
				player.Item4Image = player.Item_4 > 0 ? ConvertIDtoImageUrl(player.Item_4, Items) : "https://upload.wikimedia.org/wikipedia/commons/4/48/BLANK_ICON.png";

				player.Item5 = player.Item_5 > 0 ? ConvertIDtoName(player.Item_5, Items) : null;
				player.Item5Image = player.Item_5 > 0 ? ConvertIDtoImageUrl(player.Item_5, Items) : "https://upload.wikimedia.org/wikipedia/commons/4/48/BLANK_ICON.png";

				player.Backpack0 = player.Backpack_0 > 0 ? ConvertIDtoName(player.Backpack_0, Items) : null;
				player.Backpack1 = player.Backpack_1 > 0 ? ConvertIDtoName(player.Backpack_1, Items) : null;
				player.Backpack2 = player.Backpack_2 > 0 ? ConvertIDtoName(player.Backpack_2, Items) : null;

				if (player.Ability_Upgrades != null)
				{
					foreach (var ability in player.Ability_Upgrades)
					{
						// clean up the object a bit and put
						// id where it should be.
						ability.ID = ability.Ability;

						// map the id to a readable name.
						ability.Name = ConvertIDtoName(Convert.ToInt32(ability.Ability), Abilities);

						// add the upgrade seconds to the original start
						// time to get the upgrade time.
						ability.UpgradeTime = match.StartTime.AddSeconds(ability.Time);
					}
				}
				else
				{
					// sb.AppendLine("No abilities data");
				}
			}
			return match;
		}

		/// <summary>
		/// Generic Method using <see cref="ISearchableDictionary"/> properties.
		/// Returns Name corresponding to the ID.
		/// </summary>
		private static string ConvertIDtoName<T>(int id, Dictionary<int, T> itemDict) where T : ISearchableDictionary
		{
			T itemObject = itemDict.Where(x => x.Key == id).FirstOrDefault().Value;
			return itemObject != null ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(itemObject.Name) : "";
		}

		/// <summary>
		/// Generic Method using <see cref="ISearchableDictionary"/> properties.
		/// Returns URL corresponding to the ID.
		/// </summary>
		private static string ConvertIDtoImageUrl<T>(int id, Dictionary<int, T> itemDict) where T : ISearchableDictionary
		{
			T itemObject = itemDict.Where(x => x.Key == id).FirstOrDefault().Value;
			return itemObject != null ? itemObject.ImageURL : "";
		}

		/// <summary>
		/// Reads the npc_abilities.txt and builds the abilities class.
		/// </summary>
		private static Dictionary<int, Ability> ParseAbilityText()
		{
			string abilityFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "App_Data", ABILITY_FILE);

			var text = File.ReadAllLines(abilityFile);
			bool itemfound = false;

			// List to hold our parsed items.
			Dictionary<int, Ability> resultDic = new Dictionary<int, Ability>();

			// Item object will be populating
			Ability ability = new Ability();

			// Let's go line by line to start parsing.
			int count = 0;
			string rawAbilityName = "";
			foreach (string line in text)
			{
				// Clean up the text, remove quotes.
				string trimmed_clean = line.Replace("\"", "").Replace("\t", "").Trim();

				// If line starts with item_ then this is where we will start capturing.
				if (trimmed_clean.StartsWith("ID"))
				{
					ability = new Ability();
					itemfound = true;
					ability.ID = Convert.ToInt32(trimmed_clean.Replace("ID", "").Split('/')[0].Trim());

					// We get the hero name
					ability.HeroName = text[count - 4].Split('_')[0].Replace("\"", "").Replace("\t", "").Trim();

					rawAbilityName = text[count - 4].Replace(ability.HeroName, "").Replace("\"", "").Replace("\t", "").Trim();
					ability.ImageURL = GetImageURL(Entity.abilities, ability.HeroName + rawAbilityName, ImageSize.md);

					// Let's remove hero name from original
					ability.Name = rawAbilityName.Replace("_", " ");
				}

				// If we are on a current item then lets do some other operations to gather details
				if (itemfound == true)
				{
					if (trimmed_clean.StartsWith("AbilityBehavior"))
						ability.AbilityBehavior = trimmed_clean.Replace("AbilityBehavior", "");
					if (trimmed_clean.StartsWith("AbilityUnitTargetTeam"))
						ability.AbilityUnitTargetTeam = trimmed_clean.Replace("AbilityUnitTargetTeam", "");
					if (trimmed_clean.StartsWith("AbilityUnitTargetType"))
						ability.AbilityUnitTargetType = trimmed_clean.Replace("AbilityUnitTargetType", "");
					if (trimmed_clean.StartsWith("AbilityUnitDamageType"))
						ability.AbilityUnitDamageType = trimmed_clean.Replace("AbilityUnitDamageType", "");
					if (trimmed_clean.StartsWith("AbilityCastRange"))
						ability.AbilityCastRange = trimmed_clean.Replace("AbilityCastRange", "");
					if (trimmed_clean.StartsWith("AbilityCastPoint"))
						ability.AbilityCastPoint = trimmed_clean.Replace("AbilityCastPoint", "");
					if (trimmed_clean.StartsWith("AbilityCooldown"))
						ability.AbilityCooldown = trimmed_clean.Replace("AbilityCooldown", "");
					if (trimmed_clean.StartsWith("AbilityManaCost"))
						ability.AbilityManaCost = trimmed_clean.Replace("AbilityManaCost", "");

					// End current item, save to list
					if (trimmed_clean.StartsWith("//="))
					{
						// Add to our list of items/
						resultDic.Add(ability.ID, ability);
						itemfound = false;
					}
				}
				count++;
			}
			return resultDic;
		}

		/// <summary>
		/// Gets the latest list of items from Steam.
		/// </summary>
		private static Dictionary<int, Item> GetGameItems()
		{
			string response = DownloadSteamAPIString(ITEMSURL, API);
			ItemsObject itemsObject = JsonConvert.DeserializeObject<ItemsObject>(response);

			Dictionary<int, Item> resultDic = new Dictionary<int, Item>();

			int itemCountInt = 0;
			Item Item;
			string rawItemName = "";
			foreach (var item in itemsObject.Result.Items)
			{
				rawItemName = item.Name.Replace("item_", "");
				// $"http://cdn.dota2.com/apps/dota2/images/items/{rawItemName}_lg.png";
				string itemImage = GetImageURL(Entity.items, rawItemName, ImageSize.lg);

				Item = new Item
				{
					Name = item.Localized_Name,
					ID = item.ID,
					ImageURL = itemImage
				};

				resultDic.Add(item.ID, Item);
				itemCountInt++;
			}

			return resultDic;
		}

		/// <summary>
		/// Gets the latest list of heroes from Steam, requires "HerosClass".
		/// </summary>
		private static Dictionary<int, Hero> GetHeroes()
		{
			string response = DownloadSteamAPIString(HEROSURL, API);
			HeroesObject heroesObject = JsonConvert.DeserializeObject<HeroesObject>(response);
			Dictionary<int, Hero> resultDic = new Dictionary<int, Hero>();

			int herocountInt = 1;

			//hero id 0 is private profile i think?
			Hero Hero = new Hero
			{
				ID = 0,
				Name = "Npc_dota_hero_Private Profile"
			};

			resultDic.Add(Hero.ID, Hero);
			string rawHeroName = "";

			foreach (var hero in heroesObject.Result.Heroes)
			{

				rawHeroName = hero.Name.Replace("npc_dota_hero_", "");
				// http://cdn.dota2.com/apps/dota2/images/heroes/{rawHeroName}_lg.png
				string heroImage = GetImageURL(Entity.heroes, rawHeroName, ImageSize.sb);

				Hero = new Hero
				{
					Name = hero.Localized_Name,
					ID = hero.ID,
					OrigName = hero.Name,
					ImageURL = heroImage
				};

				resultDic.Add(Hero.ID, Hero);
				herocountInt++;
			}

			return resultDic;
		}

		private class MatchDetailsRootObject
		{
			public MatchDetailsModel Result { get; set; }
		}

		#endregion

		#region SteamAccount

		/// <summary>
		/// Gets the Steam account details for a particular user ID, requires "DotaApi.Model.SteamAccount".
		/// </summary>
		public static SteamAccountPlayerModel GetSteamAccount(string SteamID)
		{
			SteamAccountPlayerModel player = new SteamAccountPlayerModel();

			if (string.IsNullOrEmpty(SteamID))
				return player;

			string webResponse = string.Empty;
			var steamaccount = new SteamAccountRootObject();
			webResponse = DownloadSteamAPIString(STEAMACCOUNTURL, (API + "&steamids=" + StringManipulation.SteamIDConverter(SteamID)));

			// webResponse = GetWebResponse.DownloadSteamAPIString(@"http://api.opendota.com/api/players/347142169", (Common.API + "&steamids=" + StringManipulation.SteamIDConverter(SteamID)));

			SteamAccountRootObject jObj = JsonConvert.DeserializeObject<SteamAccountRootObject>(webResponse);

			if (jObj.Response.Players.Count > 0)
				player = jObj.Response.Players[0];

			return player;
		}

		private static string DownloadSteamAPIString(string uri, string api, string language = "&language=en")
		{
			//if (!string.IsNullOrEmpty(language))
			//language += "&language=en";
			var response = string.Empty;
			Uri getmatchUri = new Uri(uri + api + language);

			// client used to download the json response
			using (WebClient client = new WebClient())
			{
				// downloading the json response
				response = client.DownloadString(getmatchUri);
			}
			return response;
		}

		public class SteamAccountResponse
		{
			public List<SteamAccountPlayerModel> Players { get; set; }
		}

		public class SteamAccountRootObject
		{
			public SteamAccountResponse Response { get; set; }
		}

		#endregion

		//public static List<Item> ParseItemsText(string[] text)
		//{
		//	bool itemfound = false;

		//	// List to hold our parsed items.
		//	List<Item> items = new List<Item>();

		//	// This will be used to store this section of the item.
		//	List<string> curitem = new List<string>();

		//	// Item object will be populating
		//	Item item = new Item();

		//	// Lets go line by line to start parsing.
		//	foreach (string line in text)
		//	{
		//		// Clean up the text, remove quotes.
		//		string line_noquotes = line.Replace("\"", "");
		//		string trimmed_clean = line.Replace("\"", "").Replace("\t", "").Replace("_", " ").Trim();

		//		// If line starts with item_ then
		//		// This is where we will start capturing.
		//		if (line_noquotes.StartsWith("	item_"))
		//		{
		//			item = new Item();
		//			itemfound = true;
		//			item.Name = trimmed_clean.Replace("item ", "");
		//			item.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(item.Name);
		//			curitem.Add(trimmed_clean);
		//		}

		//		// If we are on a current item then lets do
		//		// Some other operations to gather details
		//		if (itemfound == true)
		//		{
		//			if (trimmed_clean.StartsWith("ID"))
		//			{
		//				item.ID = System.Convert.ToInt32(trimmed_clean.Replace("ID", "").Split('/')[0]);
		//				curitem.Add(line);
		//			}

		//			if (trimmed_clean.StartsWith("AbilityCastRange"))
		//			{
		//				item.CastRange = trimmed_clean.Replace("AbilityCastRange", "");
		//				curitem.Add(trimmed_clean);
		//			}

		//			if (trimmed_clean.StartsWith("AbilityCastPoint"))
		//			{
		//				item.CastPoint = trimmed_clean.Replace("AbilityCastPoint", "");
		//				curitem.Add(trimmed_clean);
		//			}

		//			if (trimmed_clean.StartsWith("AbilityCooldown"))
		//			{
		//				item.Cooldown = trimmed_clean.Replace("AbilityCooldown", "");
		//				curitem.Add(trimmed_clean);
		//			}

		//			if (trimmed_clean.StartsWith("AbilityManaCost"))
		//			{
		//				item.ManaCost = trimmed_clean.Replace("AbilityManaCost", "");
		//				curitem.Add(trimmed_clean);
		//			}

		//			if (trimmed_clean.StartsWith("ItemCost"))
		//			{
		//				item.ItemCost = trimmed_clean.Replace("ItemCost", "");
		//				curitem.Add(trimmed_clean);
		//			}

		//			if (trimmed_clean.StartsWith("ItemShopTags"))
		//			{
		//				item.ItemShopTags = trimmed_clean.Replace("ItemShopTags", "");
		//				curitem.Add(trimmed_clean);
		//			}

		//			if (trimmed_clean.StartsWith("ItemQuality"))
		//			{
		//				item.ItemQuality = trimmed_clean.Replace("ItemQuality", "");
		//				curitem.Add(trimmed_clean);
		//			}

		//			if (trimmed_clean.StartsWith("ItemAliases"))
		//			{
		//				if (string.IsNullOrEmpty(item.ItemAliases))
		//				{
		//					item.ItemAliases = trimmed_clean.Replace("ItemAliases", "");
		//					int index = item.ItemAliases.LastIndexOf(";") == -1 ? 0 : item.ItemAliases.LastIndexOf(";");
		//					item.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(item.ItemAliases.Substring(index).Replace(";", ""));
		//					curitem.Add(trimmed_clean);
		//				}
		//			}

		//			if (trimmed_clean.StartsWith("ItemStackable"))
		//			{
		//				item.ItemStackable = trimmed_clean.Replace("ItemStackable", "");
		//				curitem.Add(trimmed_clean);
		//			}

		//			if (trimmed_clean.StartsWith("ItemShareability"))
		//			{
		//				item.ItemShareability = trimmed_clean.Replace("ItemShareability", "");
		//				curitem.Add(trimmed_clean);
		//			}

		//			if (trimmed_clean.StartsWith("ItemShareability"))
		//			{
		//				item.ItemShareability = trimmed_clean.Replace("ItemShareability", "");
		//				;
		//				curitem.Add(trimmed_clean);
		//			}

		//			//end current item, save to list
		//			if (trimmed_clean.StartsWith("//="))
		//			{
		//				//add to our list of items/
		//				items.Add(item);
		//				curitem.Add(trimmed_clean);
		//				itemfound = false;
		//			}
		//		}
		//	}
		//	return items;
		//}
	}
}
