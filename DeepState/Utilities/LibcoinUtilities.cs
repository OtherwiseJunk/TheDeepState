using DeepState.Constants;
using DeepState.Data.Models;
using DeepState.Data.Services;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DDBUtils = DartsDiscordBots.Utilities.BotUtilities;
using Utils = DeepState.Utilities.Utilities;

namespace DeepState.Utilities
{
	public static class LibcoinUtilities
	{
		private static object ActivityDictionaryLock = new object();
		private static object ReactionDictionaryLock = new object();

		public static IEmote bog = Emote.Parse(SharedConstants.BogId);
		public static IEmote laughingFace = Emote.Parse(SharedConstants.LaughingFaceID);
		public static List<IEmote> RewardEmotes = new List<IEmote> { bog, laughingFace };

		private static Dictionary<ulong, List<ulong>> LCCListOfActiveUsersByGuild = new Dictionary<ulong, List<ulong>>();
		private static Dictionary<ulong, IUserMessage> MessageByIdReactionTracker = new Dictionary<ulong, IUserMessage>();
		public static async Task LibcraftCoinCheck(UserRecordsService service)
		{
			Random rand = Utils.CreateSeededRandom();
			int nextDuration = rand.Next(120000, 480000);
			lock (ActivityDictionaryLock)
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
			new Thread(() => LibcraftCoinCheck(service)).Start();
		}
		public static async Task LibcraftCoinMessageHandler(SocketMessage msg, UserRecordsService service)
		{
			ulong messageUserId = msg.Author.Id;
			ulong messageGuildId = ((IGuildChannel)msg.Channel).GuildId;
			lock (ActivityDictionaryLock)
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
			service.UpdateUserRecordActivity(messageUserId, messageGuildId);
		}

		public static async Task LibcoinReactHandler(SocketReaction reaction, ISocketMessageChannel channel, IMessage msg) {
			if(msg.Channel as IGuildChannel != null)
			{
				lock (ReactionDictionaryLock)
				{
					if (!MessageByIdReactionTracker.Keys.Contains(msg.Id))
					{
						MessageByIdReactionTracker.Add(msg.Id, msg as IUserMessage);
					}
				}
			}
		}

		public static async Task LibcoinReactionChecker(UserRecordsService service)
		{
			Random rand = Utils.CreateSeededRandom();
			int nextDuration = rand.Next((7 * 60 * 1000), rand.Next(14 * 60 * 1000));
			List<ulong> handledKeys = new List<ulong>();

			lock (ReactionDictionaryLock)
			{
				foreach (ulong id in MessageByIdReactionTracker.Keys)
				{
					IUserMessage msg = MessageByIdReactionTracker[id];
					if((DateTime.Now - msg.Timestamp.LocalDateTime).TotalMinutes >= 5)
					{
						foreach (IEmote emote in RewardEmotes)
						{
							if (msg.Reactions.ContainsKey(emote)){
								if (msg.Reactions[emote].ReactionCount >= 10)
								{
									service.Grant(msg.Author.Id, (msg.Channel as IGuildChannel).GuildId, UserRecordsService.LARGEST_PAYOUT);

								}
								else if (msg.Reactions[emote].ReactionCount >= 5)
								{
									service.Grant(msg.Author.Id, (msg.Channel as IGuildChannel).GuildId, UserRecordsService.SMALLEST_PAYOUT);
								}
							}
						}
						handledKeys.Add(id);
					}
				}
				foreach(ulong id in handledKeys)
				{
					MessageByIdReactionTracker.Remove(id);
				}
			}
			Thread.Sleep(nextDuration);
			new Thread(() => _ = LibcraftCoinCheck(service)).Start();
		}

		internal static Embed BuildLeaderboardEmbed(List<UserRecord> userRecords, int currentPage, IGuild guild)
		{
			userRecords = userRecords.OrderByDescending(ur => ur.LibcraftCoinBalance).ToList();

			EmbedBuilder embed = new EmbedBuilder();
			embed.Title = PagedEmbedConstants.LibcoinBalancesEmbedTitle;

			int place = 1;
			foreach (UserRecord record in userRecords)
			{
				IGuildUser user = guild.GetUserAsync(record.DiscordUserId, CacheMode.AllowDownload).Result;
				string userName = DDBUtils.GetDisplayNameForUser(user);

				embed.AddField($"{place + ( currentPage * 10)}. {userName}", $"{record.LibcraftCoinBalance.ToString("F8")}");
				place++;
			}
			embed.WithFooter($"{currentPage}");

			return embed.Build();
		}

		internal static Embed BuildActiveUserEmbed(List<UserRecord> activeRecords, int currentPage, IGuild guild)
		{
			activeRecords = activeRecords.OrderByDescending(ur => ur.LastTimePosted).ToList();

			EmbedBuilder embed = new EmbedBuilder();
			embed.Title = PagedEmbedConstants.LibcoinActiveUserListTitle;

			int place = 1;
			foreach (UserRecord record in activeRecords)
			{
				IGuildUser user = guild.GetUserAsync(record.DiscordUserId, CacheMode.AllowDownload).Result;
				string userName = DDBUtils.GetDisplayNameForUser(user);

				embed.AddField($"{place + (currentPage * 10)}. {userName}",$"Last Activity: {TimeZoneInfo.ConvertTimeFromUtc(record.LastTimePosted, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"))} ET");
				place++;
			}
			embed.WithFooter($"{currentPage}");

			return embed.Build();
		}
	}
}
