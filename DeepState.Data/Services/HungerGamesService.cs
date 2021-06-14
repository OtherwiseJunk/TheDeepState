using DeepState.Data.Constants;
using DeepState.Data.Context;
using DeepState.Data.Models;
using Discord;
using Microsoft.EntityFrameworkCore;
using System;
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
				context.Tributes.Add(new HungerGamesTribute
				{
					DiscordGuildId = guildId,
					DiscordUserId = userId
				});

				context.SaveChanges();
			}

			_userRecordService.DeductFromBalance(userId, guildId, HungerGameConstants.CostOfAdmission);
		}

		public List<HungerGamesTribute> GetPagedTributeList(ulong guildId, out int successfulPage, int page = 0)
		{
			int firstTribute = 0 + (page * 10);

			using (HungerGamesContext context = _contextFactory.CreateDbContext())
			{
				List<HungerGamesTribute> pageData;
				List<HungerGamesTribute> guildTributes = context.Tributes.AsQueryable().Where(t => t.DiscordGuildId == guildId).OrderByDescending(t => t.IsAlive).ToList();
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
		public List<HungerGamesTribute> GetTributeList(ulong guildId)
		{
			using (HungerGamesContext context = _contextFactory.CreateDbContext())
			{
				return context.Tributes.AsQueryable().Where(t => t.DiscordGuildId == guildId).ToList(); ;
			}
		}

		public bool TributeAnnouncementConfigurationExists(ulong guildId)
		{
			using (HungerGamesContext context = _contextFactory.CreateDbContext())
			{
				return context.GuildConfigurations.FirstOrDefault(gc => gc.DiscordGuildId == guildId)?.TributeAnnouncementChannelId != null;
			}
		}

		public void SetTributeAnnouncementChannel(ulong guildId, ulong channelId)
		{
			using (HungerGamesContext context = _contextFactory.CreateDbContext())
			{
				if (TributeAnnouncementConfigurationExists(guildId))
				{
					HungerGamesServerConfiguration config = context.GuildConfigurations.First(gc => gc.DiscordGuildId == guildId);
					config.TributeAnnouncementChannelId = channelId;
				}
				else
				{
					context.GuildConfigurations.Add(new HungerGamesServerConfiguration
					{
						DiscordGuildId = guildId,
						TributeAnnouncementChannelId = channelId
					});
				}

				context.SaveChanges();
			}
		}

		public List<HungerGamesServerConfiguration> GetAllConfigurations()
		{
			using (HungerGamesContext context = _contextFactory.CreateDbContext())
			{
				return context.GuildConfigurations.ToList();
			}
		}

		public void KillTribute(ulong userId, ulong guildId, string deathMessage, string obituary, int district)
		{
			using (HungerGamesContext context = _contextFactory.CreateDbContext())
			{
				HungerGamesTribute deceasedTribute = context.Tributes.First(t => t.DiscordGuildId == guildId && t.DiscordUserId == userId);
				deceasedTribute.IsAlive = false;
				deceasedTribute.DeathMessage = deathMessage;
				deceasedTribute.ObituaryMessage = obituary;
				deceasedTribute.District = district;

				context.SaveChanges();
			}
		}

		public void SetCorpseAnnouncementChannel(ulong guildId, ulong channelId)
		{
			using (HungerGamesContext context = _contextFactory.CreateDbContext())
			{
				if (CorpseAnnouncementConfigurationExists(guildId))
				{
					HungerGamesServerConfiguration config = context.GuildConfigurations.First(gc => gc.DiscordGuildId == guildId);
					config.CorpseAnnouncementChannelId = channelId;
				}
				else
				{
					context.GuildConfigurations.Add(new HungerGamesServerConfiguration
					{
						DiscordGuildId = guildId,
						CorpseAnnouncementChannelId = channelId
					});
				}

				context.SaveChanges();
			}
		}

		public bool CorpseAnnouncementConfigurationExists(ulong guildId)
		{
			using (HungerGamesContext context = _contextFactory.CreateDbContext())
			{
				return context.GuildConfigurations.FirstOrDefault(gc => gc.DiscordGuildId == guildId)?.CorpseAnnouncementChannelId != null;
			}
		}

		public void EndGame(ulong id, List<HungerGamesTribute> tributes)
		{
			using(HungerGamesContext context = _contextFactory.CreateDbContext())
			{
				context.Tributes.RemoveRange(tributes);
				HungerGamesPrizePool pool = context.PrizePools.First(pp => pp.DiscordGuildId == id);
				context.PrizePools.Remove(pool);

				context.SaveChanges();
			}
		}
	}
}
