using System.Collections.Generic;

namespace DotaApi.Model
{
	public class Item : ISearchableDictionary
	{
		public string Name { get; set; }
		public int ID { get; set; }
		public string Cost { get; set; }
		public string Side_Shop { get; set; }
		public string ItemImage { get; set; }
		public string Localized_Name { get; set; }
		//public string CastRange { get; set; }
		//public string CastPoint { get; set; }
		//public string Cooldown { get; set; }
		//public string ManaCost { get; set; }
		//public string ItemShopTags { get; set; }
		//public string ItemQuality { get; set; }
		//public string ItemAliases { get; set; }
		//public string ItemStackable { get; set; }
		//public string ItemShareability { get; set; }
		//public string ItemPermanent { get; set; }
		//public string ItemInitialCharges { get; set; }
		//public string AbilitySharedCooldown { get; set; }
		//public string AbilityChannelTime { get; set; }

		public class ItemsRoot
		{
			public List<Item> Items { get; set; }
			public int Count { get; set; }
		}

		public class ItemsObject
		{
			public ItemsRoot Result { get; set; }
		}
	}
}
