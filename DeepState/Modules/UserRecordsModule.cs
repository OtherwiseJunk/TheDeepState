using DeepState.Data.Models;
using DeepState.Data.Services;
using Discord;
using Discord.Commands;
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
	}
}
