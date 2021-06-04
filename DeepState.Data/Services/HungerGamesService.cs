using DeepState.Data.Constants;
using DeepState.Data.Context;
using DeepState.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DeepState.Data.Services
{
	public class HungerGamesService
	{
		IDbContextFactory<HungerGamesContext> _contextFactory { get; set; }
		UserRecordsService _userRecordService { get; set; }
		public HungerGamesService(IDbContextFactory<HungerGamesContext> contextFactory, UserRecordsService userRecordsService)
		{
			_contextFactory = contextFactory;
			_userRecordService = userRecordsService;
		}

		private bool PrizePoolExists(ulong guildId)
		{
			using(HungerGamesContext context = _contextFactory.CreateDbContext())
			{
				return context.PrizePools.FirstOrDefault(p => p.DiscordGuildId == guildId) != null;
			}
		}

		public bool TributeExists(ulong guildId, ulong userId)
		{
			using (HungerGamesContext context = _contextFactory.CreateDbContext())
			{
				return context.Tributes.FirstOrDefault(t => t.DiscordGuildId == guildId && t.DiscordUserId == userId) != null;
			}
		}
		public void RegisterTribute(ulong guildId, ulong userId)
		{
			using (HungerGamesContext context = _contextFactory.CreateDbContext())
			{
				if (PrizePoolExists(guildId))
				{
					HungerGamesPrizePool prizePool = context.PrizePools.First(p => p.DiscordGuildId == guildId);
					prizePool.PrizePool += HungerGameConstants.CostOfAdmission;
				}
				else
				{
					context.PrizePools.Add(new HungerGamesPrizePool
					{
						DiscordGuildId = guildId,
						PrizePool = HungerGameConstants.CostOfAdmission
					});
				}
				context.Tributes.Add(new HungerGamesTributes
				{
					DiscordGuildId = guildId,
					DiscordUserId = userId
				});

				context.SaveChanges();
			}

			_userRecordService.DeductFromBalance(userId, guildId, HungerGameConstants.CostOfAdmission);
		}

		public List<HungerGamesTributes> GetTributeList(ulong guildId, out int successfulPage, int page = 0)
		{
			int lastTribute = 9 + (10 * page);
			int firstTribute = lastTribute - 9;

			using (HungerGamesContext context = _contextFactory.CreateDbContext())
			{
				List<HungerGamesTributes> pageData;
				try
				{
					pageData = context.Tributes.AsQueryable().Where(t => t.DiscordGuildId == guildId).ToList().GetRange(firstTribute, lastTribute);
				}
				catch
				{
					lastTribute = 9 + (10 * --page);
					firstTribute = lastTribute - 9;
					pageData = context.Tributes.AsQueryable().Where(t => t.DiscordGuildId == guildId).ToList().GetRange(firstTribute, lastTribute);
				}
				successfulPage = page;
				return pageData;
			}
		}
	}
}
