using DartsDiscordBots.Services.Interfaces;
using DeepState.Constants;
using DeepState.Data.Models;
using DeepState.Data.Services;
using DeepState.Modules.Preconditions;
using DeepState.Utilities;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DDBUtils = DartsDiscordBots.Utilities.BotUtilities;

namespace DeepState.Modules
{
	public class UserRecordsModule : ModuleBase
	{
		public int DartoshiConstant = 100000000;
		private UserRecordsService _UserRecordsService { get; set; }
		private IMessageReliabilityService _MessageReliabilityService { get; set; }
		public UserRecordsModule(UserRecordsService service, IMessageReliabilityService messageReliabilityService)
		{
			_UserRecordsService = service;
			_MessageReliabilityService = messageReliabilityService;
		}

		[Command("balance")]
		[Summary("Lets the user check their libcoin balance.")]
		public async Task Balance()
		{
			ulong guildId = Context.Guild.Id;
			ulong userId = Context.Message.Author.Id;
			if (_UserRecordsService.UserRecordExists(userId, guildId))
			{
				_ = Context.Channel.SendMessageAsync($"Looks like you have {_UserRecordsService.GetUserBalance(userId, guildId).ToString("F8")} libcoins.");
			}
			else
			{
				_ = Context.Channel.SendMessageAsync("Sorry, it looks like you don't have a balance yet! It'll happen eventually I'm sure.");
			}
		}

		[Command("balance")]
		[RequireOwner(Group = SharedConstants.AdminsOnlyGroup), RequireUserPermission(ChannelPermission.ManageMessages, Group = SharedConstants.AdminsOnlyGroup)]
		public async Task Balance([Summary("An @ ping of the user you're granting cash")] SocketGuildUser user)
		{
			ulong guildId = Context.Guild.Id;
			ulong userId = user.Id;
			if (_UserRecordsService.UserRecordExists(userId, guildId))
			{
				_ = Context.Channel.SendMessageAsync($"Looks like they have {_UserRecordsService.GetUserBalance(userId, guildId).ToString("F8")} libcoins.");
			}
			else
			{
				_ = Context.Channel.SendMessageAsync("Sorry, it looks like they don't have a balance yet! It'll happen eventually I'm sure.");
			}
		}

		[Command("top10")]
		public async Task Top10userPleaseMock()
		{
			_ = Context.Message.AddReactionAsync(Emote.Parse(SharedConstants.YouAreWhiteID));
			await Context.Channel.SendMessageAsync("https://dart.s-ul.eu/vKP4k4so");
		}

		[Command("leaderboard"), Alias("lb")]

		[Summary("Returns the LibCoin balances for this guild.")]
		public async Task GetGuildLibcoinLeaderboard()
		{
			int currentPage;
			List<UserRecord> balances = _UserRecordsService.GetPagedGuildBalances(Context.Guild.Id, out currentPage);

			if (balances.Count == 0)
			{
				_ = Context.Channel.SendMessageAsync("No one here has a balance. You're quick.");
			}
			else
			{
				new Thread(async () =>
				{
					await Context.Guild.DownloadUsersAsync();
					Embed embed = LibcoinUtilities.BuildLeaderboardEmbed(balances, currentPage, Context.Guild);
					IUserMessage msg = Context.Channel.SendMessageAsync(embed: embed).Result;
					msg.AddReactionAsync(new Emoji("⬅️"));
					msg.AddReactionAsync(new Emoji("➡️"));
				}).Start();
			}
		}

		[Command("active")]
		[Summary("Returns a list of all active users")]
		public async Task GetActiveUsers()
		{
			int currentPage;
			List<UserRecord> balances = _UserRecordsService.GetPagedActiveUserRecords(Context.Guild.Id, out currentPage);

			if (balances.Count == 0)
			{
				_ = Context.Channel.SendMessageAsync("No one here has a balance. You're quick.");
			}
			else
			{
				new Thread(async () =>
				{
					await Context.Guild.DownloadUsersAsync();
					Embed embed = LibcoinUtilities.BuildActiveUserEmbed(balances, currentPage, Context.Guild);
					IUserMessage msg = Context.Channel.SendMessageAsync(embed: embed).Result;
					msg.AddReactionAsync(new Emoji("⬅️"));
					msg.AddReactionAsync(new Emoji("➡️"));
				}).Start();
			}
		}

		[Command("stats")]
		[Summary("Returns economic stats for the guild")]
		public async Task GetGuildEconomicStats()
		{
			NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
			nfi.PercentDecimalDigits = 2;
			EmbedBuilder embedBuilder = new EmbedBuilder();
			embedBuilder.Title = $"{Context.Guild.Name}'s Economic Statistics";
			LibcoinEconomicStatistics stats = _UserRecordsService.CalculateEconomicStats(Context.Guild);
			IGuildUser poorestUser = Context.Guild.GetUserAsync(stats.PoorestUser, CacheMode.AllowDownload).Result;
			IGuildUser richestUser = Context.Guild.GetUserAsync(stats.RichestUser, CacheMode.AllowDownload).Result;
			double richestUserBalance = _UserRecordsService.GetUserBalance(richestUser.Id, Context.Guild.Id);

			embedBuilder.AddField("Total LibCoin Circulation", stats.TotalCirculation.ToString("F8"));
			embedBuilder.AddField("Mean LibCoin Balance", stats.MeanBalance.ToString("F8"));
			embedBuilder.AddField("Median LibCoin Balance", stats.MedianBalance.ToString("F8"));
			embedBuilder.AddField("Richest Active User", $"{DDBUtils.GetDisplayNameForUser(richestUser)} - {(richestUserBalance / stats.TotalCirculation).ToString("P", nfi)} of libcoin.");
			//Poor user might represent a significant smaller slice of the economy, so we're going DEEPER
			nfi.PercentDecimalDigits = 20;
			embedBuilder.AddField("Poorest Active User", DDBUtils.GetDisplayNameForUser(poorestUser));
			embedBuilder.AddField("GINI Coefficient", stats.GiniCoefficient.ToString("0.###"));

			_ = Context.Channel.SendMessageAsync(embed: embedBuilder.Build());
		}
		[Command("statsa")]
		[Summary("Returns economic stats for the active guild users")]
		public async Task GetGuildEconoicStats()
		{
			NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
			nfi.PercentDecimalDigits = 2;
			EmbedBuilder embedBuilder = new EmbedBuilder();
			embedBuilder.Title = $"{Context.Guild.Name}'s Economic Statistics";
			LibcoinEconomicStatistics stats = _UserRecordsService.CalculateActiveEconomicStats(Context.Guild);
			IGuildUser poorestUser = Context.Guild.GetUserAsync(stats.PoorestUser, CacheMode.AllowDownload).Result;
			IGuildUser richestUser = Context.Guild.GetUserAsync(stats.RichestUser, CacheMode.AllowDownload).Result;
			double richestUserBalance = _UserRecordsService.GetUserBalance(richestUser.Id, Context.Guild.Id);

			embedBuilder.AddField("Total LibCoin Circulation", stats.TotalCirculation.ToString("F8"));
			embedBuilder.AddField("Mean LibCoin Balance", stats.MeanBalance.ToString("F8"));
			embedBuilder.AddField("Median LibCoin Balance", stats.MedianBalance.ToString("F8"));
			embedBuilder.AddField("Richest Active User", $"{DDBUtils.GetDisplayNameForUser(richestUser)} - {(richestUserBalance / stats.TotalCirculation).ToString("P", nfi)} of libcoin.");
			//Poor user might represent a significant smaller slice of the economy, so we're going DEEPER
			nfi.PercentDecimalDigits = 20;
			embedBuilder.AddField("Poorest Active User", DDBUtils.GetDisplayNameForUser(poorestUser));
			embedBuilder.AddField("GINI Coefficient", stats.GiniCoefficient.ToString("0.###"));

			_ = Context.Channel.SendMessageAsync(embed: embedBuilder.Build());
		}

		[Command("grant")]
		[Summary("Conjures up the specified amount of libcoin and gives it to the specified user ID")]
		[RequireOwner(Group = SharedConstants.AdminsOnlyGroup), RequireUserPermission(ChannelPermission.ManageMessages, Group = SharedConstants.AdminsOnlyGroup)]
		public async Task GrantLibcoin([Summary("An @ ping of the user you're granting cash")] SocketGuildUser user, [Summary("The amount to grant to the user.")] double amount)
		{
			amount = Math.Abs(amount);
			if (user != null)
			{
				_UserRecordsService.Grant(user.Id, Context.Guild.Id, amount);
				_ = Context.Channel.SendMessageAsync($"Ok, I've given {DDBUtils.GetDisplayNameForUser(user)} {amount.ToString("F8")} libcoin.");
			}
		}
		[Command("grantall")]
		[Summary("Add the given sum to all users' balances in the guild.")]
		[RequireOwner(Group = SharedConstants.AdminsOnlyGroup), RequireUserPermission(ChannelPermission.ManageMessages, Group = SharedConstants.AdminsOnlyGroup)]
		public async Task GrantToAllGuildUsers([Summary("The amount to grant to all users of the guild.")] double amount)
		{
			foreach (UserRecord user in _UserRecordsService.GetGuildUserRecords(Context.Guild.Id))
			{
				_UserRecordsService.Grant(user.DiscordUserId, user.DiscordGuildId, Math.Abs(amount));
			}
		}

		[Command("deductall")]
		[Summary("Remove the given sum from all users' balances in the guild.")]
		[RequireOwner(Group = SharedConstants.AdminsOnlyGroup), RequireUserPermission(ChannelPermission.ManageMessages, Group = SharedConstants.AdminsOnlyGroup)]
		public async Task DeductFromAllGuildUsers([Summary("The amount to grant to all users of the guild.")] double amount)
		{
			foreach (UserRecord user in _UserRecordsService.GetGuildUserRecords(Context.Guild.Id))
			{
				_UserRecordsService.Deduct(user.DiscordUserId, user.DiscordGuildId, Math.Abs(amount));
			}
		}


		[RequireOwner(Group = SharedConstants.AdminsOnlyGroup)]
		[RequireUserPermission(ChannelPermission.ManageMessages, Group = SharedConstants.AdminsOnlyGroup)]
		[Command("deduct")]
		[Summary("Incinerates the specified amount of libcoin from the specified user ID. Overages will result in a balance of 0 currently.")]
		public async Task DeductLibcoin([Summary("An @ ping of the user you're granting cash")] SocketGuildUser user, [Summary("The amount to take from the user.")] double amount)
		{
			amount = Math.Abs(amount);
			if (user != null)
			{
				if (_UserRecordsService.Deduct(user.Id, Context.Guild.Id, amount))
				{
					_ = Context.Channel.SendMessageAsync($"Ok, I've taken  {amount.ToString("F8")} libcoin from {DDBUtils.GetDisplayNameForUser(user)}. If they didn't have that much they have nothing now.");
				}
				else
				{
					_ = Context.Channel.SendMessageAsync($"They don't have any money, I can't make them poorer than that.");
				}
			}
		}

		[RequireLibcoinBalance(0.00000001)]
		[Command("give")]
		[Summary("Gives the pinged user the specified amount of libcoin from your balance. Negative values will be absolute-valued")]
		public async Task UserGive([Summary("An @ ping of the user you're granting cash")] SocketGuildUser receivingUser, [Summary("The amount to take from the user.")] double amount, string transactionType = "")
		{
			amount = Math.Abs(amount);
			if (transactionType == "d" || transactionType == "dartoshi" || transactionType == "dartoshis")
			{
				amount = 0.00000001 * amount;
			}
			ulong senderId = Context.Message.Author.Id;
			ulong guildId = Context.Guild.Id;
			double senderBalance = _UserRecordsService.GetUserBalance(senderId, guildId);
			if (amount <= senderBalance)
			{
				_UserRecordsService.Deduct(senderId, guildId, amount);
				_UserRecordsService.Grant(receivingUser.Id, guildId, amount);
				_ = Context.Channel.SendMessageAsync($"Ok, {Context.Message.Author.Mention}. I've given {DDBUtils.GetDisplayNameForUser(receivingUser)} {amount.ToString("F8")} of your libcoins.");
			}
			else
			{
				_ = Context.Channel.SendMessageAsync($"Listen friend, I hate to embarass you like this in front of all your friends... but you don't actually _have_ {amount.ToString("F8")} libcoins.");
			}
		}

		[Command("dartoshis"), Alias("dartoshi")]
		[Summary("Gives the user their balance in Dartoshis (0.00000001 libcoin is 1 Dartoshi).")]
		public async Task GetDartoshis()
		{
			ulong guildId = Context.Guild.Id;
			ulong userId = Context.Message.Author.Id;
			if (_UserRecordsService.UserRecordExists(userId, guildId))
			{
				double dartoshis = _UserRecordsService.GetUserBalance(userId, guildId) * DartoshiConstant;
				await Context.Channel.SendMessageAsync($"Looks like you have {dartoshis.ToString("F0")} Dartoshis.");
			}
			else
			{
				_ = Context.Channel.SendMessageAsync("Sorry, it looks like you don't have a balance yet! It'll happen eventually I'm sure.");
			}
		}

		[Command("marx"), Alias("readtheory")]
		[Summary("Shows you what the distribution would look like for a >communism <amount> command.")]
		public async Task MarxistTheory([Summary("The amount to distribute")] double amount, [Summary("Maximum amount any one user should get")] double maxDistribution = 0)
		{
			List<UserRecord> activeUsers = _UserRecordsService.GetActiveUserRecords(Context.Guild.Id);
			UserRecord triggeringUserRecord = activeUsers.FirstOrDefault(u => u.DiscordUserId == Context.Message.Author.Id);
			if (triggeringUserRecord != null)
			{
				activeUsers.Remove(triggeringUserRecord);
			}
			new Thread(async () =>
			{
				await Context.Guild.DownloadUsersAsync();
				List<UserProgressiveShare> shares = _UserRecordsService.CalculateProgressiveShare(activeUsers, amount, maxDistribution);
				StringBuilder sb = new("Ok, here's how I would distribute that amount for you, in theory anyway...");
				sb.AppendLine();
				foreach (UserProgressiveShare share in shares.OrderByDescending(s => s.ProgressiveShare))
				{
					IGuildUser user = Context.Guild.GetUserAsync(share.User.DiscordUserId).Result;
					sb.AppendLine($"**{DDBUtils.GetDisplayNameForUser(user)}** would get **{share.ProgressiveShare}** libcoin.");
				}
				sb.AppendLine().AppendLine($"Total sum distributed: {shares.Sum(s => s.ProgressiveShare)}");
				_MessageReliabilityService.SendMessageToChannel(sb.ToString(), Context.Message, Environment.NewLine);
			}).Start();
		}

		[Command("communism"), Alias("therevolution")]
		[Summary("Shows you what the distribution would look like for a >communism <amount> command.")]
		public async Task MarxistRevolution([Summary("The amount to distribute")] double amount, [Summary("Maximum amount any one user should get")] double maxDistribution = 0)
		{			
			ulong senderId = Context.Message.Author.Id;
			ulong guildId = Context.Guild.Id;
			double senderBalance = _UserRecordsService.GetUserBalance(senderId, guildId);
			if (senderBalance < amount)
			{
				await Context.Channel.SendMessageAsync($"Sorry, you're too poor to give out that much money, Karl. You only have {senderBalance} libcoin, how the hell would you give out {amount} libcoin?");
				return;
			}
			if(amount < 1)
			{
				await Context.Channel.SendMessageAsync($"Nuh-uh. I don't get out of the bed in the morning for at least 1 Libcoin, ya feel me? Get this weak 'Hey Deepstate, can you send {amount} lilbcoin distributed among the active users?' bullshit outta here.");
				return;
			}
			List<UserRecord> activeUsers = _UserRecordsService.GetActiveUserRecords(guildId);
			UserRecord triggeringUserRecord = activeUsers.FirstOrDefault(u => u.DiscordUserId == Context.Message.Author.Id);
			if (triggeringUserRecord != null)
			{
				activeUsers.Remove(triggeringUserRecord);
			}
			new Thread(async () =>
			{
				await Context.Guild.DownloadUsersAsync();
				List<UserProgressiveShare> shares = _UserRecordsService.CalculateProgressiveShare(activeUsers, amount, maxDistribution);
				StringBuilder sb = new("Ok, here's how I would distribute that amount for you, in theory anyway...");
				sb.AppendLine();
				foreach (UserProgressiveShare share in shares.OrderByDescending(s => s.ProgressiveShare))
				{
					IGuildUser user = Context.Guild.GetUserAsync(share.User.DiscordUserId).Result;
					_UserRecordsService.Deduct(senderId, guildId, share.ProgressiveShare);
					_UserRecordsService.Grant(share.User.DiscordUserId, guildId, share.ProgressiveShare);
					sb.AppendLine($"**{DDBUtils.GetDisplayNameForUser(user)}** got **{share.ProgressiveShare}** libcoin.");
				}
				sb.AppendLine().AppendLine($"Total sum distributed: {shares.Sum(s => s.ProgressiveShare)}");
				_MessageReliabilityService.SendMessageToChannel(sb.ToString(), Context.Message, Environment.NewLine);
			}).Start();
		}
	}
}
