using Microsoft.EntityFrameworkCore;
using DeepState.Data.Context;
using System;
using DeepState.Data.Models;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.Statistics;

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
				return context.UserRecords.AsQueryable().FirstOrDefaultAsync(ur => ur.DiscordUserId == userId && ur.DiscordGuildId == guildId).Result != null;
			}
		}
		public void IssuePayout(ulong userId, ulong guildId)
		{
			double payoutAmount = RollPayout();
			using (GuildUserRecordContext context = _contextFactory.CreateDbContext())
			{
				if (UserRecordExists(userId, guildId))
				{
					context.UserRecords.AsQueryable().FirstAsync(ur => ur.DiscordUserId == userId && ur.DiscordGuildId == guildId)
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
		public void DeductFromBalance(ulong userId, ulong guildId, double amountToDeduct)
		{
			using (GuildUserRecordContext context = _contextFactory.CreateDbContext())
			{
				UserRecord user = context.UserRecords.AsQueryable().FirstAsync(ur => ur.DiscordUserId == userId && ur.DiscordGuildId == guildId).Result;
				user.LibcraftCoinBalance -= amountToDeduct;

				context.SaveChanges();
			}
		}
		public double GetUserBalance(ulong userId, ulong guildId)
		{
			using (GuildUserRecordContext context = _contextFactory.CreateDbContext())
			{
				UserRecord user = context.UserRecords.AsQueryable().FirstAsync(ur => ur.DiscordUserId == userId && ur.DiscordGuildId == guildId).Result;
				return user != null ? user.LibcraftCoinBalance : 0.0;
			}
		}
		public double RollPayout()
		{
			Random rand = new Random(Guid.NewGuid().GetHashCode());
			double roll = (rand.NextDouble() + 0.01) * LARGEST_PAYOUT;
			return roll >= SMALLEST_PAYOUT ? roll : SMALLEST_PAYOUT;
		}
		public List<UserRecord> GetGuildTopTenBalances(ulong guildId)
		{
			using (GuildUserRecordContext context = _contextFactory.CreateDbContext())
			{
				return GetGuildUserRecords(guildId).OrderByDescending(ur => ur.LibcraftCoinBalance).Take(10).ToList();
			}
		}

		private List<UserRecord> GetGuildUserRecords(ulong guildId){
			using (GuildUserRecordContext context = _contextFactory.CreateDbContext())
			{
				return context.UserRecords.AsQueryable().Where(ur => ur.DiscordGuildId == guildId).ToList();
			}
		}

		public LibcoinEconomicStatistics CalculateEconomicStats(ulong guildId)
		{
			List<UserRecord> guildRecords = GetGuildUserRecords(guildId);
			double totalCirculation = CalculateTotalCirculation(guildRecords);
			double meanBalance = guildRecords.Average(ur => ur.LibcraftCoinBalance);
			double medianBalance = guildRecords.Select(ur => ur.LibcraftCoinBalance).Median();
			ulong poorestUser = guildRecords.OrderBy(ur => ur.LibcraftCoinBalance).First().DiscordUserId;
			ulong richestUser = guildRecords.OrderByDescending(ur => ur.LibcraftCoinBalance).First().DiscordUserId;
			double giniCoeffiecient = CalculateGiniCoefficient(guildRecords.Select(ur => ur.LibcraftCoinBalance).ToList());

			return new LibcoinEconomicStatistics
			{
				TotalCirculation = totalCirculation,
				MeanBalance = meanBalance,
				MedianBalance = medianBalance,
				PoorestUser = poorestUser,
				RichestUser = richestUser,
				GiniCoefficient = giniCoeffiecient
			};
		}

		private double CalculateGiniCoefficient(List<double> balances)
		{
			double height = 0;
			double area = 0;
			double fair_area = 0;

			foreach (double balance in balances)
			{
				height += balance;
				area += height - (balance / 2);
			}

			fair_area = height * (balances.Count / 2);
			return (fair_area - area) / fair_area;

		}
		private double CalculateTotalCirculation(List<UserRecord> economy)
		{
			double Circulation = 0.0;
			foreach (UserRecord userBalance in economy)
			{
				Circulation += userBalance.LibcraftCoinBalance;
			}
			return Circulation;
		}
	}
}
