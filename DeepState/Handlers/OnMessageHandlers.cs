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
using DeepState.Data.Services;
using static DartsDiscordBots.Constants.SharedConstants;

namespace DeepState.Handlers
{
	public static class OnMessageHandlers
	{
		static string PreggersDetector = "[p,𝐩,𝞺,𝚙,ｐ,𝞀,ß,*,р]+[r]+[e,е,ô,ó,o,é,è,ė,ê,ë,@,ò,ö,ě,ĕ,*,ē,ẽ,ę,ȩ,ɇ,ế,ề,ḗ,ḕ,ễ,ḝ,ẻ,ȅ,ȇ,ể,ẹ,ḙ,ḛ,ệ]+[g,ġ,ℊ,𝒈,𝗀,𝕘,*]+[e,*,ô,ó,o,é,è,ė,ê,ë,@,ò,ö,ě,ĕ,ē,ẽ,ę,ȩ,ɇ,ế,ề,ḗ,ḕ,ễ,ḝ,ẻ,ȅ,ȇ,ể,ẹ,ḙ,ḛ,ệ]+[r,r,*]+s*";
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
			if(Regex.Matches(msg.Content, PreggersDetector, RegexOptions.IgnoreCase).Count > 0)
            {
                if (Utils.PercentileCheck(10))
                {
					_ = msg.Channel.SendMessageAsync("https://c.tenor.com/BH_8JPewRk4AAAAd/free-guy-ryan-reynolds.gif");
                }
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

		public static async Task TableFlipCheck(SocketMessage msg, IGuild guild, UserRecordsService userRecordService)
        {
            if (BotUtilities.isUserFlippingTable(msg.Content, out TableFlipType? type) && userRecordService.UserRecordExists(msg.Author.Id, guild.Id))
            {
				int tablesFlipped = userRecordService.IncrementTableflip(msg.Author.Id, guild.Id);
				TableFlipTier tier = GetTableflipTier(tablesFlipped);
                switch (type)
                {
                    case TableFlipType.Single:
						_ = msg.Channel.SendMessageAsync(UnflippedTable);
						break;
					case TableFlipType.Double:
                        _ = msg.Channel.SendMessageAsync(LeftDoubleUnflippedTable);
						_ = msg.Channel.SendMessageAsync(RightDoubleUnflippedTable);
						break;
					case TableFlipType.Enraged:
						_ = msg.Channel.SendMessageAsync(EnragedUnflippedTable);
						break;
				}

                _ = msg.Channel.SendMessageAsync(String.Format(TableFlipResponses[tier].GetRandom(), DartsDiscordBots.Utilities.BotUtilities.GetDisplayNameForUser((IGuildUser) msg.Author)));
            }
        }
		private static TableFlipTier GetTableflipTier(int tablesFlipped)
        {
			const int tierSize = 35;

			switch (tablesFlipped)
            {
				case < (1 * tierSize):
                    return TableFlipTier.Polite;
				case < (2 * tierSize):
					return TableFlipTier.Chastising;
				case < (3 * tierSize):
					return TableFlipTier.Aggresive;
				case < (4 * tierSize):
					return TableFlipTier.Scalding;
				case > (5 * tierSize):
					return TableFlipTier.NavySeal;
                default:
					return TableFlipTier.Polite;
            }
        }
	}
}
