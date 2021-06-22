using DeepState.Data.Models;
using DeepState.Data.Services;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeepState.Modules
{
	public class UserRecordsModule : ModuleBase
	{
		private UserRecordsService _UserRecordsService { get; set; }
		public UserRecordsModule(UserRecordsService service)
		{
			_UserRecordsService = service;
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
		[RequireOwner(Group = "AdminOnly"),RequireUserPermission(ChannelPermission.ManageMessages,Group = "AdminOnly")]
		public async Task Balance(SocketGuildUser user)
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

		[Command("leaderboard"), Alias("top10")]
		[Summary("Returns the top 10 LibCoin balances for this guild.")]
		public async Task GetGuildLibcoinLeaderboard()
		{
			EmbedBuilder embedBuilder = new EmbedBuilder();
			List<UserRecord> topBalances = _UserRecordsService.GetGuildTopTenBalances(Context.Guild.Id);
			embedBuilder.Title = $"Top {topBalances.Count} LibCoin Balances";
			int place = 1;
			foreach (UserRecord record in topBalances)
			{
				IGuildUser user = Context.Guild.GetUserAsync(record.DiscordUserId).Result;
				string username = user.Nickname ?? user.Username;
				embedBuilder.AddField($"{place}. {username}", $"{record.LibcraftCoinBalance.ToString("F8")}");
				place++;
			}
			_ = Context.Channel.SendMessageAsync(embed: embedBuilder.Build());
		}

		[RequireOwner(Group = "AllowedUsers")]
		[RequireUserPermission(ChannelPermission.ManageMessages, Group = "AllowedUsers")]
		[Command("stats")]
		[Summary("Returns economic stats for the guild")]
		public async Task GetGuildEconomicStats()
		{
			EmbedBuilder embedBuilder = new EmbedBuilder();
			embedBuilder.Title = $"{Context.Guild.Name}'s Economic Statistics";
			LibcoinEconomicStatistics stats = _UserRecordsService.CalculateEconomicStats(Context.Guild.Id);
			IGuildUser poorestUser = Context.Guild.GetUserAsync(stats.PoorestUser).Result;
			IGuildUser richestUser = Context.Guild.GetUserAsync(stats.RichestUser).Result;

			embedBuilder.AddField("Total LibCoin Circulation", stats.TotalCirculation.ToString("F8"));
			embedBuilder.AddField("Mean LibCoin Balance", stats.MeanBalance.ToString("F8"));
			embedBuilder.AddField("Median LibCoin Balance", stats.MedianBalance.ToString("F8"));
			embedBuilder.AddField("Richest User", richestUser.Nickname ?? richestUser.Username);
			embedBuilder.AddField("Poorest User", poorestUser.Nickname ?? poorestUser.Username);
			embedBuilder.AddField("GINI Coefficient", stats.GiniCoefficient.ToString("0.###"));

			_ = Context.Channel.SendMessageAsync(embed: embedBuilder.Build());
		}

		[RequireOwner(Group = "AdminsOnly")]
		[RequireUserPermission(ChannelPermission.ManageMessages, Group = "AdminsOnly")]
		[Command("grant")]
		[Summary("Conjures up the specified amount of libcoin and gives it to the specified user ID")]
		public async Task GrantLibcoin(ulong discordUserID, double amount)
		{
			IGuildUser user = Context.Guild.GetUserAsync(discordUserID).Result;
			if (user != null)
			{
				_UserRecordsService.Grant(user.Id, Context.Guild.Id, amount);
				_ = Context.Channel.SendMessageAsync($"Ok, I've given {user.Nickname ?? user.Username} {amount.ToString("F8")} libcoin.");
			}
		}

		[RequireOwner(Group = "AdminsOnly")]
		[RequireUserPermission(ChannelPermission.ManageMessages, Group = "AdminsOnly")]
		[Command("deduct")]
		[Summary("Incinerates the specified amount of libcoin from the specified user ID. Overages will result in a balance of 0 currently.")]
		public async Task DeductLibcoin(ulong discordUserID, double amount)
		{
			IGuildUser user = Context.Guild.GetUserAsync(discordUserID).Result;
			if (user != null)
			{
				if (_UserRecordsService.Deduct(user.Id, Context.Guild.Id, amount)) {
					_ = Context.Channel.SendMessageAsync($"Ok, I've taken  {amount.ToString("F8")} libcoin from {user.Nickname ?? user.Username}. If they didn't have that much they have nothing now.");
				}
				else
				{
					_ = Context.Channel.SendMessageAsync($"They don't have any money, I can't make them poorer than that.");
				}
			}
		}
	}
}
