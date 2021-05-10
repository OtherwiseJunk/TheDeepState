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

		[Command("mstatus"), Alias("minecraft", "minecraftstatus")]
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
	}
}