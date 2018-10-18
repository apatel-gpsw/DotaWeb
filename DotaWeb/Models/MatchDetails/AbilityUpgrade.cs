using System;

namespace DotaWeb.Models
{
	public class AbilityUpgrade
	{
		public string Ability { get; set; }
		public string Name { get; set; }
		public int Time { get; set; }
		public DateTime UpgradeTime { get; set; }
		public int Level { get; set; }
		public string ID { get; set; }
	}
}