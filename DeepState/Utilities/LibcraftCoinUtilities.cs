using DeepState.Data.Services;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Utils = DeepState.Utilities.Utilities;

namespace DeepState.Utilities
{
	public static class LibcraftCoinUtilities
	{
		private static object DictionaryLock = new object();
		private static Dictionary<ulong, List<ulong>> LCCListOfActiveUsersByGuild = new Dictionary<ulong, List<ulong>>();
		public static async Task LibcraftCoinCheck(UserRecordsService service)
		{
			Random rand = Utils.CreateSeededRandom();
			int nextDuration = rand.Next(60000, 240000);
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
			new Thread(async () => _ = LibcraftCoinCheck(service)).Start();
		}
		public static async Task LibcraftCoinMessageHandler(SocketMessage msg)
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
	}
}
