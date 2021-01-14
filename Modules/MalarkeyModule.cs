using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TheDeepState.Modules
{
	public class MalarkeyModule : ModuleBase
	{
		[Command("clap")]
		[Summary("Places a 👏 emoji in place of any spaces. Will delete the original message, but will include the triggering user's username.")]
		public async Task Clap([Summary("The message to Clapify."), Remainder] string msg)
		{
			string user = (Context.Message.Author as IGuildUser).Nickname ?? Context.Message.Author.Username;

			await Context.Message.DeleteAsync();
			await Context.Channel.SendMessageAsync($"**{user}**:👏 {string.Join(" 👏 ", msg.Split(' '))} 👏");
		}
	}
}