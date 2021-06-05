using DeepState.Data.Constants;
using DeepState.Data.Models;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Utilities
{
	public class HungerGameUtilities
	{
		public static Embed BuildTributeEmbed(List<HungerGamesTributes> tributes, int currentPage, IGuild guild)
		{
			EmbedBuilder embed = new EmbedBuilder();
			embed.Title = HungerGameConstants.HungerGameTributesEmbedTitle;
			foreach (HungerGamesTributes tribute in tributes)
			{
				IGuildUser user = guild.GetUserAsync(tribute.DiscordUserId).Result;
				embed.AddField(user.Nickname ?? user.Username, "Status: Alive");
				embed.WithFooter($"{currentPage}");
			}

			return embed.Build();
		}
	}
}
