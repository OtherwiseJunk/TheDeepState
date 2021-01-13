using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Reflection;
using System.Threading.Tasks;
using TheDeepState.Constants;

namespace TheDeepState
{
	public class Program
	{
		private readonly DiscordSocketClient _client;
		private readonly CommandService _commands;

		private Program()
		{
			_client = new DiscordSocketClient();
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

		private Task Log(LogMessage msg)
		{
			Console.WriteLine($"[{DateTime.Now.ToString("g")}] ({msg.Source}): {msg.Message}");
			return Task.CompletedTask;
		}
		public async Task InstallCommandsAsync()
		{
			_client.MessageReceived += HandleCommandAsync;

			//This will find all the Discord Modules in the current assumbly.
			//Additional assemblies can be specified depending on needs.
			await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(),
											services: null);
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

			await _commands.ExecuteAsync(
				context: context,
				argPos: argPos,
				services: null);
		}
	}
}
