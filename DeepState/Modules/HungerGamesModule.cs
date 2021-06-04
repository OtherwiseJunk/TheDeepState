using DeepState.Data.Constants;
using DeepState.Data.Services;
using DeepState.Modules.Preconditions;
using Discord.Commands;
using System.Threading.Tasks;

namespace DeepState.Modules
{
	[Group("hungergames"), Alias("hg")]
	public class HungerGamesModule : ModuleBase
	{
		public HungerGamesService _service { get; set; }

		public HungerGamesModule(HungerGamesService service)
		{
			_service = service;
		}

		[Command("register"), Alias("reg")]
		[RequireLibcoinBalance(HungerGameConstants.CostOfAdmission)]
		[RequireDayOfMonthRange(1,7)]
		public async Task RegisterHungerGameTribute()
		{
			if(_service.TributeExists(Context.Guild.Id, Context.User.Id))
			{
				await Context.Channel.SendMessageAsync("Sorry, you're already registered for this month's game!");
			}
			else
			{
				_service.RegisterTribute(Context.Guild.Id, Context.User.Id);
				await Context.Channel.SendMessageAsync($"Gosh you're brave. Ok! I've registered you as a Tribute in this month's Hunger Games, and deducted {HungerGameConstants.CostOfAdmission} libcoins from your account. Good luck!");
			}
		}
	}
}
