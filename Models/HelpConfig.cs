using DartsDiscordBots.Modules.Help.Interfaces;
using TheDeepState.Constants;

namespace TheDeepState.Models
{
	public class HelpConfig : IHelpConfig
	{
		public string Prefix { get; set; } = BotProperties.CommandPrefix.ToString();
	}
}
