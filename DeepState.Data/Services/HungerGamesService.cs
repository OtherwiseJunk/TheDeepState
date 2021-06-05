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
		private int PageSize = 10;
		public HungerGamesService(IDbContextFactory<HungerGamesContext> contextFactory, UserRecordsService userRecordsService)
		{
			_contextFactory = contextFactory;
			_userRecordService = userRecordsService;
		}

		public bool PrizePoolExists(ulong guildId)
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
		public double GetPrizePool(ulong guildId)
		{
			using(HungerGamesContext context = _contextFactory.CreateDbContext())
			{
				return context.PrizePools.First(p => p.DiscordGuildId == guildId).PrizePool;
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

		public List<HungerGamesTributes> GetPagedTributeList(ulong guildId, out int successfulPage, int page = 0)
		{
			int firstTribute = 0 + (page * 10);

			using (HungerGamesContext context = _contextFactory.CreateDbContext())
			{
				List<HungerGamesTributes> pageData;
				List<HungerGamesTributes> guildTributes = context.Tributes.AsQueryable().Where(t => t.DiscordGuildId == guildId).ToList();
				int tributeCount = guildTributes.Count();
				try
				{
					if (tributeCount >= firstTribute && (tributeCount - firstTribute) < PageSize)
					{
						pageData = guildTributes.GetRange(firstTribute, tributeCount - firstTribute);
					}
					else
					{
						pageData = guildTributes.GetRange(firstTribute, PageSize);
					}

				}
				catch
				{
					firstTribute = 0 + (--page * 10);
					pageData = guildTributes.GetRange(firstTribute, PageSize);
				}
				successfulPage = page;
				return pageData;
			}
		}
		public List<HungerGamesTributes> GetTributeList(ulong guildId)
		{
			using (HungerGamesContext context = _contextFactory.CreateDbContext())
			{
				return context.Tributes.AsQueryable().Where(t => t.DiscordGuildId == guildId).ToList(); ;
			}
		}
	}
}
