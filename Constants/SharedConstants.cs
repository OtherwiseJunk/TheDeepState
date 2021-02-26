using System;
using System.Collections.Generic;
using System.Text;

namespace TheDeepState.Constants
{
	public class SharedConstants
	{
		#region String Formats
		public static string ReplacedMessageFormat(string username, string modifiedMessage) => $"**{username}:** {modifiedMessage}";
		#endregion

		#region Discord ID Variables

		#region Emote Ids
		public static string BogId = "<:bog:763615501992591361>";
		public static string ConcernedFroggyId = "<:concernfroggy:737512429310836806>";
		public static string ThisTBHId = "<:ThisTBH:339832219818524672>";
		public static string ForeheadID = "<:4head:724065591077371915>";
		#endregion

		#endregion
	}
}
