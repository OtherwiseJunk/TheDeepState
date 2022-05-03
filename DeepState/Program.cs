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
using DartsDiscordBots.Modules.Chat;
using DartsDiscordBots.Services.Interfaces;
using DartsDiscordBots.Services;
using DeepState.Data.Context;
using DeepState.Service;
using System.Threading;
using DeepState.Data.Services;
using DeepState.Handlers;
using Utils = DeepState.Utilities.Utilities;
using DeepState.Utilities;
using OMH = DartsDiscordBots.Handlers.OnMessageHandlers;
using ORH = DartsDiscordBots.Handlers.OnReactHandlers;
using FluentScheduler;
using HungerGameConstants = DeepState.Data.Constants.HungerGameConstants;
using Serilog;
using DartsDiscordBots.Modules.Jackbox.Interfaces;
using DartsDiscordBots.Modules.NFT;

namespace DeepState
{
	public class Program
	{
		private readonly DiscordSocketClient _client;
		private readonly CommandService _commands;
		private readonly IServiceProvider _services;
		private Program()
		{			
			_client = new DiscordSocketClient(new DiscordSocketConfig()
			{
				GatewayIntents = GatewayIntents.All
			});
			_services = ConfigureServices();
			ConfigureDatabases();
			_commands = new CommandService();
			_client.Log += Log;
			_commands.Log += Log;
			_client.ReactionAdded += OnReact;
			JobManager.Initialize();
			//We want this to run at 8 AM ET, but it looks like the host is GMT, so we need to set this to 12
			JobManager.AddJob(() => HungerGameUtilities.DailyEvent((HungerGamesService)_services.GetService(typeof(HungerGamesService)), (UserRecordsService)_services.GetService(typeof(UserRecordsService)), _client), s => s.ToRunEvery(1).Days().At(8, 0));
			JobManager.AddJob(() => ((RPGService)_services.GetService(typeof(RPGService))).LongRest(), s => s.ToRunEvery(1).Days().At(8, 0));
		}
		public static void Main(string[] args)
	=> new Program().MainAsync().GetAwaiter().GetResult();
		public async Task MainAsync()
		{
			Console.WriteLine($"DEEPSTATE has been INITIALIZED");

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

			new Thread( () => _ = LibcoinUtilities.LibcraftCoinCheck(_services.GetService<UserRecordsService>() ) ).Start();
			new Thread( () => _ = LibcoinUtilities.LibcoinReactionChecker(_services.GetService<UserRecordsService>() ) ).Start();
			// Block this task until the program is closed.
			await Task.Delay(-1);
		}
		private void ConfigureDatabases()
		{
			/* var oocContext = _services.GetService<OOCDBContext>();
			oocContext.Database.EnsureCreated();
			var userRecordContext = _services.GetService<GuildUserRecordContext>();
			userRecordContext.Database.EnsureCreated();
			var hungerGamesContext = _services.GetService<HungerGamesContext>();
			hungerGamesContext.Database.EnsureCreated(); */
		}
		private static IServiceProvider ConfigureServices()
		{
			var log = new LoggerConfiguration()
				.WriteTo.Console()
				.CreateLogger();
			//We don't have any services currently for DI
			//but once we do this is where we would add them.
			var map = new ServiceCollection()
				.AddSingleton<IHelpConfig, HelpConfig>()
				.AddSingleton<IBotInformation, BotInformation>()
				.AddSingleton<IMessageReliabilityService, MessageReliabilityService>()
				.AddSingleton<IJackboxService, JackboxService>()
				.AddSingleton<ImagingService>()
				.AddSingleton<OutOfContextService>()
				.AddSingleton<HungerGamesService>()
				.AddSingleton<UserRecordsService>()
				.AddSingleton<ModTeamRequestService>()
				.AddSingleton<RPGService>()
				.AddSingleton<ILogger>(log)
				.AddDbContext<OOCDBContext>()
				.AddDbContext<GuildUserRecordContext>()
				.AddDbContext<HungerGamesContext>()
				.AddDbContextFactory<OOCDBContext>()
				.AddDbContextFactory<GuildUserRecordContext>()
				.AddDbContextFactory<HungerGamesContext>()
				.AddDbContextFactory<ModTeamRequestContext>()
				.AddDbContextFactory<JackboxContext>()
				.AddDbContextFactory<RPGContext>();

			return map.BuildServiceProvider();
		}
		private Task Log(LogMessage msg)
		{
			Console.WriteLine($"[{DateTime.Now.ToString("g")}] ({msg.Source}): {msg.Message}");
			return Task.CompletedTask;
		}
		public async Task InstallCommandsAsync()
		{
#if !DEBUG
			
#endif
			await _commands.AddModuleAsync<MalarkeyModule>(_services);
			await _commands.AddModuleAsync<HelpModule>(_services);
			await _commands.AddModuleAsync<IndecisionModule>(_services);
			await _commands.AddModuleAsync<BotModule>(_services);
			await _commands.AddModuleAsync<ChatModule>(_services);
			await _commands.AddModuleAsync<OutOfContextModule>(_services);
			await _commands.AddModuleAsync<UserRecordsModule>(_services);
			await _commands.AddModuleAsync<HungerGamesModule>(_services);
			await _commands.AddModuleAsync<ModTeamRequestModule>(_services);
			await _commands.AddModuleAsync<JackboxModule>(_services);
			await _commands.AddModuleAsync<RPGModule>(_services);
			await _commands.AddModuleAsync<NFTModule>(_services);

#if !DEBUG
			
#endif
            _client.MessageReceived += (async (SocketMessage messageParam) =>
            {
                try
                {
                    _ = OMH.HandleCommandWithSummaryOnError(messageParam, new CommandContext(_client, (SocketUserMessage)messageParam), _commands, _services, BotProperties.CommandPrefix);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception from the DDB Command Handler:");
                    Console.WriteLine(ex.Message);
                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                        Console.WriteLine("Inner Exception: ");
                        Console.WriteLine(ex.Message);
                    }
                }
            });
			_client.GuildScheduledEventCreated += OnEventCreated;
			_client.MessageReceived += OnMessage;
			_client.ButtonExecuted += OnButtonClicked;

		}

		private async Task OnEventCreated(SocketGuildEvent arg)
        {
			new Task(() =>
			{
				OnEventCreatedHandlers.AnnounceNewEvent(arg);
			}).Start();
        }

        private async Task OnButtonClicked(SocketMessageComponent component)
		{
			new Thread(() => {
				_ = OnComponentInteractionHandlers.ProgressiveSharesDistributionHandler(component, _services);
			}).Start();
		}
		private async Task OnMessage(SocketMessage messageParam)
		{
			//Don't process the command if it was a system message
			SocketUserMessage message = messageParam as SocketUserMessage;
			IGuild guild = ((IGuildChannel)message.Channel).Guild;
			if (message == null) return;

			if (message.Author.IsBot)
			{
				//We don't want to process messages from bots. Screw bots, all my homies hate bots.
				return;
			}
			UserRecordsService urservice = _services.GetService<UserRecordsService>();
			new Thread(async () => { LibcoinUtilities.LibcraftCoinMessageHandler(messageParam, urservice); }).Start();
            new Thread(async () => { OnMessageHandlers.DownloadUsersForGuild(message, guild); }).Start();
			new Thread(async () => { OnMessageHandlers.DeletePreggersMessage(message); }).Start();

			if (!SharedConstants.NoAutoReactsChannel.Contains(message.Channel.Id))
			{
				new Thread(() => { OnMessageHandlers.EgoCheck(messageParam, Utils.IsMentioningMe(messageParam, _client.CurrentUser)); }).Start();
				new Thread(() => { _ = OnMessageHandlers.RandomReactCheck(messageParam); }).Start();
				new Thread(() => { OnMessageHandlers.Imposter(messageParam, Utils.IsSus(messageParam.Content)); }).Start();
				new Thread(() => { _ = OnMessageHandlers.MalarkeyLevelOfHandler(message); }).Start();
			}

		}
		private async Task OnReact(Cacheable<IUserMessage, ulong> cachedMessage, Cacheable<IMessageChannel,ulong> cachedChannel, SocketReaction reaction)
		{
			ISocketMessageChannel channel = (ISocketMessageChannel)cachedChannel.Value;
			IEmote reactionEmote;
			IMessage msg = channel.GetMessageAsync(cachedMessage.Id).Result;
			if ((reaction.Emote as Emote) != null)
			{
				reactionEmote = (Emote)reaction.Emote;
			}
			else
			{
				reactionEmote = (Emoji)reaction.Emote;
			}

			//One of the voting reactions, :x:, can also be used to clear DeepState reacts, so we run this regardless.
			new Thread(() => { _ = OnReactHandlers.ClearDeepStateReactionCheck(reactionEmote, channel, msg, _client.CurrentUser); }).Start();
			//We only want to process Msg.Author.IsBot requests here actually, so we put this before too.
			new Thread(() => { _ = ORH.EmbedPagingHandler(reaction, msg, _client.CurrentUser, PagedEmbedConstants.HungerGameTributesEmbedTitle, PagingUtilities.TributeEmbedPagingCallback, _services); }).Start();
			new Thread(() => { _ = ORH.EmbedPagingHandler(reaction, msg, _client.CurrentUser, PagedEmbedConstants.OpenRequestEmbedTitle, PagingUtilities.OpenRequestsPagingCallback, _services); }).Start();
			new Thread(() => { _ = ORH.EmbedPagingHandler(reaction, msg, _client.CurrentUser, PagedEmbedConstants.ClosedRequestEmbedTitle, PagingUtilities.ClosedRequestsPagingCallback, _services); }).Start();
			new Thread(() => { _ = ORH.EmbedPagingHandler(reaction, msg, _client.CurrentUser, PagedEmbedConstants.LibcoinBalancesEmbedTitle, PagingUtilities.LibcoinLeaderboardPagingCallback, _services); }).Start();
			new Thread(() => { _ = ORH.EmbedPagingHandler(reaction, msg, _client.CurrentUser, PagedEmbedConstants.LibcoinActiveUserListTitle, PagingUtilities.ActiveUsersPaginingCallback, _services); }).Start();
			new Thread(() => { _ = LibcoinUtilities.LibcoinReactHandler(reaction, msg.Channel as ISocketMessageChannel, msg); }).Start();

			if (SharedConstants.VotingEmotes.Contains(reaction.Emote.Name) || msg.Author.IsBot)
			{
				return;
			}

			new Thread(() => { _ = OnReactHandlers.KlaxonCheck(reactionEmote, channel, msg); }).Start();
		}
		
	}
}
