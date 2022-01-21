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
using DartsDiscordBots.Services.Interfaces;
using DeepState.Service;
using System.IO;
using SkiaSharp;
using System.Threading;

namespace DeepState.Modules
{
	public class MalarkeyModule : ModuleBase
	{
		ImagingService _imaging { get; set; }
		public MalarkeyModule(ImagingService imaging)
		{
			_imaging = imaging;
		}
		[Command("mstatus"), Alias("minecraft", "minecraftstatus", "mcstatus"), RequireGuild(new ulong[] { SharedConstants.LibcraftGuildId, 95887290571685888 })]
		[Summary("Returns a message with a status of Sporf's Minecraft server")]
		public async Task MinecraftStatus(string serverAddress = SporfbaseConstants.ServerAddress, ushort serverPort = SporfbaseConstants.ServerPort)
		{
			MineStat ms = new MineStat(serverAddress, serverPort);

			if (ms.ServerUp)
			{
				EmbedBuilder eb = new EmbedBuilder();

				eb.WithTitle($"{serverAddress} Status");
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

		[Command("walkingdad"), RequireGuild(new ulong[] { SharedConstants.LibcraftGuildId, 95887290571685888 })]
		[Summary("Check in on the server's favorite dad!")]
		public async Task WalkingDad()
		{
			IGuildUser theDad = Context.Guild.GetUserAsync(SharedConstants.TheDad, CacheMode.AllowDownload).Result;
			string name = theDad.Nickname ?? theDad.Username;
			EmbedBuilder embed = new EmbedBuilder();
			embed.WithTitle($"Time to check in on {name}!");
			embed.WithImageUrl(theDad.GetAvatarUrl());
			embed.AddField($"{name}", "Status: Dad");

			_ = Context.Channel.SendMessageAsync(embed: embed.Build());
		}

		[Command("imgonnacome")]
		public async Task ImGonnaCome()
		{
			_ = Context.Channel.SendMessageAsync("https://youtu.be/NRCf3KUEVyw");
		}

		[Command("donotcome")]
		public async Task DoNotCome()
		{
			_ = Context.Channel.SendMessageAsync("https://media.tenor.com/images/a7b5e8c66c4214d3f04f3726a5475a65/tenor.gif");
		}

		[Command("portal")]
		[Summary("Opens a portal to another channel. Generally used for off-topic discussion in a channel.")]
		public async Task OpenAPortal([Summary("A # link to the channel to open the portal to.")] ITextChannel portalTargetChannel)
		{

			string username = (Context.User as IGuildUser).Nickname ?? Context.User.Username;
			IUserMessage targetChannelMessage = await portalTargetChannel.SendMessageAsync(PortalConstants.PortalSummoningText.GetRandom());
			IUserMessage sourceChannelMessage = await Context.Channel.SendMessageAsync(PortalConstants.PortalSummoningText.GetRandom());
			_ = targetChannelMessage.ModifyAsync(msg =>
			 {
				 msg.Content = "";
				 msg.Embed = PortalUtilities.BuildPortalEmbed(username, Context.Channel.Name, sourceChannelMessage.GetJumpUrl(), true);
			 });

			_ = sourceChannelMessage.ModifyAsync(msg =>
			{
				msg.Content = "";
				msg.Embed = PortalUtilities.BuildPortalEmbed(username, portalTargetChannel.Name, targetChannelMessage.GetJumpUrl(), false);
			});
		}

		[Command("nft")]
		[Summary("Generates an NFT for the user."), RequireChannel(new ulong[] { SharedConstants.LCBotCommandsChannel, SharedConstants.TestChannel })]
		public async Task MakeNFT()
		{
			new Thread(async () =>
			{
				string guid = Guid.NewGuid().ToString();

				SKBitmap bmp = _imaging.GenerateJuliaSetImage(1028, 720).Result;

				Stream stream = bmp.Encode(SKEncodedImageFormat.Png, 100).AsStream();
				await Context.Channel.SendFileAsync(stream, $"guid.png", text: $"Here is your newly minted NFT, ID {Guid.NewGuid()}. Write it down or something, I'm not gonna track it.");
			}).Start();

		}
	}
}
