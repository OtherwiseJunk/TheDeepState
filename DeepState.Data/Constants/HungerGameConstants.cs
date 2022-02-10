
using DartsDiscordBots.Utilities;
using System.Collections.Generic;

namespace DeepState.Data.Constants
{
	public static class HungerGameConstants
	{
		public const ulong TheRepublican = 735391839280169040;
		public const double CostOfAdmission = 1;
		public static string HungerGameTributesEmbedTitle { get; set; } = "⛈️ **T H U N D E R D O M E TRIBUTES** ⛈️";
		public static string RandomTributeWeaponKill { get; set; } = "TributeWeaponKill";
		public static string RandomTributeWeaponKillFormat { get; set; } = "{0} killed {1} with {2}";

		public const string TributeRoleName = "Tribute";
		public const string ChampionRoleName = "⛈️CHAMPION⛈️";
		public const string CorpseRoleName = "Corpse";

		public static List<string> Weapons = new List<string>
		{
			"a Halibut.",
			"an AK-47.",
			"a Deagle.",
			"Gwalms' Body?!",
			"a Katana.",
			"a Sword (Great).",
			"a Sword (Short).",
			"a Sword (Bastard).",
			"a Sword (Fish).",
			"a Great Axe.",
			"a Hand Axe.",
			"a Bong.",
			"an Icepick.",
			"a Type 100 SMG.",
			"a Skorpion (the gun).",
			"a Scorpion (the arachnid).",
			"a StG-44.",
			"an XM8.",
			"a FG-42.",
			"an H&K G11.",
			"a Japanese Type 5.",
			"a PTRS-41.",
			"a PP-2000.",
			"a G-42.",
			"a Golden Gun.",
			"a Spartan Lazer.",
			"an Empty Bottle of Everclear.",
			"a G-43.",
			"a WA-2000.",
			"a Super Cool!!!! Mech.",
			"a Covenant Energy Sword.",
			"a Hidden Blade.",
			"a Gravity Gun.",
			"the Master Sword.",
			"a Keyblade.",
			"a BFG-9001.",
			"the Mega-Buster.",
			"the Blades of Chaos.",
			"a Frying Pan.",
			"a Orbital Bombardment.",
			"a Rod from God missile.",
			"the Moon.",
			"Hydroxychloriquine.",
			"a Syringe filled with Bleach labeled 'Virus-Killing Science Juice'.",
			"Jeb's `PLEASE CLAP` sign.",
			"a (sabotauged) Faulty O-Ring.",
			"a Tesla, in autopilot mode.",
			"an ICMB.",
			"a battleaxe.",
			"a dagger.",
			"a sickle.",
			"a rapier.",
			"a rake.",
			"a kitchen knife.",
			"a scimitar.",
			"a swiss army knife.",
			"a chainsaw.",
			"a cleaver.",
			"a MOTHERFUCKING LIGHTNING BOLT.",
			"a Time Machine and a very small pillow.",
			//$"a cashed-in favor from {OldGods.GetRandom()}.",
			"a cashed-in favor from D9.",
			"a cashed-in favor from 🇺🇸President Joe Biden🇺🇸.",
			"a Million God-Damn Mosquitos.",
			"their Bare Hands.",
			"their Bear Hands.",
			"a spoon.",
			"both barrels of a Sawed-Off Shotgun.",
			"a Mateba Unica 6.",
			"a Hamster. Do Not Ask.",
			"the Sword that Gwindor gave to Túrin, who knew that it was heavy and strong and had great power; but its blade was black and dull and its edges blunt. Then Gwindor said: 'This is a strange blade, and unlike any that I have seen in Middle-earth.'",
			"a Big-Iron (worn on their hips).",
			"a Wooden Steak.",
			"a Wooden Stake.",
			"a Meaty Steak.",
			"their MIND !!!!!",
			"the Force.",
			"an Easily Exploitable Bug. Fucking Hackers.",
			"a Fat-Man portable nuke launcher.",
			"a Typewriter (Bludgeoning)",
			"a Typewriter (Character Assassination. Then Bludgeoning.)",
			"the Thu'um. FUS RO DEAD!",
			"a Death Note. Cause of Death? Shitting out their organs.",
			"a Death Note. Cause of Death? Spontaneous Combustion.",
			"a Death Note. Cause of Death? Spontaneous DECAPATION!!!!",
			"a Death Note. Cause of Death? Choking on their own tongue.",
			"ONE PUNCH.",
			"a MIGHTY Kamehameha.",
			"a Clank of Tanks.",
			"a Ukelele.",
			"the sickest dance moves you've ever seen!",
		};

		public static List<string> ThingsToExplain = new List<string> {
			"The QAnon Cinematic Universe.",
			"One Piece.",
			"THe Marvel Cinematic Universe.",
			"The Apocrypha.",
			"the full and glorious history of these very games.",
			"the James Bond Cinematic Universe.",
			"the original Libcraft Minecraft Server lore.",
			"the Dark Universe Cinematic Universe.",
			"the Halo Literary Universe.",
			"the Lovecraft Literary Universe.",
			"why Libcoin is a good investment.",
			"all 250 pages of the Magic the Gathering official rules.",
			"how to properly simp for TikTok thots.",
			"how Bernie can still win the 2020 elections.",
			"how simping is actually vital infrastructure",
			"how Trump is still running everything from behind the scenes, and will be re-instated any day now...",
			"what Neoliberalism _ACTUALLY_ means."
		};
		public static List<string> OldGods = new List<string> {
			"Dagon, the Freaky Fish Guy",
			"Abholos, the Devourer in the Mist",
			"Alala, Hearld of S'glhuo",
			"Ammutseba, the Devourer of Stars",
			"Amon-Goroloth, Creator of the Nile and Universe's Equilibrium",
			"Aphoom-Zhah, the Cold Flame",
			"Cthulhu, the Master of R'lyeh",
			"Arwassa, the Silent Shouter on the Hill",
			"Atlach-Nacha, the Spinner in Darkness",
			"Basatan, the Master of Crabs",
			"B'gnu-Thun, the Soul-Chilling Ice-God",
			"Byatis, the Berkeley Toad,",
			"Byagoona, the Faceless Ones",
			"The Color Out of Space",
			"Coatlicue, the Serpant Skirted One",
			"Dhumin, the Burrower from the Bluff",
			"Dythalla, the Lord of Lizards",
			"Gleeth, the Blind God of the Moon",
			"Hastur Hastur Hastur, The King in Yellow, The Peacock King, He Who is Not to be Named, Carcosa is lovely this time of year won't you visit? I cannot forget Carcosa where black stars hang in the heavens; where the shadows of men's thoughts lengthen in the afternoon, when the twin suns sink into the lake of Hali; and my mind will bear for ever the memory of the Pallid Mask. I pray God will curse the writer, as the writer has cursed the world with this beautiful, stupendous creation, terrible in its simplicity, irresistible in its truth—a world which now trembles before the King in Yellow - Sorry, where was I? right, ",
			"Yog-Sothoth, the All-in-One and One-in-All of limitless being and self",
			"Nyarlathotep, the Herald of Azathoth",
			"Azathoth, the Blind Idiot God"
		};
		//$"Fatally injured by {Pronoun} malfunctioning {Equipment.GetRandom()}",
		public static List<string> Equipment = new List<string> {
			"gun, which misfired"
		};

		//Words For Dead.
		public static List<string> HowDeadAreYou = new List<string>
		{
			"Dead",
			"Dead AF",
			"CANCELLED (permanently)",
			"Rotting",
			"Hasn't Moved In A While",
			"If it looks like a corpse, and smells like a corpse, it's probably a corpse.",
			"💀",
			"👻",
			"F's in the chat",
			"👆",
			"👇",
			"🧟",
			"🔩",
			"🪦",
			"⚰",
			"♻",
			"Cadaverific",
			"So Insanely Dead",
			"In a better place",
			"In a worse palce",
			"in an ok place, but still super dead.",
			"With their ancestors",
		};

		public static List<string> AvatarURLs = new List<string>
		{
			"https://play-lh.googleusercontent.com/VHB9bVB8cTcnqwnu0nJqKYbiutRclnbGxTpwnayKB4vMxZj8pk1220Rg-6oQ68DwAkqO=s128",
			"https://lh3.googleusercontent.com/8yk3gmXNmA98yDgnpGNHgaC1zzMj6w2wRRxomVCPI7fRFDrqMxkvqm1VTEag__Ec3f6OvkUoA4CM2cI3asVKpS3kdTI=w128-h128-e365-rj-sc0x00ffffff",
			"https://cdn.icon-icons.com/icons2/2620/PNG/128/among_us_player_light_blue_icon_156934.png",
			"https://static.wikia.nocookie.net/among-us-wiki/images/2/2c/Tan_emoji.png/revision/latest/scale-to-width-down/250?cb=20201123201156",
			"https://disforge.com/assets/icons/759421199951462420.png?t=95995a601a88945edecaa77bad465952",
			"https://lh3.googleusercontent.com/Os_iXh8eYDBbyc8sK9QzwP877Pk-KJRYTbibO3O9fkvGJKen6W5eptgv_t9txsAuaX3Fk2FTBZ7Kj9qFd4CPgi8k9g=w128-h128-e365-rj-sc0x00ffffff",
			"https://img.wattpad.com/useravatar/amonguslogic-life.128.36875.jpg",
			"https://user-content.flat-cdn.com/5fb2a5d8aee2625783a3aded/847fccbc-a7e1-463b-a47b-94ec395222ea.jpeg",
			"https://64.media.tumblr.com/5046d5574ea098b9e0708a33f9fe5781/3bb176e3c00d22ff-54/s128x128u_c1/c5f07490620fc89e913f1a930ad1c474eba8328b.jpg",
			"https://emojis.slackmojis.com/emojis/images/1570729670/6654/cthulhu.jpg?1570729670",
			"https://cdn.kqed.org/wp-content/uploads/sites/39/2013/04/Obama-128x128.jpg",
			"https://media.newyorker.com/photos/5c3e10b2f92c694343cc71b9/2:2/w_128,h_128,c_limit/Rosner-Fast-Food-Trump.jpg",
			"https://64.media.tumblr.com/avatar_4a3efe2f2d5b_128.pnj",
			"https://cdn.discordapp.com/attachments/850406580037222420/933510154291396649/image0.jpg",

		};
	}
}
