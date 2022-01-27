using Microsoft.EntityFrameworkCore;
using DeepState.Data.Context;
using System;
using DeepState.Data.Models;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.Statistics;
using DDBUtils = DartsDiscordBots.Utilities.BotUtilities;
using Discord;
using DeepState.Data.Utilities;

namespace DeepState.Data.Services
{
	public class UserRecordsService
	{
		public const double SMALLEST_PAYOUT = 0.00100000;
		public const double LARGEST_PAYOUT = 0.10000000;
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
					UserRecord userRecord = context.UserRecords.AsQueryable().FirstAsync(ur => ur.DiscordUserId == userId && ur.DiscordGuildId == guildId)
						.Result;
					List<UserRecord> guildRecords = GetGuildUserRecords(guildId);
					double totalCirculation = CalculateTotalCirculation(guildRecords);
					double pieceOfThePie = userRecord.LibcraftCoinBalance / totalCirculation;
					payoutAmount *= (1 - pieceOfThePie);
					userRecord.LibcraftCoinBalance += payoutAmount;
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

		public List<UserRecord> GetPagedGuildBalances(ulong guildId, out int successfulPage, int page = 0)
		{
			using (GuildUserRecordContext context = _contextFactory.CreateDbContext())
			{
				List<UserRecord> guildBalances = context.UserRecords.AsQueryable().Where(ur => ur.DiscordGuildId == guildId).OrderByDescending(ur => ur.LibcraftCoinBalance).ToList();
				return PagingUtilities.GetPagedList<UserRecord>(guildBalances, out successfulPage, page);
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
			return Math.Round(roll >= SMALLEST_PAYOUT ? roll : SMALLEST_PAYOUT, 8);
		}

		public List<UserRecord> GetGuildUserRecords(ulong guildId){
			using (GuildUserRecordContext context = _contextFactory.CreateDbContext())
			{
				return context.UserRecords.AsQueryable().Where(ur => ur.DiscordGuildId == guildId).ToList();
			}
		}

		public LibcoinEconomicStatistics CalculateEconomicStats(IGuild guild)
		{
			ulong guildId = guild.Id;
			List<UserRecord> guildRecords = GetGuildUserRecords(guildId);
			double totalCirculation = CalculateTotalCirculation(guildRecords);
			double meanBalance = guildRecords.Average(ur => ur.LibcraftCoinBalance);
			double medianBalance = guildRecords.Select(ur => ur.LibcraftCoinBalance).Median();
			ulong poorestUser = FindPoorestActiveUserId(guild, guildRecords);
			ulong richestUser = FindRichestActiveUserId(guild, guildRecords);
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

		public ulong FindPoorestActiveUserId(IGuild guild, List<UserRecord> guildRecords)
		{
			IGuildUser user = null;
			int skipCount = 0;
			guildRecords = guildRecords.OrderBy(ur => ur.LibcraftCoinBalance).ToList();
			while (user == null)
			{
				user = guild.GetUserAsync(guildRecords.Skip(skipCount).First().DiscordUserId, CacheMode.AllowDownload).Result;
				skipCount++;
			}

			return user.Id;
		}

		public ulong FindRichestActiveUserId(IGuild guild, List<UserRecord> guildRecords)
		{
			IGuildUser user = null;
			int skipCount = 0;
			guildRecords = guildRecords.OrderByDescending(ur => ur.LibcraftCoinBalance).ToList();
			while (user == null)
			{
				user = guild.GetUserAsync(guildRecords.Skip(skipCount).First().DiscordUserId, CacheMode.AllowDownload).Result;
				skipCount++;
			}

			return user.Id;
		}

		public void Grant(ulong userId, ulong guildId, double amount)
		{
			Console.WriteLine($"Attempting to grant {amount.ToString("F8")} libcoin to user {userId} from guild {guildId}");
			using (GuildUserRecordContext context = _contextFactory.CreateDbContext())
			{
				Console.WriteLine($"Attempting to lookup user {userId} in guild {guildId}...");
				UserRecord user = context.UserRecords.FirstOrDefault(ur => ur.DiscordUserId == userId && ur.DiscordGuildId == guildId);
				if (user != null)
				{
					Console.WriteLine($"Found user record! Adding {amount.ToString("F8")} libcoin to their balance of {user.LibcraftCoinBalance}...");
					user.LibcraftCoinBalance += amount;
					context.SaveChanges();
					Console.WriteLine($"Success! User now has a balance of {user.LibcraftCoinBalance}");
					return;
				}

				context.UserRecords.Add(new UserRecord
				{
					DiscordGuildId = guildId,
					DiscordUserId = userId,
					LibcraftCoinBalance = amount
				});
				context.SaveChanges();
			}
		}

		public bool Deduct(ulong userId, ulong guildId, double amount)
		{
			using (GuildUserRecordContext context = _contextFactory.CreateDbContext())
			{
				UserRecord user = context.UserRecords.FirstOrDefault(ur => ur.DiscordUserId == userId && ur.DiscordGuildId == guildId);
				if (user != null)
				{
					if(user.LibcraftCoinBalance >= amount)
					{
						user.LibcraftCoinBalance -= amount;
					}
					else
					{
						user.LibcraftCoinBalance = 0;
					}
					context.SaveChanges();
					return true;
				}

				return false;
			}
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
			return (-1 * (fair_area - area) / fair_area);

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
