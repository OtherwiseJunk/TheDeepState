using DartsDiscordBots.Modules.Bot.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheDeepState.Models
{
	public class BotInformation : IBotInformation
	{
		public string InstallationLink { get; set; } = "https://discord.com/api/oauth2/authorize?client_id=799039246668398633&permissions=70544448&scope=bot";
		public string GithubRepo { get; set; } = "https://github.com/OtherwiseJunk/TheDeepState";
	}
}
