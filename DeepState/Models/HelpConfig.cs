using DartsDiscordBots.Modules.Help.Interfaces;
using DeepState.Constants;

namespace DeepState.Models
{
	public class HelpConfig : IHelpConfig
	{
		public string Prefix { get; set; } = BotProperties.CommandPrefix.ToString();
	}
}
