using DeepState.Constants;
using DeepState.Data.Models;
using DeepState.Data.Services;
using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DDBUtils = DartsDiscordBots.Utilities.BotUtilities;

namespace DeepState.Modules
{
	public class ModTeamRequestModule : ModuleBase
	{
		ModTeamRequestService _requestService { get; set; }
		UserRecordsService _userRecordService { get; set; }

		public ModTeamRequestModule(ModTeamRequestService requestService, UserRecordsService userRecordService)
		{
			_requestService = requestService;
			_userRecordService = userRecordService;
		}

		[Command("request")]
		[Summary("Submit a request to the mod team. They may indicate a libcoin price for your request to be completed.")]
		public async Task AddModTeamRequest([Summary("The request to be sent to the mod team. Minimum 20 characters."), Remainder] string requestMessage)
		{
			if (requestMessage.Length < 20)
			{
				await Context.Channel.SendMessageAsync("Sorry, a request needs at least 20 characters. Can you expand a bit?");
			}
			else
			{
				int requestId = _requestService.CreateRequest(Context.Message.Author.Id, Context.Guild.Id, requestMessage);
				
				IMessage msg = Context.Channel.SendMessageAsync("Ok, I've submitted your request!").Result;
				ITextChannel requests = (ITextChannel) Context.Guild.GetChannelAsync(SharedConstants.RequestsChannelId).Result;
				await requests.SendMessageAsync($"{DDBUtils.GetDisplayNameForUser((IGuildUser)Context.Message.Author)} has submitted a new request: {requestId}. {requestMessage} {Environment.NewLine} Link: {msg.GetJumpUrl()}");
			}
		}

		[Command("requests")]
		[Summary("Submit a request to the mod team. They may indicate a libcoin price for your request to be completed.")]		
		public async Task GetOpenRequestLIst()
		{
			IGuildUser user = (IGuildUser) Context.User;
			Thread getRequests;
			if (user.GuildPermissions.ManageMessages)
			{
				getRequests = new Thread(() =>
				{
					List<ModTeamRequest> requests = _requestService.GetOpenModTeamRequestPage(Context.Guild.Id, out int successfulPage);
					Embed embed = _requestService.BuildRequestsEmebed(requests, successfulPage, Context.Guild, true);
					IUserMessage msg = Context.Channel.SendMessageAsync(embed: embed).Result;
					msg.AddReactionAsync(new Emoji("⬅️"));
					msg.AddReactionAsync(new Emoji("➡️"));
				});
			}
			else
			{
				getRequests = new Thread(() =>
				{
					List<ModTeamRequest> requests = _requestService.GetUsersRequestsPage(Context.Guild.Id, Context.User.Id, out int successfulPage);
					Embed embed = _requestService.BuildRequestsEmebed(requests, successfulPage, Context.Guild, true);
					IUserMessage msg = Context.Channel.SendMessageAsync(embed: embed).Result;
				});
			}
			getRequests.Start();
		}

		[Command("closed")]
		[Summary("Submit a request to the mod team. They may indicate a libcoin price for your request to be completed.")]
		[RequireUserPermission(ChannelPermission.ManageMessages, Group = SharedConstants.AdminsOnlyGroup), RequireOwner(Group = SharedConstants.AdminsOnlyGroup)]
		public async Task GetClosedRequestList()
		{
			new Thread(() => {
				List<ModTeamRequest> requests = _requestService.GetClosedModTeamRequestPage(Context.Guild.Id, out int successfulPage);
				Embed embed = _requestService.BuildRequestsEmebed(requests, successfulPage, Context.Guild, false);
				IUserMessage msg = Context.Channel.SendMessageAsync(embed: embed).Result;
				msg.AddReactionAsync(new Emoji("⬅️"));
				msg.AddReactionAsync(new Emoji("➡️"));
			}).Start();
		}

		[Command("price")]
		[Summary("Submit a request to the mod team. They may indicate a libcoin price for your request to be completed.")]
		[RequireUserPermission(ChannelPermission.ManageMessages, Group = SharedConstants.AdminsOnlyGroup), RequireOwner(Group = SharedConstants.AdminsOnlyGroup)]
		public async Task PriceRequest([Summary("The request ID to set the price for. Can be found from >requests.")]int requestId, [Summary("The price to set for the request")] double price)
		{
			price = Math.Abs(price);
			if (_requestService.OpenRequestExists(requestId))
			{
				_requestService.PriceRequest(requestId, Context.Message.Author.Id, price);
				_ = Context.Channel.SendMessageAsync($"Ok, I've set the price for Request {requestId} to {price.ToString("F8")} libcoins.");
				ModTeamRequest request = _requestService.GetRequest(requestId);
				IDMChannel channel = Context.Guild.GetUserAsync(request.RequestingUserDiscordId).Result.GetOrCreateDMChannelAsync().Result;
				await channel.SendMessageAsync($"{Context.Message.Author.Username} has set a price of {price.ToString("F8")} libcoin for your request: {request.Request}");
				ITextChannel requests = (ITextChannel)Context.Guild.GetChannelAsync(SharedConstants.RequestsChannelId).Result;
				await requests.SendMessageAsync($"{DDBUtils.GetDisplayNameForUser((IGuildUser)Context.Message.Author)} has priced request {requestId}. `{request.Request}`");
			}
			else
			{
				_ = Context.Channel.SendMessageAsync($"Sorry, I couldn't find a request for request number {requestId}");
			}
		}

		[Command("reject")]
		[Summary("Submit a request to the mod team. They may indicate a libcoin price for your request to be completed.")]
		[RequireUserPermission(ChannelPermission.ManageMessages, Group = SharedConstants.AdminsOnlyGroup), RequireOwner(Group = SharedConstants.AdminsOnlyGroup)]
		public async Task RejectRequest([Summary("The request ID to reject. Can be found from >requests.")] int requestId, [Summary("The request to be sent to the mod team. Minimum 20 characters."), Remainder] string rejectionMessage="")
		{
			if (_requestService.OpenRequestExists(requestId))
			{
				_requestService.RejectRequest(requestId, Context.Message.Author.Id, rejectionMessage);
				_ = Context.Channel.SendMessageAsync("Ok, I've rejected the request and let the user know.");
				ModTeamRequest request = _requestService.GetRequest(requestId);
				IDMChannel channel = Context.Guild.GetUserAsync(request.RequestingUserDiscordId).Result.GetOrCreateDMChannelAsync().Result;
				string message = $"{Context.Message.Author.Username} has rejected your request: {request.Request}";
				if (!String.IsNullOrEmpty(rejectionMessage))
				{
					message += $"{Environment.NewLine}{Environment.NewLine}Rejection Reason: {rejectionMessage}";
				}
				await channel.SendMessageAsync(message);
				ITextChannel requests = (ITextChannel)Context.Guild.GetChannelAsync(SharedConstants.RequestsChannelId).Result;
				await requests.SendMessageAsync($"{DDBUtils.GetDisplayNameForUser((IGuildUser)Context.Message.Author)} has rejected request {requestId}. `{request.Request}`");
			}
			else
			{
				_ = Context.Channel.SendMessageAsync($"Sorry, I couldn't find a request for request number {requestId}");
			}
		}

		[Command("complete"), Alias("jobdone")]
		[Summary("Submit a request to the mod team. They may indicate a libcoin price for your request to be completed.")]
		[RequireUserPermission(ChannelPermission.ManageMessages, Group = SharedConstants.AdminsOnlyGroup), RequireOwner(Group = SharedConstants.AdminsOnlyGroup)]
		public async Task CompleteRequest([Summary("The request ID to reject. Can be found from >requests.")] int requestId, [Summary("The request to be sent to the mod team. Minimum 20 characters."), Remainder] string completionMessage="")
		{
			if (_requestService.OpenRequestExists(requestId))
			{
				ModTeamRequest request = _requestService.GetRequest(requestId);
				double requestingUserBalance = _userRecordService.GetUserBalance(request.RequestingUserDiscordId, Context.Guild.Id);
				if (requestingUserBalance >= request.Price || request.Price == null)
				{
					_requestService.CompleteRequest(requestId, Context.Message.Author.Id, completionMessage);
					await Context.Channel.SendMessageAsync("Ok, I've marked that request as complete. Any price has been deducted from the users account.");
					IDMChannel channel = Context.Guild.GetUserAsync(request.RequestingUserDiscordId).Result.GetOrCreateDMChannelAsync().Result;
					if (request.Price != null)
					{
						double price = (double)request.Price;
						_userRecordService.Deduct(request.RequestingUserDiscordId, Context.Guild.Id, price);
						_ = channel.SendMessageAsync($"{Context.Message.Author.Username} has fufiled your request: {request.Request}. {price.ToString("F8")} libcoin was deducted from your account for this request. {completionMessage}]");
					}
					else
					{
						_ = channel.SendMessageAsync($"{Context.Message.Author.Username} has fufiled your request: {request.Request}. {completionMessage}");
					}

					ITextChannel requests = (ITextChannel)Context.Guild.GetChannelAsync(SharedConstants.RequestsChannelId).Result;
					await requests.SendMessageAsync($"{DDBUtils.GetDisplayNameForUser((IGuildUser)Context.Message.Author)} has completed request {requestId}. `{request.Request}`");
				}
				else
				{
					await Context.Channel.SendMessageAsync($"Actually, it looks like they don't have {((double)request.Price).ToString("F8")} libcoin, so the request can't be fufilled yet. Maybe lower the price? They have {requestingUserBalance.ToString("F8")} libcoin.");
				}
			}
			else
			{
				_ = Context.Channel.SendMessageAsync($"Sorry, I couldn't find a request for request number {requestId}");
			}
		}
	}
}
