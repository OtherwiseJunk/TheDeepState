using DeepState.Data.Services;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
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
	}
}
