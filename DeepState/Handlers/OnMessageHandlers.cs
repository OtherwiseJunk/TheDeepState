using Discord.WebSocket;
using System;
using Utils = DeepState.Utilities.Utilities;
using System.Threading;
using System.Threading.Tasks;
using DeepState.Constants;
using DartsDiscordBots.Utilities;
using Discord;
using System.Linq;

namespace DeepState.Handlers
{
	public static class OnMessageHandlers
	{
		public static void EgoCheck(SocketMessage msg, bool isMentioningMe)
		{
			if (isMentioningMe)
			{
				_ = msg.AddReactionAsync(Emote.Parse(SharedConstants.RomneyRightEyeID));
				_ = msg.AddReactionAsync(Emote.Parse(SharedConstants.RomneyLeftEyeID));
			}
		}
		public static void Imposter(SocketMessage msg, bool isSus)
		{
			if (isSus)
			{
				_ = msg.AddReactionAsync(Emote.Parse(SharedConstants.SusID));
			}
		}
		public static async Task RandomReactCheck(SocketMessage msg)
		{
			if (msg.Content.ToLower() == "!rank") Console.WriteLine("Rolling for rank...");
			if (msg.Content.ToLower() == "!rank" && Utils.PercentileCheck(1))
			{
				await msg.Channel.SendMessageAsync(SharedConstants.RankNerdResponses.GetRandom());
				return;
			}
			if (Utils.PercentileCheck(1) & Utils.PercentileCheck(40))
			{
				if (Utils.PercentileCheck(1))
				{
					await msg.AddReactionAsync(Emote.Parse(SharedConstants.GwalmsID));
				}
				else
				{
					await msg.AddReactionAsync(Emote.Parse(SharedConstants.ReactableEmotes.GetRandom()));
				}
			}
		}

		public static async Task MalarkeyLevelOfHandler(SocketMessage msg)
		{
			if(msg.Content.ToLower().Contains("malarkey level of"))
			{
				var response = msg.AddReactionAsync(SharedConstants.MalarkeyLevels.GetRandom());
			}
		}

		public static async Task CommandInDiscordSuggestionsHandler(SocketMessage msg)
		{
			if (msg.Content.First() == BotProperties.CommandPrefix && msg.Channel.Id == SharedConstants.DiscordSuggestionsChannelId)
			{
				_ = msg.Author.GetOrCreateDMChannelAsync().Result.SendMessageAsync("Hey, I saw that. YOU KNOW WHERE MY COMMANDS SHOULD GO !");
				_ = msg.DeleteAsync();
			}
		}
	}
}
