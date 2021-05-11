﻿using DartsDiscordBots.Modules.Help;
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
using System.Collections.Generic;
using DartsDiscordBots.Utilities;
using System.Linq;
using DartsDiscordBots.Modules.Chat;
using DartsDiscordBots.Services.Interfaces;
using DartsDiscordBots.Services;
using System.Text.RegularExpressions;

namespace TheDeepState
{
	public class Program
	{
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
			_commands = new CommandService();
			_client.Log += Log;
			_commands.Log += Log;
			_rand = new Random(DateTime.Now.Millisecond);
			_client.ReactionAdded += OnReact;
		}

		private async Task OnReact(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction)
		{
			if (SharedConstants.VotingEmotes.Contains(reaction.Emote.Name))
			{
				return;
			}

			IMessage msg = channel.GetMessageAsync(message.Id).Result;
			Emote reactionEmote = (Emote)reaction.Emote;

			if (SharedConstants.EmoteNameandId(reactionEmote.Name, reactionEmote.Id) == SharedConstants.QIID && msg.Reactions[Emote.Parse(SharedConstants.QIID)].ReactionCount == 1)
			{
				await channel.SendMessageAsync(SharedConstants.KlaxonResponse, messageReference: new MessageReference(msg.Id));
			}
			else if (msg.Reactions.Count == 1)
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
				.AddSingleton<IMessageReliabilityService, MessageReliabilityService>();

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

			_client.MessageReceived += HandleCommandAsync;
		}
		private async Task HandleCommandAsync(SocketMessage messageParam)
		{

			

			//Don't process the command if it was a system message
			var message = messageParam as SocketUserMessage;
			if (message == null) return;

			if (IsMentioningMe(messageParam) && !message.Author.IsBot)
			{
				messageParam.AddReactionAsync(Emote.Parse(SharedConstants.RomneyRightEyeID));
				messageParam.AddReactionAsync(Emote.Parse(SharedConstants.RomneyLeftEyeID));
			}

			int argPos = 0;
			if (message.Channel.Id != SharedConstants.SelfCareChannelId)
			{
				if (message.Content.ToLower() == "!rank") Console.WriteLine("Rolling for rank...");
				if (message.Content.ToLower() == "!rank" && PercentileCheck(1))
				{
					await message.Channel.SendMessageAsync(RankNerdResponses.GetRandom());
					return;
				}
				if (PercentileCheck(1) & PercentileCheck(40))
				{
					if (PercentileCheck(1))
					{
						await message.AddReactionAsync(Emote.Parse(SharedConstants.GwalmsID));
					}
					else
					{
						await message.AddReactionAsync(Emote.Parse(ReactableEmotes.GetRandom()));
					}
				}
			}
			// Determine if the message is a command based on the prefix and make sure no bots trigger commands
			if (!(message.HasCharPrefix(BotProperties.CommandPrefix, ref argPos) ||
					message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
					message.Author.IsBot)
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
