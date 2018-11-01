using System;
using System.Collections.Generic;

namespace DotaWeb.Models
{
	public class MatchDetailsModel
	{
		public IEnumerable<MatchDetailsPlayerModel> Players { get; set; }
		public bool Radiant_Win { get; set; }
		public int Duration { get; set; }
		public string DurationStr { get; set; }
		public int Start_Time { get; set; }
		public DateTime StartTime { get; set; }
		public string Start_TimeStr { get; set; }
		public string PlayedTimeAgo { get; set; }
		public long Match_ID { get; set; }
		public long Match_Seq_Num { get; set; }
		public int Tower_Status_Radiant { get; set; }
		public int Tower_Status_Dire { get; set; }
		public int Barracks_Status_Radiant { get; set; }
		public int Barracks_Status_Dire { get; set; }
		public int Cluster { get; set; }
		public int First_Blood_Time { get; set; }
		public string First_Blood_TimeStr { get; set; }
		public int Lobby_Type { get; set; }
		public string Lobbytype { get; set; }
		public int Human_Players { get; set; }
		public int Leagueid { get; set; }
		public int Positive_Votes { get; set; }
		public int Negative_Votes { get; set; }
		public int Game_Mode { get; set; }
		public string Game_ModeStr { get; set; }
	}
}