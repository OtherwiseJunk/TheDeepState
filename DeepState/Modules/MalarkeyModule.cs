using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeepState.Constants;
using TraceLd.MineStatSharp;
using DartsDiscordBots.Permissions;
using DeepState.Utilities;
using DartsDiscordBots.Utilities;

namespace DeepState.Modules
{
	public class MalarkeyModule : ModuleBase
	{
		[Command("jackbox")]
		[Summary("Makes a jackbox poll, and will announce a winner after 5 mintues. User must provide a comma separated list of the jack.")]
		public async Task Jackbox([Summary("A comma seperated list of the versions of jackbox to make the list for")] string versions)
		{
			List<string> versionList = versions.Split(',').ToList();
			List<string> pollGameList = new List<string>();

			for (int i = 1; i < 8; i++)
			{
				if (versionList.Contains(i.ToString()))
				{
					Dictionary<int, List<string>> jackbox = JackboxConstants.JackboxGameListByNumber;
					var gameList = jackbox[i];
					pollGameList.AddRange(gameList);
				}
			}

			await Context.Channel.SendMessageAsync(string.Join(Environment.NewLine, pollGameList));
		}

		[Command("mstatus"), Alias("minecraft", "minecraftstatus"), RequireGuild(new ulong[] { 698639095940907048, 95887290571685888 })]
		[Summary("Returns a message with a status of Sporf's Minecraft server")]
		public async Task MinecraftStatus()
		{
			MineStat ms = new MineStat(SporfbaseConstants.ServerAddress, SporfbaseConstants.ServerPort);

			if (ms.ServerUp)
			{
				EmbedBuilder eb = new EmbedBuilder();

				eb.WithTitle($"{SporfbaseConstants.ServerAddress} Status");
				eb.AddField("Player Count:", $"{ms.CurrentPlayers}/{ms.MaximumPlayers}");
				eb.AddField("MotD:", $"{ms.Motd}");				
				await Context.Channel.SendMessageAsync("", false, eb.Build());
			}
			else
			{
				await Context.Channel.SendMessageAsync("Server is offline!");
			}
		}

		[Command("weekend")]
		[Summary("It's The Weekend, Ladies and Gentleman!")]
		public async Task TheWeekend()
		{
			await Context.Channel.SendMessageAsync("https://cdn.discordapp.com/attachments/745024703365644320/840383340790939658/theweekend.mp4");
		}

		[Command("walkingdad"), RequireGuild(new ulong[] { 698639095940907048, 95887290571685888 })]
		[Summary("Check in on the server's favorite dad!")]
		public async Task WalkingDad()
		{
			IGuildUser theDad = Context.Guild.GetUserAsync(SharedConstants.TheDad).Result;
			string name = theDad.Nickname ?? theDad.Username;
			EmbedBuilder embed = new EmbedBuilder();
			embed.WithTitle($"Time to check in on {name}!");
			embed.WithImageUrl(theDad.GetAvatarUrl());
			embed.AddField($"{name}", "Status: Dad");

			_ = Context.Channel.SendMessageAsync(embed: embed.Build());
		}

		[Command("portal")]
		[RequireOwner()]
		public async Task OpenAPortal(ITextChannel portalTargetChannel)
		{

			string username = (Context.User as IGuildUser).Nickname ?? Context.User.Username;
			IUserMessage targetChannelMessage = await portalTargetChannel.SendMessageAsync(PortalConstants.PortalSummoningText.GetRandom());
			IUserMessage sourceChannelMessage = await Context.Channel.SendMessageAsync(PortalConstants.PortalSummoningText.GetRandom());
			_ = targetChannelMessage.ModifyAsync(msg =>
			 {
				 msg.Content = "";
				 msg.Embed = PortalUtilities.BuildPortalEmbed(username, Context.Channel.Name, sourceChannelMessage.GetJumpUrl());
			 });

			_ = sourceChannelMessage.ModifyAsync(msg =>
			{
				msg.Content = "";
				msg.Embed = PortalUtilities.BuildPortalEmbed(username, portalTargetChannel.Name, targetChannelMessage.GetJumpUrl());
			});
		}
	}
}