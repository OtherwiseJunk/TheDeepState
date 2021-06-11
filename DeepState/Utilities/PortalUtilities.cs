using DeepState.Constants;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Utilities
{
	public static class PortalUtilities
	{
		public static Embed BuildPortalEmbed(string portalUser, string channelName, string messageLink)
		{
			EmbedBuilder builder = new EmbedBuilder();
			byte[] channelNameHash = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(channelName));
			builder.WithColor(new Color(channelNameHash[0], channelNameHash[1], channelNameHash[2]));
			builder.WithUrl(messageLink);
			builder.WithImageUrl(PortalConstants.PortalIamge);
			builder.WithTitle(PortalConstants.PortalTitle(portalUser, channelName));
			builder.AddField(PortalConstants.PortalFieldTitle, PortalConstants.PortalFieldContent);
			
			return builder.Build();
		}
	}
}
