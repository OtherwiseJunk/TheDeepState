using Discord;
using System.Collections.Generic;

namespace DeepState.Constants
{
	public class SharedConstants
	{
		#region Discord ID Variables

		#region Emote Ids
		public static string BogId = "<:bog:763615501992591361>";
		public static string ConcernedFroggyId = "<:concernfroggy:737512429310836806>";
		public static string ThisTBHId = "<:ThisTBH:339832219818524672>";
		public static string ForeheadID = "<:4head:724065591077371915>";
		public static string LaughingFaceID = "<:emoji:707407221792702525>";
		public static string BooHooCrackerID = "<:boohoo_cracker:783170303559729162>";
		public static string GwalmsID = "<:gwalms:793892038134857728>";
		public static string BonkID = "<:bonk:759126452112326697>";
		public static string YouAreWhiteID = "<:white:785272845890486293>";
		public static string QIID = "<:QI:824733109240332308>";
		public static string RomneyRightEyeID = "<:romneyrighteye:809203888858464307>";
		public static string RomneyLeftEyeID = "<:romneylefteye:809203904071598180>";
		public static string SusID = "<:sus:811454356914044968>";
		#endregion

		#region Channel Ids
		public static ulong SelfCareChannelId = 736092567497867284;
		public static ulong SeriousDiscussionChannelId = 732019626367516682;
		#endregion

		#region Channel Lists
		public static List<ulong> NoAutoReactsChannel = new List<ulong>
		{
			SelfCareChannelId,
			SeriousDiscussionChannelId
		};
		#endregion

		#region User Ids
		public static ulong ThePoliceUser = 151162710757867521;
		public static ulong TheCheatingUser = 696443422130831370;
		#endregion

		#endregion

		#region String Lists
		public static List<string> SelfReactResponses = new List<string>
		{
			"BOOHOO CRACKER! No one cares about what emotes you think we should react with.",
			"Imagine being the first reaction on your own message.",
			"DAE think reacting to your own messages is lame as hell?",
			"We get it, you want us to notice you.",
			"BOOHOO CRACKER! No body cares about your self-react, plus you're white.",
			$"{Emote.Parse(YouAreWhiteID)} SELF-REACT DETECTED {Emote.Parse(YouAreWhiteID)}."
		};

		public static readonly List<string> RankNerdResponses = new List<string>

		{
			"https://m.media-amazon.com/images/I/91umiveo5mL._SS500_.jpg",
			"https://i.imgur.com/PvT6XMa.gif",
			"https://i.imgur.com/1Y8Czxu.gif",
			"https://i.gifer.com/9clm.gif",
			"https://i.imgur.com/Ek2X3Hw.gif"
		};

		public static readonly List<string> ReactableEmotes = new List<string>
		{
			SharedConstants.BogId,
			SharedConstants.ConcernedFroggyId,
			SharedConstants.ThisTBHId,
			SharedConstants.ForeheadID,
			SharedConstants.BooHooCrackerID,
			SharedConstants.LaughingFaceID,
			SharedConstants.BonkID
		};
		#endregion

		#region Emote Lists
		public static List<string> VotingEmotes = new List<string>
		{
			"✅",
			"❌"
		};

		public static List<string> ClearingEmotes = new List<string>
		{
			"📵",
			"❌"
		};
		#endregion

		#region Strings
		public static string KlaxonResponse = "https://www.youtube.com/watch?v=xU5mOT57ghM";
		public static string SusRegex = ".*[(sus)(amo.*gus)].*";
		#endregion

		#region String Formats
		public static string EmoteNameandId(string name, ulong? id) => $"<:{name}:{id}>";
		#endregion
	}
}
