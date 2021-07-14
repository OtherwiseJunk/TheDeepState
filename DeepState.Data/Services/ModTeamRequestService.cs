using DeepState.Data.Context;
using DeepState.Data.Models;
using DeepState.Data.Utilities;
using Discord;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using DDBUtils = DartsDiscordBots.Utilities.BotUtilities;

namespace DeepState.Data.Services
{
	public class ModTeamRequestService
	{
		ILogger _logger { get; set; }
		IDbContextFactory<ModTeamRequestContext> _dbContextFactory { get; set; }
		public ModTeamRequestService(ILogger logger, IDbContextFactory<ModTeamRequestContext> dbContextFactory)
		{
			_logger = logger;
			_dbContextFactory = dbContextFactory;
		}

		public bool OpenRequestExists(int requestId)
		{
			using (ModTeamRequestContext context = _dbContextFactory.CreateDbContext())
			{
				return context.Requests.AsQueryable().Where(mtr => mtr.Status == RequestStatus.Opened || mtr.Status == RequestStatus.Priced)
					.FirstOrDefault(mtr => mtr.RequestId == requestId) != null;
			}
		}

		public ModTeamRequest GetRequest(int requestId)
		{
			using(ModTeamRequestContext context = _dbContextFactory.CreateDbContext())
			{
				return context.Requests.First(mtr => mtr.RequestId == requestId);
			}
		}

		public void CreateRequest(ulong userId, ulong guildId, string RequestMessage)
		{
			using(ModTeamRequestContext context = _dbContextFactory.CreateDbContext())
			{
				_logger.Information($"[ModTeamRequestService] Creating a request for userID {userId} in guild {guildId}: {RequestMessage}");
				context.Add(new ModTeamRequest
				{
					RequestingUserDiscordId = userId,
					DiscordGuildId = guildId,
					Request = RequestMessage
				});
				context.SaveChanges();
			}
		}

		public void PriceRequest(int requestId, ulong modDiscordId, double price)
		{
			_logger.Information($"[ModTeamRequestService] Mod {modDiscordId} is setting a price of {price.ToString("F8")} for request {requestId}.");
			using (ModTeamRequestContext context = _dbContextFactory.CreateDbContext())
			{
				ModTeamRequest request = context.Requests.First(mtr => mtr.RequestId == requestId);
				request.Price = price;
				request.modifyingModDiscordId = modDiscordId;
				request.Status = RequestStatus.Priced;
				request.UpdateDatetime = DateTime.Now;
				context.SaveChanges();
			}
		}

		public void RejectRequest(int requestId, ulong modDiscordId, string rejectionReason)
		{
			_logger.Information($"[ModTeamRequestService] Mod {modDiscordId} has rejected request {requestId}. {rejectionReason}");
			using (ModTeamRequestContext context = _dbContextFactory.CreateDbContext())
			{
				ModTeamRequest request = context.Requests.First(mtr => mtr.RequestId == requestId);
				request.modifyingModDiscordId = modDiscordId;
				request.Status = RequestStatus.Rejected;
				request.ClosingMessage = rejectionReason;
				request.UpdateDatetime = DateTime.Now;
				context.SaveChanges();
			}
		}
		public void CompleteRequest(int requestId, ulong modDiscordId, string completionMessage)
		{
			_logger.Information($"[ModTeamRequestService] Mod {modDiscordId} has completed request {requestId}. {completionMessage}");
			using (ModTeamRequestContext context = _dbContextFactory.CreateDbContext())
			{
				ModTeamRequest request = context.Requests.First(mtr => mtr.RequestId == requestId);
				request.modifyingModDiscordId = modDiscordId;
				request.Status = RequestStatus.Completed;
				request.ClosingMessage = completionMessage;
				request.UpdateDatetime = DateTime.Now;
				context.SaveChanges();
			}

		}
		public List<ModTeamRequest> GetUsersRequestsPage(ulong guildId, ulong userId, out int successfulPage, int page = 0)
		{
			using (ModTeamRequestContext context = _dbContextFactory.CreateDbContext())
			{
				List<ModTeamRequest> userRequests = context.Requests.AsQueryable()
					.Where(mtr => mtr.DiscordGuildId == guildId && mtr.RequestingUserDiscordId == userId && (mtr.Status == RequestStatus.Priced || mtr.Status == RequestStatus.Opened)).ToList();
				return PagingUtilities.GetPagedList<ModTeamRequest>(userRequests, out successfulPage, page);
			}
		}
		public List<ModTeamRequest> GetOpenModTeamRequestPage(ulong guildId, out int successfulPage, int page = 0)
		{
			using (ModTeamRequestContext context = _dbContextFactory.CreateDbContext())
			{
				List<ModTeamRequest> guildRequests = context.Requests.AsQueryable()
					.Where(mtr => mtr.DiscordGuildId == guildId && (mtr.Status == RequestStatus.Priced || mtr.Status == RequestStatus.Opened)).ToList();
				return PagingUtilities.GetPagedList<ModTeamRequest>(guildRequests, out successfulPage, page);
			}
		}
		public List<ModTeamRequest> GetClosedModTeamRequestPage(ulong guildId, out int successfulPage, int page = 0)
		{
			using(ModTeamRequestContext context = _dbContextFactory.CreateDbContext())
			{
				List<ModTeamRequest> guildRequests = context.Requests.AsQueryable()
					.Where(mtr => mtr.DiscordGuildId == guildId && (mtr.Status == RequestStatus.Completed || mtr.Status == RequestStatus.Rejected)).ToList();
				return PagingUtilities.GetPagedList<ModTeamRequest>(guildRequests, out successfulPage, page);
			}
		}

		public Embed BuildRequestsEmebed(List<ModTeamRequest> requests, int currentPage, IGuild guild, bool openRequests)
		{

			EmbedBuilder embed = new EmbedBuilder();
			if (openRequests)
			{
				embed.Title = $"Open Mod Team Requests";
			}
			else
			{
				embed.Title = $"Closed Mod Team Requests";
			}
			foreach(ModTeamRequest request in requests)
			{
				IGuildUser requestingUser = guild.GetUserAsync(request.RequestingUserDiscordId).Result;
				IGuildUser modTeamMemberUser = guild.GetUserAsync(request.modifyingModDiscordId).Result;
				embed.AddField($"{request.RequestId}.{DDBUtils.GetDisplayNameForUser(requestingUser)} - {request.Request}",
					BuildStatusMessage(request, modTeamMemberUser));
			}
			embed.WithFooter($"{currentPage}");

			return embed.Build();
		}

		internal string BuildStatusMessage(ModTeamRequest request, IGuildUser modTeamMemberUser)
		{
			string fieldMessage;
			if (request.Price != null)
			{
				fieldMessage = $"**Status:**{request.Status.ToString()} **Created On:**{request.CreationDatetime.ToString("yyyy-MM-dd - HH:mm tt")} {Environment.NewLine}" +
				$"**Current Price:** {request.Price}";
			}
			else
			{
				fieldMessage = $"**Status:**{request.Status.ToString()} **Created On:**{request.CreationDatetime.ToString("yyyy-MM-dd - HH:mm tt")} {Environment.NewLine}";
			}

			if (modTeamMemberUser != null)
			{
				fieldMessage += $"{Environment.NewLine}**Last Modified By:** {DDBUtils.GetDisplayNameForUser(modTeamMemberUser)} on {request.UpdateDatetime.ToString("yyyy-MM-dd - HH:mm tt")}";
			}

			return fieldMessage;
		}
	}
}
