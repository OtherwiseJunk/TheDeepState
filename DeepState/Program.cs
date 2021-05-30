using DartsDiscordBots.Modules.Help;
using DartsDiscordBots.Modules.Bot;
using DartsDiscordBots.Modules.Indecision;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using DeepState.Constants;
using DeepState.Modules;
using DartsDiscordBots.Modules.Help.Interfaces;
using DeepState.Models;
using DartsDiscordBots.Modules.Bot.Interfaces;
using System.Collections.Generic;
using DartsDiscordBots.Utilities;
using System.Linq;
using DartsDiscordBots.Modules.Chat;
using DartsDiscordBots.Services.Interfaces;
using DartsDiscordBots.Services;
using System.Text.RegularExpressions;
using DeepState.Data.Context;
using Microsoft.EntityFrameworkCore;
using DeepState.Service;
using System.Threading;
using DeepState.Data.Services;

namespace DeepState
{
	public class Program
	{
		private Dictionary<ulong, List<ulong>> LCCListOfActiveUsersByGuild = new Dictionary<ulong, List<ulong>>();
		private object DictionaryLock = new object();
		private readonly DiscordSocketClient _client;
		private readonly CommandService _commands;
		private readonly IServiceProvider _services;
		private readonly Random _rand;
		private readonly string _meRegex = @"(de*r?p)\s*(sta*te*)";

		private readonly List<string> RankNerdResponses = new List<string>

		{
			"https://m.media-amazon.com/images/I/91umiveo5mL._SS500_.jpg",
			"https://i.imgur.com/PvT6XMa.gif",
			"https://i.imgur.com/1Y8Czxu.gif",
			"https://i.gifer.com/9clm.gif",
			"https://i.imgur.com/Ek2X3Hw.gif"
		};

		private readonly List<string> ReactableEmotes = new List<string>
		{
			SharedConstants.BogId,
			SharedConstants.ConcernedFroggyId,
			SharedConstants.ThisTBHId,
			SharedConstants.ForeheadID,
			SharedConstants.BooHooCrackerID,
			SharedConstants.LaughingFaceID,
			SharedConstants.BonkID
		};

		private Program()
		{
			_client = new DiscordSocketClient();
			_services = ConfigureServices();
			_services.GetService<OOCDBContext>().Database.EnsureCreated();
			_services.GetService<GuildUserRecordContext>().Database.EnsureCreated();
			_commands = new CommandService();
			_client.Log += Log;
			_commands.Log += Log;
			_rand = new Random(DateTime.Now.Millisecond);
			_client.ReactionAdded += OnReact;
		}

		public async Task KlaxonCheck(IEmote reactionEmote, ISocketMessageChannel channel, IMessage msg)
		{
			//If it's an Emote, extract the ID. Otherwise we will not need it.
			ulong? emoteId = (reactionEmote as Emote) != null ? (ulong?)(reactionEmote as Emote).Id : null;
			if (SharedConstants.EmoteNameandId(reactionEmote.Name, emoteId) == SharedConstants.QIID && msg.Reactions[Emote.Parse(SharedConstants.QIID)].ReactionCount == 1)
			{
				await channel.SendMessageAsync(SharedConstants.KlaxonResponse, messageReference: new MessageReference(msg.Id));
			}
		}

		public async Task SelfReactCheck(SocketReaction reaction, ISocketMessageChannel channel, IMessage msg)
		{
			if (msg.Reactions.Count == 1)
			{
				if (msg.Reactions.First().Value.ReactionCount == 1 && reaction.UserId == msg.Author.Id && channel.Id != SharedConstants.SelfCareChannelId)
				{
					await msg.AddReactionAsync(Emote.Parse(SharedConstants.YouAreWhiteID));
					await channel.SendMessageAsync($"{msg.Author.Mention} {SharedConstants.SelfReactResponses.GetRandom()}", messageReference: new MessageReference(msg.Id), allowedMentions: AllowedMentions.All);
					if (msg.Author.Id == SharedConstants.TheCheatingUser)
					{
						await channel.SendMessageAsync($"WE GOT HIM! {channel.GetUserAsync(SharedConstants.ThePoliceUser).Result.Mention}");
					}
				}
			}
		}

		private async Task ClearDeepStateReactionCheck(IEmote reactionEmote, ISocketMessageChannel channel, IMessage msg)
		{
			//Check for the clearing reaction emote, and do no clear if the author is Deep State.
			if (SharedConstants.ClearingEmotes.Contains(reactionEmote.Name) && msg.Author != _client.CurrentUser)
			{
				foreach (var reaction in msg.Reactions)
				{
					if (reaction.Value.IsMe)
					{
						await msg.RemoveReactionAsync(reaction.Key, _client.CurrentUser);
					}
				}
			}
		}

		private async Task OnReact(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction)
		{
			IEmote reactionEmote;
			IMessage msg = channel.GetMessageAsync(message.Id).Result;
			if ((reaction.Emote as Emote) != null)
			{
				reactionEmote = (Emote)reaction.Emote;
			}
			else
			{
				reactionEmote = (Emoji)reaction.Emote;
			}

			//One of the voting reactions, :x:, can also be used to clear DeepState reacts, so we run this regardless.
			new Thread(async () => { _ = ClearDeepStateReactionCheck(reactionEmote, channel, msg); }).Start();

			if (SharedConstants.VotingEmotes.Contains(reaction.Emote.Name) || msg.Author.IsBot)
			{
				return;
			}

			new Thread(async () => { _ = KlaxonCheck(reactionEmote, channel, msg); }).Start();
			new Thread(async () => { _ = SelfReactCheck(reaction, channel, msg); }).Start();

		}

		public static void Main(string[] args)
			=> new Program().MainAsync().GetAwaiter().GetResult();

		public async Task MainAsync()
		{
			Console.WriteLine($"{BotProperties.InternalName} has been INITIALIZED");

			await InstallCommandsAsync();


			string token = Environment.GetEnvironmentVariable("DEEPSTATE");
			Console.WriteLine("Attempting to retrieve bot token from Environment...");

			if (string.IsNullOrEmpty(token))
			{
				Console.WriteLine("Please provide the bot token:");
				token = Console.ReadLine();
			}
			else
			{
				Console.WriteLine("Success!");
			}

			await _client.LoginAsync(TokenType.Bot, token);
			await _client.StartAsync();

			LibcraftCoinCheck();
			// Block this task until the program is closed.
			await Task.Delay(-1);
		}
		private static IServiceProvider ConfigureServices()
		{
			//We don't have any services currently for DI
			//but once we do this is where we would add them.
			var map = new ServiceCollection()
				.AddSingleton<IHelpConfig, HelpConfig>()
				.AddSingleton<IBotInformation, BotInformation>()
				.AddSingleton<IMessageReliabilityService, MessageReliabilityService>()
				.AddSingleton<ImagingService>()
				.AddSingleton<OutOfContextService>()
				.AddSingleton<UserRecordsService>()
				.AddDbContext<OOCDBContext>()
				.AddDbContext<GuildUserRecordContext>()
				.AddDbContextFactory<OOCDBContext>()
				.AddDbContextFactory<GuildUserRecordContext>();

			return map.BuildServiceProvider();
		}
		private Task Log(LogMessage msg)
		{
			Console.WriteLine($"[{DateTime.Now.ToString("g")}] ({msg.Source}): {msg.Message}");
			return Task.CompletedTask;
		}
		public async Task InstallCommandsAsync()
		{
			await _commands.AddModuleAsync<MalarkeyModule>(_services);
			await _commands.AddModuleAsync<HelpModule>(_services);
			await _commands.AddModuleAsync<IndecisionModule>(_services);
			await _commands.AddModuleAsync<BotModule>(_services);
			await _commands.AddModuleAsync<ChatModule>(_services);
			await _commands.AddModuleAsync<OutOfContextModule>(_services);
			await _commands.AddModuleAsync<UserRecordsModule>(_services);


			_client.MessageReceived += HandleCommandAsync;
		}

		public async Task EgoCheck(SocketMessage msg)
		{
			if (IsMentioningMe(msg))
			{
				_ = msg.AddReactionAsync(Emote.Parse(SharedConstants.RomneyRightEyeID));
				_ = msg.AddReactionAsync(Emote.Parse(SharedConstants.RomneyLeftEyeID));
			}
		}

		public async Task RandomReactCheck(SocketMessage msg)
		{
			if (!SharedConstants.NoAutoReactsChannel.Contains(msg.Channel.Id))
			{
				if (msg.Content.ToLower() == "!rank") Console.WriteLine("Rolling for rank...");
				if (msg.Content.ToLower() == "!rank" && PercentileCheck(1))
				{
					await msg.Channel.SendMessageAsync(RankNerdResponses.GetRandom());
					return;
				}
				if (PercentileCheck(1) & PercentileCheck(40))
				{
					if (PercentileCheck(1))
					{
						await msg.AddReactionAsync(Emote.Parse(SharedConstants.GwalmsID));
					}
					else
					{
						await msg.AddReactionAsync(Emote.Parse(ReactableEmotes.GetRandom()));
					}
				}
			}
		}

		public async Task LibcraftCoinCheck()
		{
			Random rand = new Random(DateTime.Now.Millisecond * DateTime.Now.Second);
			int nextDuration = 10000;//rand.Next(60000, 240000);
			UserRecordsService service = _services.GetRequiredService<UserRecordsService>();
			lock (DictionaryLock)
			{
				foreach (ulong guildId in LCCListOfActiveUsersByGuild.Keys)
				{
					foreach (ulong userId in LCCListOfActiveUsersByGuild[guildId])
					{
						service.IssuePayout(userId, guildId);
					}
				}
				LCCListOfActiveUsersByGuild = new Dictionary<ulong, List<ulong>>();
			}

			Thread.Sleep(nextDuration);
			_ = LibcraftCoinCheck();
		}
		public async Task LibcraftCoinMessageHandler(SocketMessage msg)
		{
			ulong messageUserId = msg.Author.Id;
			ulong messageGuildId = ((IGuildChannel)msg.Channel).GuildId;
			lock (DictionaryLock)
			{
				if (LCCListOfActiveUsersByGuild.Keys.Contains(messageGuildId))
				{
					if (!LCCListOfActiveUsersByGuild[messageGuildId].Contains(messageUserId))
					{
						LCCListOfActiveUsersByGuild[messageGuildId].Add(messageUserId);
					}
				}
				else
				{

					LCCListOfActiveUsersByGuild[messageGuildId] = new List<ulong> { messageUserId };
				}
			}
		}

		private async Task HandleCommandAsync(SocketMessage messageParam)
		{



			//Don't process the command if it was a system message
			var message = messageParam as SocketUserMessage;
			if (message == null) return;

			if (message.Author.IsBot)
			{
				//We don't want to process messages from bots. Screw bots, all my homies hate bots.
				return;
			}

			new Thread(async () => { await EgoCheck(messageParam); }).Start();
			new Thread(async () => { await RandomReactCheck(messageParam); }).Start();
			new Thread(async () => { await LibcraftCoinMessageHandler(messageParam); }).Start();

			int argPos = 0;

			// Determine if the message is a command based on the prefix
			if (!(message.HasCharPrefix(BotProperties.CommandPrefix, ref argPos) ||
					message.HasMentionPrefix(_client.CurrentUser, ref argPos)))
				return;

			var context = new SocketCommandContext(_client, message);

			var isSuccess = await _commands.ExecuteAsync(
				context: context,
				argPos: argPos,
				services: _services);

		}

		public bool PercentileCheck(int successCheck)
		{
			return _rand.Next(1, 100) <= successCheck;
		}

		public bool IsMentioningMe(SocketMessage discordMessage)
		{
			IMessage replyingToMessage = discordMessage.Reference != null ? discordMessage.Channel.GetMessageAsync(discordMessage.Reference.MessageId.Value).Result : null;

			if (discordMessage.MentionedUsers.Contains(_client.CurrentUser))
			{
				return true;
			}
			if (replyingToMessage != null && replyingToMessage.Author.Id == _client.CurrentUser.Id)
			{
				return true;
			}
			if (Regex.IsMatch(discordMessage.Content, _meRegex, RegexOptions.IgnoreCase))
			{
				return true;
			}
			return false;
		}
	}
}
