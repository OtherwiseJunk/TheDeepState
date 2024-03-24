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
		public int IncrementTableflip(ulong userId, ulong guildId)
        {
			using(GuildUserRecordContext context = _contextFactory.CreateDbContext())
            {
                if (UserRecordExists(userId, guildId))
                {
                    UserRecord record = context.UserRecords.AsQueryable().First(ur => ur.DiscordUserId == userId && ur.DiscordGuildId == guildId);
                    record.TableFlipCount++;
                    context.SaveChanges();

					return record.TableFlipCount;
                }
				return 0;
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
		public void UpdateUserRecordActivity(ulong userId, ulong guildId)
		{
			using (GuildUserRecordContext context = _contextFactory.CreateDbContext())
			{
				if (UserRecordExists(userId, guildId))
				{
					UserRecord user = context.UserRecords.AsQueryable().First(ur => ur.DiscordUserId == userId && ur.DiscordGuildId == guildId);
					user.LastTimePosted = DateTime.Now;
					context.SaveChanges();
				}
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
		public List<UserRecord> GetPagedActiveUserRecords(ulong guildId, out int succesfulPage, int page = 0)
		{
			using (GuildUserRecordContext context = _contextFactory.CreateDbContext())
			{
				DateTime defaultDate = new DateTime(0001, 1, 1, 0, 0, 0);
				List<UserRecord> activeUsers = new List<UserRecord>();
				foreach (UserRecord record in context.UserRecords.AsQueryable().Where(ur => ur.DiscordGuildId == guildId))
				{
					if (record.LastTimePosted != defaultDate && DateTime.Now.Subtract(record.LastTimePosted).TotalDays <= 14)
					{
						activeUsers.Add(record);
					}
				}
				return PagingUtilities.GetPagedList<UserRecord>(activeUsers.OrderByDescending(ur => ur.LastTimePosted).ToList(), out succesfulPage, page);
			}
		}
		public List<UserProgressiveShare> GetPagedProgressiveShares(List<UserRecord> activePool, double amountToShare, double maximumAmount, out int succesfulPage, int page = 0)
		{
			List<UserProgressiveShare> shares = CalculateProgressiveShare(activePool, amountToShare, maximumAmount);
			return PagingUtilities.GetPagedList<UserProgressiveShare>(shares.OrderByDescending(ur => ur.ProgressiveShare).ToList(), out succesfulPage, page); 
		}
		public List<UserProgressiveShare> CalculateProgressiveShare(List<UserRecord> activePool, double amountToShare, double maximumAmount)
		{
			List<UserProgressiveShare> shares = new();
			double[] distribution = new double[activePool.Count];
			double totalCirculation = activePool.Sum(u => u.LibcraftCoinBalance);
			if (maximumAmount == 0)
			{
				maximumAmount = amountToShare;
			}
			int index = 0;
			foreach (UserRecord user in activePool.OrderByDescending(u => u.LibcraftCoinBalance))
			{
				distribution[index] = user.LibcraftCoinBalance / totalCirculation;
				index++;
			}
			index--;
			foreach (UserRecord user in activePool.OrderByDescending(u => u.LibcraftCoinBalance))
			{
				double share = Math.Round(amountToShare * distribution[index], 8);
				share = share > maximumAmount ? maximumAmount : share;
				shares.Add(new UserProgressiveShare { User = user, ProgressiveShare = share });
				index--;
			}

			return shares;
		}
		public List<UserRecord> GetActiveUserRecords(IGuild guild)
		{
			ulong guildId = guild.Id;
			using (GuildUserRecordContext context = _contextFactory.CreateDbContext())
			{
				DateTime defaultDate = new DateTime(0001, 1, 1, 0, 0, 0);
				List<UserRecord> activeUsers = new List<UserRecord>();
				foreach (UserRecord record in context.UserRecords.AsQueryable().Where(ur => ur.DiscordGuildId == guildId))
				{
					if (record.LastTimePosted != defaultDate && DateTime.Now.Subtract(record.LastTimePosted).TotalDays <= 14)
					{
						if(guild.GetUserAsync(record.DiscordUserId).Result != null)
						{
							activeUsers.Add(record);
						}						
					}
				}
				return activeUsers;
			}
		}
		public List<UserRecord> GetGuildUserRecords(ulong guildId)
		{
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
		public LibcoinEconomicStatistics CalculateActiveEconomicStats(IGuild guild)
		{			
			List<UserRecord> guildRecords = GetActiveUserRecords(guild);
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
					user.LibcraftCoinBalance = Math.Round(user.LibcraftCoinBalance, 8);
					context.SaveChanges();
					Console.WriteLine($"Success! User now has a balance of {user.LibcraftCoinBalance}");
					return;
				}

				context.UserRecords.Add(new UserRecord
				{
					DiscordGuildId = guildId,
					DiscordUserId = userId,
					LibcraftCoinBalance = Math.Round(amount, 8)
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
					if (user.LibcraftCoinBalance >= amount)
					{
						user.LibcraftCoinBalance -= amount;
					}
					else
					{
						user.LibcraftCoinBalance = 0;
					}
					user.LibcraftCoinBalance = Math.Round(user.LibcraftCoinBalance, 8);
					context.SaveChanges();
					return true;
				}

				return false;
			}
		}
		private double CalculateGiniCoefficient(List<double> balances)
		{
			balances = balances.OrderByDescending(b => b).ToList();
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
	public class UserProgressiveShare
	{
		public UserRecord User;
		public double ProgressiveShare;
	}
}
