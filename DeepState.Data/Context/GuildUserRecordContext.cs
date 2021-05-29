using DeepState.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeepState.Data.Context
{
	class GuildUserRecordContext : DbContext
	{
		public GuildUserRecordContext(DbContextOptions options) : base(options)
		{
		}

		public const decimal SMALLEST_PAYOUT = 0.00000001m;
		public const decimal LARGEST_PAYOUT = 0.00000100m;

		public DbSet<UserRecord> UserRecords { get; set; }

		public bool UserRecordExists(ulong userId, ulong guildId)
		{
			return UserRecords.FirstOrDefaultAsync(ur => ur.DiscordUserId == userId && ur.DiscordGuildId == guildId) != null;
		}

		public void IssuePayout(ulong userId, ulong guildId)
		{
			decimal payoutAmount = RollPayout();
			if (UserRecordExists(userId, guildId))
			{
				UserRecords.FirstAsync(ur => ur.DiscordUserId == userId && ur.DiscordGuildId == guildId)
					.Result.LibcraftCoinBalance += payoutAmount;
			}
			else
			{
				UserRecords.Add(new UserRecord
				{
					DiscordUserId = userId,
					DiscordGuildId = guildId,
					LibcraftCoinBalance = payoutAmount,
					TimeOut = false
				}) ;
			}
		}
		public decimal GetUserBalance(ulong userId, ulong guildId)
		{
			return UserRecords.FirstAsync(ur => ur.DiscordUserId == userId && ur.DiscordGuildId == guildId)
					.Result.LibcraftCoinBalance;
		}

		public decimal RollPayout()
		{
			Random rand = new Random(DateTime.Now.Millisecond * DateTime.Now.Second);
			decimal roll = new decimal(rand.NextDouble() + 0.01) * LARGEST_PAYOUT;
			return roll >= SMALLEST_PAYOUT ? roll : SMALLEST_PAYOUT;
		}
	}
}
