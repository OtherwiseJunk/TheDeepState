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

		public static List<string> FuckYouGifs = new()
		{
			"https://images-ext-2.discordapp.net/external/wjfypiPJyBZX9iRXg0CJnePlTJzz61zEJZOvMGAS6jc/%3Fcid%3D73b8f7b16ae7523f0c81a978553172da2e47c9e336aea690%26rid%3Dgiphy.mp4%26ct%3Dg/https/media4.giphy.com/media/XozypzpGakVuX2ciZJ/giphy.mp4",
			"https://giphy.com/gifs/middle-finger-mister-rogers-fred-44Eq3Ab5LPYn6",
			"https://giphy.com/gifs/gq-kim-kardashian-fuck-you-middle-finger-xT0GqgBS0IdI3rFXHy",
			"https://tenor.com/view/april-fool-april-fools-spongebob-spongebob-april-fools-april-fools-spongebob-gif-13844032",
			"https://tenor.com/view/touching-grass-touch-gif-21219969",
			"https://tenor.com/view/jerk-off-the-hangover-ken-jeong-jerk-off-motion-screw-you-gif-5953847",
			"https://tenor.com/view/you-freaking-suck-kevin-hart-cold-as-balls-youre-no-good-youre-trash-gif-18321224",
			"https://tenor.com/view/cedric-xmen-gif-21396093",
			"https://tenor.com/view/homer-simpson-middle-finger-fuck-peace-out-gif-12375457",
			"https://tenor.com/view/mickey-mouse-middle-finger-dirty-finger-fuck-you-flip-off-gif-15416854",
			"https://tenor.com/view/middle-finger-umbrella-to-my-haters-gif-5679654",
			"https://tenor.com/view/middle-finger-dirty-finger-fuck-you-flip-off-gif-4904551",
			"https://tenor.com/view/middle-finger-veronica-mars-kristen-bell-fuck-you-fuck-off-gif-4576746",
			"https://tenor.com/view/middle-finger-fuck-jack-nicholson-the-finger-fuck-off-gif-15549395",
			"https://tenor.com/view/funny-kids-middle-finger-finger-gun-gif-14266608",
			"https://tenor.com/view/kid-middle-finger-fu-flip-off-flipping-off-gif-16219575",
			"https://tenor.com/view/middle-finger-ryan-stiles-pocket-dirty-finger-fuck-you-gif-4672090",
			"https://tenor.com/view/middle-finger-fuck-off-fuck-you-flip-off-screw-you-gif-4275997",
			"https://tenor.com/view/spongebob-fuck-you-rainbow-gif-10696976",
			"https://tenor.com/view/cut-beans-punch-in-the-throat-mixing-stirring-gif-16715704",
			"https://tenor.com/view/no-fuck-you-mad-mean-angry-gif-5685671",
			"https://tenor.com/view/hangover-ken-jeong-mr-chow-hahaha-fuck-you-gif-4887628",
			"https://tenor.com/view/flipping-off-flip-off-middle-finger-smile-happy-gif-4746862",
			"https://tenor.com/view/baby-girl-middle-finger-mood-screw-you-leave-me-alone-gif-10174031",
			"https://giphy.com/gifs/workaholics-comedy-central-season-4-3ofT5VKbcCMGMoHULm",
			"https://giphy.com/gifs/3oEjI2JdQPkmLxMcrm",
			"https://giphy.com/gifs/XHr6LfW6SmFa0",
			"https://giphy.com/gifs/tpain-3o7btYc0vx0tTPYVLa",
			"https://tenor.com/view/obama-fuckit-fuck-this-im-out-fuck-it-im-out-im-done-gif-13701118",
			"https://tenor.com/view/you-wanna-play-games-try-me-come-on-im-ready-samuel-l-jackson-gif-16248260",
			"https://tenor.com/view/nope-danny-de-vito-no-gif-8123780",
			"https://tenor.com/view/i-give-a-fuck-none-of-that-shit-idgaf-run-the-jewels-run-the-jewels-gifs-gif-11848065",
			"https://tenor.com/view/chris-tucker-hell-no-friday-no-fuck-that-gif-10096547",
			"https://tenor.com/view/chris-rock-huh-what-confused-wtf-gif-18025317",
			"https://tenor.com/view/joe-biden-gif-18249938",
			"https://tenor.com/view/fuck-that-nah-ice-cube-gif-9513452",
			"https://tenor.com/view/that70s-show-kitty-drinks-drinking-day-chill-gif-16669162",
			"https://tenor.com/view/naw-mike-epps-nah-hell-hell-naw-gif-14557582",
			"https://tenor.com/view/no-absolutely-not-gif-13012986",
			"https://tenor.com/view/pete-davidson-advice-fuck-that-shit-homie-fuck-that-gif-14270327",
			"https://tenor.com/view/ceelo-green-shit-forget-you-fuck-you-gif-4276316",
			"https://tenor.com/view/screw-you-nikki-minaj-snl-snl-gifs-saturday-night-live-gif-11870311",
			"https://tenor.com/view/talking-boy-mic-what-the-fuck-are-you-doing-gif-16697464",
			"https://tenor.com/view/yeah-yeah-go-away-bernie-sanders-saturday-night-live-go-away-stay-away-gif-17199593",
			"https://tenor.com/view/flipping-off-flip-off-teich-middle-finger-fuck-off-fuck-you-gif-15587868",
			"https://tenor.com/view/snoop-dogg-rapper-raper-angry-anger-gif-17002797",
			"https://tenor.com/view/buh-bye-shoo-go-away-gif-15313860",
			"https://tenor.com/view/benedict-cumberbatch-go-away-bye-leave-gif-5875554",
			"https://tenor.com/view/mad-angry-angry-girl-angry-little-girl-get-out-gif-14317590",
			"https://tenor.com/view/bender-futurama-kill-all-humans-robot-gif-17343915",
			"https://tenor.com/view/cute-black-boy-fuck-you-cute-black-boy-fuck-you-loswr-loser-gif-24719293",
		};

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
		public static List<IEmote> AprilFoolsMalarkeyLevels = new()
		{
			new Emoji("🕧"),
			new Emoji("🕦"),
			new Emoji("🕔"),
			new Emoji("🕤"),
			new Emoji("🕥"),
			new Emoji("🕢"),
			new Emoji("🕣"),
			new Emoji("🕠"),
			new Emoji("🕡"),
			new Emoji("🕞"),
			new Emoji("🕟"),
			new Emoji("🕜"),
			new Emoji("🕝"),
			new Emoji("🕕"),
			new Emoji("🕓"),
			new Emoji("🕚"),
			new Emoji("🕘"),
			new Emoji("🕙"),
			new Emoji("🕖"),
			new Emoji("🕗"),
			new Emoji("🕛"),
			new Emoji("🕒"),
			new Emoji("🕑"),
			new Emoji("🕐")
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
