using System.Collections.Generic;

namespace DotaWeb.Models
{
	public class Player
	{
		public string Account_ID { get; set; }
		public string Name { get; set; }
		public string PlayerName { get; set; }
		public string Steamid64 { get; set; }
		public string Steamid32 { get; set; }
		public string ProfileUrl { get; set; }
		public int PlayerSlot { get; set; }
		public int Hero_ID { get; set; }
		public int Item_0 { get; set; }
		public string Item0 { get; set; }
		public int Item_1 { get; set; }
		public string Item1 { get; set; }
		public int Item_2 { get; set; }
		public string Item2 { get; set; }
		public int Item_3 { get; set; }
		public string Item3 { get; set; }
		public int Item_4 { get; set; }
		public string Item4 { get; set; }
		public int Item_5 { get; set; }
		public string Item5 { get; set; }
		public int Kills { get; set; }
		public int Deaths { get; set; }
		public int Assists { get; set; }
		public int Leaver_Status { get; set; }
		public int Gold { get; set; }
		public int Last_Hits { get; set; }
		public int Denies { get; set; }
		public int Gold_Per_Min { get; set; }
		public int Xp_Per_Min { get; set; }
		public int Gold_Spent { get; set; }
		public int Hero_Damage { get; set; }
		public int Tower_Damage { get; set; }
		public int Hero_Healing { get; set; }
		public int Level { get; set; }
		public int Backpack_0 { get; set; }
		public string Backpack0 { get; set; }
		public int Backpack_1 { get; set; }
		public string Backpack1 { get; set; }
		public int Backpack_2 { get; set; }
		public string Backpack2 { get; set; }
		public List<AbilityUpgrade> Ability_Upgrades { get; set; }
	}
}