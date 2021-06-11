using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Constants
{
	public static class PortalConstants
	{
		public static List<string> PortalSummoningText = new List<string>()
		{
			"Ph'nglui mglw'nafh Cthulhu R'lyeh wgah'nagl fhtagn",
			"Hastur Hastur Hastur",
			$"Conjuro potentiae Lucifer{Environment.NewLine}Patefacio a porta ad inferos{Environment.NewLine}Et tua deamones transire{Environment.NewLine}Sic urantar mundi",
			"Yu Mo Gui Gwai Fai Di Zao Yu Mo Gui Gwai Fai Di Zao Yu Mo Gui Gwai Fai Di Zao Yu Mo Gui Gwai Fai Di Zao Yu Mo Gui Gwai Fai Di Zao",
			"100110110100001011001001010010100110110100001000100101010000101000011011010000101011111",
			"Portal Is Loading...",
			"You Didn't See Anything Penguin Dot JPEG",
			"Opening a Portal...",
			"404 Message Not Found",
			"How Embarassing... Just a moment.",
			"Under Construction"
		};

		public static string PortalIamge = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcREcWHgH3VH_18aPmUyMaKX0leVnwscivYRXg&usqp=CAU";
		public static string PortalTitle(string portalUser, string channelName) => $"{portalUser} Opened A Portal To {channelName}";
		public static string PortalFieldTitle = "You feel the portal pulling you in...";
		public static string PortalFieldContent = "Perhaps it would be best to continue your conversation on the other side.";
	}
}
