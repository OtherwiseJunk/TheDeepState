using Discord;
using System.Collections.Generic;

namespace DeepState.Constants
{
	public class SharedConstants
	{
		#region Discord ID Variables

		#region Guild Ids
		public const ulong LibcraftGuildId = 698639095940907048;
		#endregion

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
		public static string GodIWishThatWereMeID = "<:godiwishthatwereme:929146317958152213>";
		public static string RageID = "<:rage:929155553735876668>";
		#endregion

		#region Channel Ids
		public const ulong SelfCareChannelId = 736092567497867284;
		public const ulong SeriousDiscussionChannelId = 732019626367516682;
		public const ulong DiscordSuggestionsChannelId = 740033615617982514;
		public const ulong RequestsChannelId = 858490606145634354;
		public const ulong LCBotCommandsChannel = 718986327642734654;
		public const ulong TestChannel = 176357319687405569;
		public const ulong LCShitpostChannelId = 855227586212134922;
		public const ulong UkraineInfoThread = 941783493476753408;
		public const ulong UkraineMemeThread = 946913420219932712;
		#endregion

		#region Channel Lists
		public static List<ulong> NoAutoReactsChannel = new List<ulong>
		{
			SelfCareChannelId,
			SeriousDiscussionChannelId,
			UkraineInfoThread,
			UkraineMemeThread
		};
		#endregion

		#region User Ids
		public static ulong ThePoliceUser = 151162710757867521;
		public static ulong TheCheatingUser = 696443422130831370;
		public static ulong TheDad = 622578719252283432;
		public static ulong TheCheeselessQuesadillaUser = 144986616467947520;
		public static ulong TheBotmaker = 94545463906144256;
		#endregion

		#region User Id Lists
		public static List<ulong> KnownSocks = new List<ulong>
		{
			883895808238432346			
		};
		#endregion

		#region Role ID Lists
		public static List<string> PronounLowercaseRoleNames = new List<string> {
			MasculinePronounRoleName,
			FemininePronnounRoleName,
			NongenderedPronounRolename
		};
		#endregion

		#endregion

		#region String Lists

		public static List<string> JonathanFrakesThatsNotTrue = new List<string>
		{
			"https://c.tenor.com/EMcrFOZAnswAAAAC/beyond-belief-jonathan-frakes.gif",
			"https://c.tenor.com/GdzJAhvk9JgAAAAC/no-jonathan-frakes.gif",
			"https://c.tenor.com/bQT4sSN0KFUAAAAC/beyond-belief-jonathan-frakes.gif",
			"https://c.tenor.com/nrVNaZJaCrkAAAAC/beyond-belief-jonathan-frakes.gif",
			"https://c.tenor.com/EMcrFOZAnswAAAAC/beyond-belief-jonathan-frakes.gif",
			"https://c.tenor.com/GdzJAhvk9JgAAAAC/no-jonathan-frakes.gif",
			"https://c.tenor.com/bQT4sSN0KFUAAAAC/beyond-belief-jonathan-frakes.gif",
			"https://c.tenor.com/nrVNaZJaCrkAAAAC/beyond-belief-jonathan-frakes.gif",
			"https://64.media.tumblr.com/30da3f51d5895c32415ef8358509498a/51eae155c32bac69-56/s500x750/0b13325749d330f3f0ecc1232e544b7568971a19.gifv",
			"https://c.tenor.com/BeGiUvvMqtwAAAAC/beyond-belief-jonathan-frakes.gif",
			"https://thumbs.gfycat.com/ElectricPessimisticBull-size_restricted.gif",
			"https://thumbs.gfycat.com/HelpfulTidyBaleenwhale-max-1mb.gif",
			"https://64.media.tumblr.com/30da3f51d5895c32415ef8358509498a/51eae155c32bac69-56/s500x750/0b13325749d330f3f0ecc1232e544b7568971a19.gifv",
			"https://c.tenor.com/BeGiUvvMqtwAAAAC/beyond-belief-jonathan-frakes.gif",
			"https://thumbs.gfycat.com/ElectricPessimisticBull-size_restricted.gif",
			"https://thumbs.gfycat.com/HelpfulTidyBaleenwhale-max-1mb.gif",
			"https://c.tenor.com/JzRolGfSnGAAAAAC/riker-frakes.gif",
			"https://preview.redd.it/1orbrmelz9s21.jpg?auto=webp&s=cab6beac53a7a8ccd8b13df8099b27a6aae01c3b",
			"https://c.tenor.com/qtEFTzvo7l8AAAAM/beyond-belief-jonathan-frakes.gif",
			"https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQhnSdy3r99Fac92MgpzhDvY0GdzOVFOJW-_A&usqp=CAU",
			"https://media.tenor.com/images/3d21bb8ab5ef6209c7ff8cbf85fcfa90/tenor.gif"
		};

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
			SharedConstants.BonkID,
			SharedConstants.GodIWishThatWereMeID
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
		public static List<IEmote> MalarkeyLevels = new List<IEmote>
		{
			new Emoji("\u0031\ufe0f\u20e3"),
			new Emoji("\u0031\ufe0f\u20e3"),
			new Emoji("\u0031\ufe0f\u20e3"),
			new Emoji("\u0031\ufe0f\u20e3"),
			new Emoji("\u0031\ufe0f\u20e3"),

			new Emoji("\u0032\ufe0f\u20e3"),
			new Emoji("\u0032\ufe0f\u20e3"),
			new Emoji("\u0032\ufe0f\u20e3"),
			new Emoji("\u0032\ufe0f\u20e3"),
			new Emoji("\u0032\ufe0f\u20e3"),

			new Emoji("\u0033\ufe0f\u20e3"),
			new Emoji("\u0033\ufe0f\u20e3"),
			new Emoji("\u0033\ufe0f\u20e3"),
			new Emoji("\u0033\ufe0f\u20e3"),
			new Emoji("\u0033\ufe0f\u20e3"),

			new Emoji("\u0034\ufe0f\u20e3"),
			new Emoji("\u0034\ufe0f\u20e3"),
			new Emoji("\u0034\ufe0f\u20e3"),
			new Emoji("\u0034\ufe0f\u20e3"),
			new Emoji("\u0034\ufe0f\u20e3"),

			new Emoji("\u0035\ufe0f\u20e3"),
			new Emoji("\u0035\ufe0f\u20e3"),
			new Emoji("\u0035\ufe0f\u20e3"),
			new Emoji("\u0035\ufe0f\u20e3"),
			new Emoji("\u0035\ufe0f\u20e3"),

			new Emoji("\u0036\ufe0f\u20e3"),
			new Emoji("\u0036\ufe0f\u20e3"),
			new Emoji("\u0036\ufe0f\u20e3"),
			new Emoji("\u0036\ufe0f\u20e3"),
			new Emoji("\u0036\ufe0f\u20e3"),

			new Emoji("\u0037\ufe0f\u20e3"),
			new Emoji("\u0037\ufe0f\u20e3"),
			new Emoji("\u0037\ufe0f\u20e3"),
			new Emoji("\u0037\ufe0f\u20e3"),
			new Emoji("\u0037\ufe0f\u20e3"),

			Emote.Parse(BooHooCrackerID),
			Emote.Parse(BogId)
		};
		#endregion

		#region Strings
		public static string KlaxonResponse = "https://www.youtube.com/watch?v=xU5mOT57ghM";
		public static string SusRegex = @"(\bs+u+s+y?\b)|(\bamo+\w{0,2}\s*us)";
		public static string SubjectiveNonGenderedPronoun = "they";
		public static string SubjectiveFemininePronoun = "she";
		public static string SubjectiveMasculinePronoun = "he";
		public static string ObjectiveNonGenderedPronoun = "them";
		public static string ObjectiveFemininePronoun = "her";
		public static string ObjectiveMasculinePronoun = "him";
		public static string PossessiveAdjectiveNonGenderedPronoun = "their";
		public static string PossessiveAdjectiveFemininePronoun = "her";
		public static string PossessiveAdjectiveMasculinePronoun = "his";
		public static string PossesiveNonGenderedPronoun = "theirs";
		public static string PossesiveFemininePronoun = "hers";
		public static string PossesiveMasculinePronoun = "his";
		public static string ReflexiveNonGenderedPronoun = "they";
		public static string ReflexiveFemininePronoun = "she";
		public static string ReflexiveMasculinePronoun = "he";
		public static string MasculinePronounRoleName = @"he/him";
		public static string FemininePronnounRoleName = @"she/her";
		public static string NongenderedPronounRolename = @"they/them";
		public const string AdminsOnlyGroup = "AdminsOnly";
		public const string HungerGamesRegistrationDateGroup = "HGRegistrationPeriod";
		#endregion

		#region String Formats
		public static string EmoteNameandId(string name, ulong? id) => $"<:{name}:{id}>";
		#endregion

		#region Enums
		public enum PronounConjugations
		{
			Subjective,
			Objective,
			PossessiveAdjective,
			Possessive,
			Reflexive
		}
		public enum ConfiguredPronouns
		{
			Nongendered,
			Feminine,
			Masculine
		}
		#endregion
	}
}
