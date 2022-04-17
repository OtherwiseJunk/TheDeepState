using Discord.WebSocket;
using System;
using Utils = DeepState.Utilities.Utilities;
using System.Threading;
using System.Threading.Tasks;
using DeepState.Constants;
using DartsDiscordBots.Utilities;
using Discord;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DeepState.Handlers
{
	public static class OnMessageHandlers
	{
		static HashSet<ulong> GuildUserCacheDownloaded = new();
		static object HashsetLock = new();
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
		public static async Task DownloadUsersForGuild(SocketMessage msg, IGuild guild)
		{
			if(!GuildUserCacheDownloaded.Contains(guild.Id)){				
				await guild.DownloadUsersAsync();
				lock (HashsetLock)
				{
					GuildUserCacheDownloaded.Add(guild.Id);
				}				
			}
		}

		public static async Task DeletePreggersMessage(SocketMessage msg)
        {
			if(Regex.Matches(msg.Content, "p+r+e+g+e+r+s+", RegexOptions.IgnoreCase).Count > 0)
            {
                _ = msg.Channel.SendMessageAsync("Gwalms.........");
				_ = msg.DeleteAsync();
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
			if(msg.Content.ToLower() == "!eank" && Utils.PercentileCheck(10))
			{
				msg.Channel.SendMessageAsync("J0nny5 detected. Preparing to terminate...");
			}
		}

		public static async Task MalarkeyLevelOfHandler(SocketMessage msg)
		{
			if (msg.Content.ToLower().Contains("malarkey level of"))
			{
				DateTime now = DateTime.Now;
				if(now.Day == 1 && now.Month == 4)
				{
					_ = msg.AddReactionAsync(SharedConstants.AprilFoolsMalarkeyLevels.GetRandom());
				}
				else
				{
					_ = msg.AddReactionAsync(SharedConstants.MalarkeyLevels.GetRandom());
				} 
			}
		}
	}
}
