using DartsDiscordBots.Modules.Help;
using DartsDiscordBots.Modules.Bot;
using DartsDiscordBots.Modules.Indecision;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;
using TheDeepState.Constants;
using TheDeepState.Modules;
using DartsDiscordBots.Modules.Help.Interfaces;
using TheDeepState.Models;
using DartsDiscordBots.Modules.Bot.Interfaces;

namespace TheDeepState
{
	public class Program
	{
		private readonly DiscordSocketClient _client;
		private readonly CommandService _commands;
		private readonly IServiceProvider _services;

		private Program()
		{
			_client = new DiscordSocketClient();
			_services = ConfigureServices();
			_commands = new CommandService();			
			_client.Log += Log;
			_commands.Log += Log;
		}

		public static void Main(string[] args)
			=> new Program().MainAsync().GetAwaiter().GetResult();

		public async Task MainAsync()
		{
			Console.WriteLine($"{BotProperties.InternalName} has been INITIALIZED");

			await InstallCommandsAsync();
			//Eventually rig this up to take environment variables for docker support.
			Console.WriteLine("Please provide the bot token:");
			var token = Console.ReadLine();

			await _client.LoginAsync(TokenType.Bot, token);
			await _client.StartAsync();

			// Block this task until the program is closed.
			await Task.Delay(-1);
		}

		private static IServiceProvider ConfigureServices()
		{
			//We don't have any services currently for DI
			//but once we do this is where we would add them.
			var map = new ServiceCollection()
				.AddSingleton<IHelpConfig, HelpConfig>()
				.AddSingleton<IBotInformation, BotInformation>();				

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


			_client.MessageReceived += HandleCommandAsync;
		}
		private async Task HandleCommandAsync(SocketMessage messageParam)
		{
			//Don't process the command if it was a system message
			var message = messageParam as SocketUserMessage;
			if (message == null) return;

			int argPos = 0;

			// Determine if the message is a command based on the prefix and make sure no bots trigger commands
			if (!(message.HasCharPrefix(BotProperties.CommandPrefix, ref argPos) ||
				message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
				message.Author.IsBot)
				return;

			var context = new SocketCommandContext(_client, message);

			var test = await _commands.ExecuteAsync(
				context: context,
				argPos: argPos,
				services: _services);
		}
	}
}
