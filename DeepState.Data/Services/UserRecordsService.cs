using Microsoft.EntityFrameworkCore;
using DeepState.Data.Context;
using System;
using DeepState.Data.Models;

namespace DeepState.Data.Services
{
	public class UserRecordsService
	{
		public const double SMALLEST_PAYOUT = 0.00000001;
		public const double LARGEST_PAYOUT = 0.00000100;
		IDbContextFactory<GuildUserRecordContext> _contextFactory { get; set; }
		public UserRecordsService(IDbContextFactory<GuildUserRecordContext> contextFactory)
		{
			_contextFactory = contextFactory;
		}

		public bool UserRecordExists(ulong userId, ulong guildId)
		{
			using (GuildUserRecordContext context = _contextFactory.CreateDbContext())
			{
				return context.UserRecords.FirstOrDefaultAsync(ur => ur.DiscordUserId == userId && ur.DiscordGuildId == guildId).Result != null;
			}				
		}

		public void IssuePayout(ulong userId, ulong guildId)
		{
			double payoutAmount = RollPayout();
			using (GuildUserRecordContext context = _contextFactory.CreateDbContext())
			{
				if (UserRecordExists(userId, guildId))
				{
					context.UserRecords.FirstAsync(ur => ur.DiscordUserId == userId && ur.DiscordGuildId == guildId)
						.Result.LibcraftCoinBalance += payoutAmount;
				}
				else
				{
					context.UserRecords.Add(new UserRecord
					{
						DiscordUserId = userId,
						DiscordGuildId = guildId,
						LibcraftCoinBalance = payoutAmount,
						TimeOut = false
					});
				}

				context.SaveChanges();
			}				
		}
		public double GetUserBalance(ulong userId, ulong guildId)
		{
			using (GuildUserRecordContext context = _contextFactory.CreateDbContext())
			{
				return context.UserRecords.FirstAsync(ur => ur.DiscordUserId == userId && ur.DiscordGuildId == guildId)
					.Result.LibcraftCoinBalance;
			}
		}

		public double RollPayout()
		{
			Random rand = new Random(Guid.NewGuid().GetHashCode());
			double roll = (rand.NextDouble() + 0.01) * LARGEST_PAYOUT;
			return roll >= SMALLEST_PAYOUT ? roll : SMALLEST_PAYOUT;
		}
	}
}
