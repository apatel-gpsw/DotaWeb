namespace DotaApi.Model
{
	public class Ability : ISearchableDictionary
	{
		public string Name { get; set; }
		public string HeroName { get; set; }
		public int ID { get; set; }
		public string AbilityType { get; set; }
		public string AbilityBehavior { get; set; }
		public string OnCastbar { get; set; }
		public string OnLearnbar { get; set; }
		public string FightRecapLevel { get; set; }
		public string AbilityUnitTargetTeam { get; set; }
		public string AbilityUnitTargetType { get; set; }
		public string AbilityUnitDamageType { get; set; }
		public string AbilityCastRange { get; set; }
		public string AbilityCastRangeBuffer { get; set; }
		public string AbilityCastPoint { get; set; }
		public string AbilityChannelTime { get; set; }
		public string AbilityCooldown { get; set; }
		public string AbilityDuration { get; set; }
		public string AbilitySharedCooldown { get; set; }
		public string AbilityDamage { get; set; }
		public string AbilityManaCost { get; set; }
		public string AbilityModifierSupportValue { get; set; }
		public string ItemCost { get; set; }
		public string ItemInitialCharges { get; set; }
		public string ItemCombinable { get; set; }
		public string ItemPermanent { get; set; }
		public string ItemStackable { get; set; }
		public string ItemRecipe { get; set; }
		public string ItemDroppable { get; set; }
		public string ItemPurchasable { get; set; }
		public string ItemSellable { get; set; }
		public string ItemRequiresCharges { get; set; }
		public string ItemKillable { get; set; }
		public string ItemDisassemblable { get; set; }
		public string ItemShareability { get; set; }
		public string ItemDeclaresPurchase { get; set; }
		public string AbilityImage { get; set; }
	}
}
