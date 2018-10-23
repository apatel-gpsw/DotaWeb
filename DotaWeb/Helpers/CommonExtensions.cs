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
		#region MatchDetails

		/// <summary>
		/// Gets match details for a single match, this includes player builds and details. Requires "MatchClass".
		/// </summary>
		public static MatchDetailsModel GetMatchDetail(long matchid)
		{
			List<Item> items = GetGameItems(false);
			List<Hero> heroes = GetHeroes(false);
			List<Ability> abilities = ParseAbilityText();

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
				//StringBuilder sb = new StringBuilder();

				//sb.AppendLine($"\nAccount ID: {player.Account_ID}");
				player.Name = ConvertIDtoName(player.Hero_ID, heroes);
				player.HeroImage = GetImageURL(Entity.heroes, player.Name.ToLower().Replace(" ", "_"), ImageSize.sb);
				player.Steamid64 = StringManipulation.SteamIDConverter(player.Account_ID);
				player.Steamid32 = StringManipulation.SteamIDConverter64to32(player.Steamid64);

				SteamAccountPlayerModel steamaccount = GetSteamAccount(player.Account_ID);
				player.PlayerName = string.IsNullOrEmpty(steamaccount.PersonaName) ? "Anonymous" : steamaccount.PersonaName;

				//sb.AppendLine($"Player Name: {player.PlayerName}");
				//sb.AppendLine($"Hero: {player.Name}");
				//sb.AppendLine($"\tHero Level: {player.Level}");
				//sb.AppendLine($"K/D/A: {player.Kills}/{player.Deaths}/{player.Assists}");
				//sb.AppendLine($"CS: {player.Last_Hits}/{player.Denies}");
				//sb.AppendLine($"\tGPM: {player.Gold_Per_Min}");
				//sb.AppendLine($"\tXPM: {player.Xp_Per_Min}");

				// getting item names based on the id number
				player.Item0 = player.Item_0 > 0 ? ConvertIDtoName(player.Item_0, items) : null;
				player.Item1 = player.Item_1 > 0 ? ConvertIDtoName(player.Item_1, items) : null;
				player.Item2 = player.Item_2 > 0 ? ConvertIDtoName(player.Item_2, items) : null;
				player.Item3 = player.Item_3 > 0 ? ConvertIDtoName(player.Item_3, items) : null;
				player.Item4 = player.Item_4 > 0 ? ConvertIDtoName(player.Item_4, items) : null;
				player.Item5 = player.Item_5 > 0 ? ConvertIDtoName(player.Item_5, items) : null;

				player.Backpack0 = player.Backpack_0 > 0 ? ConvertIDtoName(player.Backpack_0, items) : null;
				player.Backpack1 = player.Backpack_1 > 0 ? ConvertIDtoName(player.Backpack_1, items) : null;
				player.Backpack2 = player.Backpack_2 > 0 ? ConvertIDtoName(player.Backpack_2, items) : null;

				//sb.AppendLine("Items:");
				//if (!string.IsNullOrEmpty(player.Item0))
				//	sb.Append($"Slot 1: {player.Item0}");
				//if (!string.IsNullOrEmpty(player.Item1))
				//	sb.Append($" | Slot 2: {player.Item1}");
				//if (!string.IsNullOrEmpty(player.Item2))
				//	sb.Append($" | Slot 3: {player.Item2}");
				//if (!string.IsNullOrEmpty(player.Item3))
				//	sb.Append($" | Slot 4: {player.Item3}");
				//if (!string.IsNullOrEmpty(player.Item4))
				//	sb.Append($" | Slot 5: {player.Item4}");
				//if (!string.IsNullOrEmpty(player.Item5))
				//	sb.Append($" | Slot 6: {player.Item5}");
				//sb.AppendLine();

				//if (!string.IsNullOrEmpty(player.Backpack0))
				//	sb.Append($"Backpack Slot 1: {player.Backpack0}");
				//if (!string.IsNullOrEmpty(player.Backpack1))
				//	sb.Append($" | Backpack  Slot 2: {player.Backpack1}");
				//if (!string.IsNullOrEmpty(player.Backpack2))
				//	sb.Append($" | Backpack Slot 3: {player.Backpack2}");
				//sb.AppendLine();
				//sb.Append("Ability Upgrade Path:");
				//sb.AppendLine();

				// ability output
				// In some scenarios a user might play the game
				// but not upgrade any abilities or he/she
				// might get kicked out before they get a chance.
				// In this type of situation, we must check for null
				// before continuing.
				if (player.Ability_Upgrades != null)
				{
					foreach (var ability in player.Ability_Upgrades)
					{
						// clean up the object a bit and put
						// id where it should be.
						ability.ID = ability.Ability;

						// map the id to a readable name.
						ability.Name = ConvertIDtoName(Convert.ToInt32(ability.Ability), abilities);

						// add the upgrade seconds to the original start
						// time to get the upgrade time.
						ability.UpgradeTime = match.StartTime.AddSeconds(ability.Time);

						// output to screen
						// sb.AppendLine($" {ability.Name} upgraded at {ability.Level} @ {ability.UpgradeTime}");
					}
				}
				else
				{
					// sb.AppendLine("No abilities data");
				}
				// Console.WriteLine(sb.ToString());
			}
			return match;
		}

		/// <summary>
		/// Generic Method using <see cref="ISearchableDictionary"/> properties.
		/// Converts incoming List into a dictionary and then returns Name corresponding to the ID.
		/// Used for Items, Abilities and Heros.
		/// </summary>
		private static string ConvertIDtoName<T>(int id, List<T> list) where T : ISearchableDictionary
		{
			var itemDict = list.ToDictionary(x => x.ID, x => x.Name);
			string name = itemDict.Where(x => x.Key == id).FirstOrDefault().Value;

			return name != null ? CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name) : "";
		}

		/// <summary>
		/// Reads the npc_abilities.txt and builds the abilities class.
		/// </summary>
		private static List<Ability> ParseAbilityText()
		{
			string abilityFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "App_Data", ABILITY_FILE);

			var text = File.ReadAllLines(abilityFile);
			bool itemfound = false;

			// List to hold our parsed items.
			List<Ability> abilities = new List<Ability>();

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
					ability.ID = System.Convert.ToInt32(trimmed_clean.Replace("ID", "").Split('/')[0].Trim());

					// We get the hero name
					ability.HeroName = text[count - 4].Split('_')[0].Replace("\"", "").Replace("\t", "").Trim();

					rawAbilityName = text[count - 4].Replace(ability.HeroName, "").Replace("\"", "").Replace("\t", "").Trim();
					ability.AbilityImage = GetImageURL(Entity.abilities, ability.HeroName + rawAbilityName, ImageSize.md);

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
						abilities.Add(ability);
						itemfound = false;
					}
				}
				count++;
			}
			return abilities;
		}

		/// <summary>
		/// Gets the latest list of items from Steam.
		/// </summary>
		private static List<Item> GetGameItems(bool verbose)
		{
			string response = DownloadSteamAPIString(ITEMSURL, API);
			ItemsObject itemsObject = JsonConvert.DeserializeObject<ItemsObject>(response);
			List<Item> resultItems = new List<Item>();

			//if verbose flag is set to true, then show console
			//message.
			if (verbose == true)
				Console.WriteLine("Count of Heroes {0}", itemsObject.Result.Items.Count);

			int itemCountInt = 0;
			Item Item;
			string rawItemName = "";
			foreach (var item in itemsObject.Result.Items)
			{
				if (verbose == true)
					Console.Write("{0} of {1}. Item localized-name: {2}|", itemCountInt, itemsObject.Result.Items.Count, item.Localized_Name);

				rawItemName = item.Name.Replace("item_", "");
				// $"http://cdn.dota2.com/apps/dota2/images/items/{rawItemName}_lg.png";
				string itemImage = GetImageURL(Entity.items, rawItemName, ImageSize.lg);

				Item = new Item
				{
					Name = item.Localized_Name,
					ID = item.ID,
					ItemImage = itemImage
				};
				if (verbose == true)
					Console.WriteLine(" cleaned: {0}", Item.Name);

				resultItems.Add(Item);
				itemCountInt++;
			}

			return resultItems;
		}

		/// <summary>
		/// Gets the latest list of heroes from Steam, requires "HerosClass".
		/// </summary>
		private static List<Hero> GetHeroes(bool verbose)
		{
			string response = DownloadSteamAPIString(HEROSURL, API);
			HeroesObject heroesObject = JsonConvert.DeserializeObject<HeroesObject>(response);
			List<Hero> resultHeroes = new List<Hero>();

			//if verbose flag is set to true, then show console
			//message.
			if (verbose == true)
				Console.WriteLine("Count of Heroes {0}", heroesObject.Result.Heroes.Count);

			int herocountInt = 1;

			//hero id 0 is private profile i think?
			Hero Hero = new Hero
			{
				ID = 0,
				Name = "Npc_dota_hero_Private Profile"
			};
			if (verbose == true)
				Console.WriteLine("Hero orig-name: {0}", Hero.Name);

			resultHeroes.Add(Hero);
			string rawHeroName = "";

			foreach (var hero in heroesObject.Result.Heroes)
			{
				if (verbose == true)
					Console.Write("{0} of {1}. Hero orig-name: {2}|", herocountInt, heroesObject.Result.Heroes.Count, hero.Name);

				rawHeroName = hero.Name.Replace("npc_dota_hero_", "");
				// http://cdn.dota2.com/apps/dota2/images/heroes/{rawHeroName}_lg.png
				string heroImage = GetImageURL(Entity.heroes, rawHeroName, ImageSize.lg);

				Hero = new Hero
				{
					Name = hero.Localized_Name,
					ID = hero.ID,
					OrigName = hero.Name,
					HeroImage = heroImage
				};
				if (verbose == true)
					Console.WriteLine(" cleaned: {0}", Hero.Name);

				resultHeroes.Add(Hero);
				herocountInt++;
			}

			return resultHeroes;
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
