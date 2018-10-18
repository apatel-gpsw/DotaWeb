using System;

namespace DotaApi.Helpers
{
	public class StringManipulation
	{
		/// <summary>
		/// Converts UTC time to local time.
		/// <seealso cref="https://stackoverflow.com/questions/249760/how-to-convert-unix-timestamp-to-datetime-and-vice-versa"/>
		/// </summary>
		/// <remarks>UnixTimeStampToDateTime is a part of the Dota 2 helper class.</remarks>
		public static DateTime UnixTimeStampToDateTime(int unixTimeStamp)
		{
			// Unix timestamps is seconds past epoch
			DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
			return dtDateTime;
		}

		/// <summary>
		/// SteamIDConverter will convert the input account id to its opposite.
		/// <para>If SteamAccountID is a 32bit number, this method will convert it to a 64bit and vice versa. <see cref="System.Console.WriteLine(System.String)"/> for information about output statements.</para>
		/// <seealso cref="https://gist.github.com/almirsarajcic/4664387"/>
		/// </summary>
		public static string SteamIDConverter(string SteamAccountID)
		{
			// Found a cool way in PHP to convert steam ID so I adopted the method, credit
			// goes to original author, page here: https://gist.github.com/almirsarajcic/4664387

			if (string.IsNullOrEmpty(SteamAccountID))
				return null;
			// If the length of the SteamID is 17 characters
			// we will assume it is a 64 id
			else if (SteamAccountID.Length == 17)
				return SteamIDConverter64to32(SteamAccountID);
			else
				return SteamIDConverter32to64(SteamAccountID);
		}

		/// <summary>
		/// Converts a 32bit Steam account ID to a 64bit account ID.
		/// <para>If SteamAccountID is a 32bit number, this method will convert it to a 64bit.
		/// 32bit account id input = 32500026
		/// 64bit account id output = 76561197992765754</para>
		/// <seealso cref="https://gist.github.com/almirsarajcic/4664387"/>
		/// </summary>
		public static string SteamIDConverter32to64(string SteamAccountID)
		{
			decimal steamidDec = Convert.ToDecimal(SteamAccountID);
			string converted_id = "765" + (steamidDec + 61197960265728).ToString();

			return converted_id;
		}

		/// <summary>
		/// Converts a 64bit Steam account ID to a 32bit account ID.
		/// <para>If SteamAccountID is a 64bit number, this method will convert it to a 32bit.
		/// 32bit account id input = 76561197992765754
		/// 64bit account id output = 32500026</para>
		/// <seealso cref="https://gist.github.com/almirsarajcic/4664387"/>
		/// </summary>
		public static string SteamIDConverter64to32(string SteamAccountID)
		{
			// There is a rare occurrence when I get a null
			// steam-id and that is why we are using try/catch
			try
			{
				// We remove the prefix of 765 then do the math
				decimal steamidDec = Convert.ToDecimal(SteamAccountID.Substring(3));
				string converted_id = (steamidDec - 61197960265728).ToString();

				return converted_id;
			}
			catch (Exception)
			{
				return SteamAccountID;
			}
		}
	}
}
