using System;
using System.Collections.Generic;

using Discord.Commands;
using Discord;

namespace DeepState.Constants
{
	public static class EmojiMonetizationConstants
	{
		public static List<string> ThankYouMessages = new List<string>
		{
			"Thank you for supporting the LibCraft:registered: Admin Team!",
			"It's support like yours that makes servers like ours possible.",
			"Thank you, ~~loyal customer~~ beloved friend!",
			"Thank you for your support! Please consider donating *real* money to our gracious admins as well.",
			"The LibCraft:registered: Admin Team thanks you for ~~lining their pockets~~ supporting our community.",
			"Free emojis were a ZIRP. Thank you for your understanding.",
			"Thank you for your support! Any complaints are to be addressed to our parent company, NotAShell Holdings, 1209 Orange Street, Wilmington, DE 19801."
		};

		// TODO: Finish populating this

		/*public static List<Emote> BasicPack = new List<Emote>
		{
			Emote.Parse("<:emoji:707407221792702525>"),
			Emote.Parse("<:this:707412329297346622>"),
			Emote.Parse("<:dAmn:815123565485228082>"),
			Emote.Parse("<:bog:763615501992591361>"),
			Emote.Parse("<:gore:854587419391164457>"),
			Emote.Parse("<:concernfroggy:737512429310836806>"),
			Emote.Parse("<:gorepat:858524272258973696>"),
			Emote.Parse("<:pensive_hug:831240330144776262>"),
			Emote.Parse("<:true:923420410350039060>")
		};

		public static List<Emote> ProPack = new List<Emote>
		{
			Emote.Parse("<:emoji:707407221792702525>"),
			Emote.Parse("<:this:707412329297346622>"),
			Emote.Parse("<:dAmn:815123565485228082>"),
			Emote.Parse("<:bog:763615501992591361>"),
			Emote.Parse("<:gore:854587419391164457>"),
			Emote.Parse("<:concernfroggy:737512429310836806>"),
			Emote.Parse("<:gorepat:858524272258973696>"),
			Emote.Parse("<:pensive_hug:831240330144776262>"),
			Emote.Parse("<:true:923420410350039060>"),
			Emote.Parse("<:woah:814576121541951510>"),
			Emote.Parse("<:concerneyes:879191937935106069>"),
			Emote.Parse("<a:gorepatanimated:906750561406292040>"),
			Emote.Parse("<:deepfriedemoji:961468023611457556>"),
			Emote.Parse("<:smugbrow:1013536644621664268>"),
			Emote.Parse("<:concernfromney:872173122533621790>"),
			Emote.Parse("<:tucker:826959853519896597>"),
			Emote.Parse("<:gutfeld:1141737776908357763>"),
			Emote.Parse("<:thonk:728045761010466847>"),
			Emote.Parse("<:boohoo_cracker:783170303559729162>")
		};

		public static List<Emote> GorePack = new List<Emote>
		{
			Emote.Parse("<:gore:854587419391164457>"),
			Emote.Parse("<:gorepat:858524272258973696>"),
			Emote.Parse("<:GorePat2:905366033534156800>"),
			Emote.Parse("<:goretrue:1122399419703230515>"),
			Emote.Parse("<:GoreBat:905364714031308801>"),
			Emote.Parse("<:GoreClown:905646998676439111>"),
			Emote.Parse("<:GoreLove:905364357989421076>"),
			Emote.Parse("<:gorejus:1064079788454723614>"),
			Emote.Parse("<:goretice:1064079849574105088>"),
			Emote.Parse("<a:tbhspin:1125989543821180999>"),
			Emote.Parse("<:goreHug:1061554920580460595>"),
			Emote.Parse("<a:gorepatanimated:906750561406292040>"),
			Emote.Parse("<:GoreClown:905646998676439111>"),
			Emote.Parse("<a:Goretate:986395197283901471>"),
			Emote.Parse("<a:goremoment:948458034282246214>"),
			Emote.Parse("<a:gorespin:904260690246762516>"),
			Emote.Parse("<:Amogore:962916525889708082>")
		};


		public static List<Emote> SympathyPack = new List<Emote>
		{
			Emote.Parse("<:pensive_hug:831240330144776262>"),
			Emote.Parse("<:goreHug:1061554920580460595>"),
			Emote.Parse("<:dAmn:815123565485228082>"),
			Emote.Parse("<:dAmney:924900221962551317>"),
			Emote.Parse("<a:gorepatanimated:906750561406292040>"),
			Emote.Parse("<:gorepat:858524272258973696>"),
			Emote.Parse("<a:damnpatanimated:953488419894480926>"),
			Emote.Parse("<a:angrypat:974037397299925022>"),
			Emote.Parse("<a:brainpat:1060384985829359676>"),
			Emote.Parse("<:dAmnpat:953488587163328532>"),
			Emote.Parse("<:dAmnpat2:953489511466291230>"),
			Emote.Parse("<:headpat_hand:953484733952700446>")
		};

		public static List<Emote> GwalmsPack = new List<Emote>
		{
			Emote.Parse("<:gwog:904087583569633291>"),
			Emote.Parse("<:gwalmstrue:1155328726280704040>"),
			Emote.Parse("<a:gwalk:903869026453831710>"),
			Emote.Parse("<:gwalms:793892038134857728>"),
			Emote.Parse("<a:gwalmsapproachingyourlocation:1097041075060875346>"),
			Emote.Parse("<a:gwalmsarmy:903869384454447135>"),
			Emote.Parse("<a:gwalmslogson:878814593881874472>"),
			Emote.Parse("<a:gwalmspat:970112249907789877>"),
			Emote.Parse("<a:gwalmsposting:861739773675438080>"),
			Emote.Parse("<:ingwallah:902377740655792158>"),
			Emote.Parse("<:this:707412329297346622>"),
			Emote.Parse("<:youdidthis:914352033056821269>")
		};

		public static List<Emote> UserpatPack = new List<Emote>
		{
			Emote.Parse("<a:BDpat:1064616315958071447>"),
			Emote.Parse("<a:Deltapat:977441936715677836>"),
			Emote.Parse("<a:Ericpat:1006428772318322698>"),
			Emote.Parse("<a:PuppyPat:1100807005754634341>"),
			Emote.Parse("<a:Ryopat:1007159352941101056>"),
			Emote.Parse("<a:STICKYPAT:1189042055998087229>"),
			Emote.Parse("<a:StickyWOKEpat:1099922482552635555>"),
			Emote.Parse("<a:ampat:974029568560218112>"),
			Emote.Parse("<a:anstuffpat:1005591557929447454>"),
			Emote.Parse("<a:breadpat:1081838903813750834>"),
			Emote.Parse("<a:campat:1006075449329320006>"),
			Emote.Parse("<a:clarapat:974035732597461072>"),
			Emote.Parse("<a:d9pat:944698428829605948>"),
			Emote.Parse("<a:forerunnerpat:1023473456731394058>"),
			Emote.Parse("<a:genderpat:1005591982317510757>"),
			Emote.Parse("<a:ghostypat:1096612559999729755>"),
			Emote.Parse("<a:gorepatanimated:906750561406292040>"),
			Emote.Parse("<a:gwalmspat:970112249907789877>"),
			Emote.Parse("<a:sanitycutepat:906995438341742632>"),
			Emote.Parse("<a:sanitypat:1056434856793473024>"),
			Emote.Parse("<a:stickypat:944698360735096923>"),
			Emote.Parse("<a:weeslypat:1191483351408857098>")
		};

		public static List<Emote> FroggyPack = new List<Emote>
		{
			Emote.Parse("<:concernfroggy:737512429310836806>"),
			Emote.Parse("<a:concernfrogry:873430600667774976>"),
			Emote.Parse("<:concerned9froggy:888954311533535232>"),
			Emote.Parse("<:copcernfroggy:885013388755882025>"),
			Emote.Parse("<:crycernfroggy:873430575275470848>"),
			Emote.Parse("<:concerneyes:879191937935106069>"),
			Emote.Parse("<:concernemoji:901642928160657458>"),
			Emote.Parse("<:concernfromney:872173122533621790>"),
			Emote.Parse("<:concerndrigo:883215430666313798>"),
			Emote.Parse("<:concerneye:925275848376975370>")
		};

		public static List<Emote> BootlickerPack = new List<Emote>
		{
			Emote.Parse("<:stickywoah:940114633795186718>"),
			Emote.Parse("<a:stickypat:944698360735096923>"),
			Emote.Parse("<:StickyWoke:1053861440676704366>"),
			Emote.Parse("<a:StickyWOKEpat:1099922482552635555>"),
			Emote.Parse("<a:STICKYPAT:1189042055998087229>"),
			Emote.Parse("<a:d9pat:944698428829605948>"),
			Emote.Parse("<:d9erism:938633822893932545>"),
			Emote.Parse("<:badmin:892871108360040498>"),
			Emote.Parse("<:concerned9froggy:888954311533535232>"),
			Emote.Parse("<a:tfwurinfrontofd9intraffic:921648793437470740>"),
			Emote.Parse("<:libcraftadmins:938627454636032031>"),
			Emote.Parse("<:libcoin:905356863011426355>")
		};

		public static List<Emote> CrabPack = new List<Emote>
		{
			Emote.Parse("<a:crabrave:820772881410818108>"),
			Emote.Parse("<a:crabrave:820772908355420240>"),
			Emote.Parse("<a:crabrave:820772950147072011>"),
			Emote.Parse("<a:crabrave:820772965754863656>"),
			Emote.Parse("<a:crabrave:820772980140802089>"),
			Emote.Parse("<a:crabrave:820772993755381820>"),
			Emote.Parse("<a:crabrave:820773009299472384>")
		};

		public static List<Emote> AnguishPack = new List<Emote>
		{
			Emote.Parse("<:doom:822351184810737726>"),
			Emote.Parse("<:husk:941093178847666226>"),
			Emote.Parse("<:dAmn:815123565485228082>"),
			Emote.Parse("<:dAmney:924900221962551317>"),
			Emote.Parse("<:IHATE:903398862314676334>"),
			Emote.Parse("<a:aware:1065855248696082543>"),
			Emote.Parse("<:fearless:990357783553646672>"),
			Emote.Parse("<:clueless:973065463368122368>"),
			Emote.Parse("<:cluelessconfident:960020740814565396>"),
			Emote.Parse("<:pain:840604059903328257>"),
			Emote.Parse("<a:HAHAHAHAHAHAHAHAHAHAHAHAHAHAHA:905987165614407772>"),
			Emote.Parse("<a:killmepleasethepainisunbearable:995726834220138496>"),
			Emote.Parse("<:killme:801674108491792394>"),
			Emote.Parse("<:gutfeld:1141737776908357763>"),
			Emote.Parse("<:utterrage:820744882561613905>")
		};*/



		// For testing purposes, seldom-used emoji
		public static List<Emote> TestPack = new List<Emote>
		{
			Emote.Parse("<:magnathonk:928158272836472872>"),
			Emote.Parse("<:george:707391310830632990>"),
			Emote.Parse("<:horseshoe:747625023719604264>")
        };
	}
}