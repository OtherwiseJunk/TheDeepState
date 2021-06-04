using DartsDiscordBots.Services.Interfaces;
using DeepState.Data.Constants;
using DeepState.Data.Models;
using DeepState.Data.Services;
using DeepState.Modules.Preconditions;
using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DeepState.Modules
{
	[Group("hungergames"), Alias("hg")]
	public class HungerGamesModule : ModuleBase
	{
		public HungerGamesService _service { get; set; }
		public IMessageReliabilityService _messenger { get; set; }

		public HungerGamesModule(HungerGamesService service, IMessageReliabilityService messenger)
		{
			_service = service;
			_messenger = messenger;
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
				await Context.Channel.SendMessageAsync($"Gosh you're brave. Ok! I've registered you as a Tribute in this month's ⛈️ **T H U N D E R D O M E** ⛈️, and deducted {HungerGameConstants.CostOfAdmission.ToString("F8")} libcoins from your account. Good luck! {Environment.NewLine} https://media1.tenor.com/images/f9da8dd0e06d31730afb9ad12abed53c/tenor.gif?itemid=17203535");
			}
		}

		[Command("tributes"), Alias("walkingcorpses")]
		[Summary("Returns the list of registered tributes for this server.")]
		public void GetTributeList()
		{
			List<HungerGamesTributes> tributes = _service.GetTributeList(Context.Guild.Id);
			
			

			if(tributes.Count == 0)
			{
				_ = Context.Channel.SendMessageAsync("This is a channel of COWARDS because no one has signed up as tribute for the games!");
			}
			else
			{
				new Thread(() => {
					List<string> tributeNames = new List<string>();
					foreach (HungerGamesTributes tribute in tributes)
					{
						IGuildUser user = Context.Guild.GetUserAsync(tribute.DiscordUserId).Result;
						tributeNames.Add(user.Nickname ?? user.Username);
					}

					MessageReference reference = Context.Message.Reference ?? new MessageReference(Context.Message.Id);
					_ = _messenger.SendMessageToChannel(String.Join(',', tributeNames), Context.Channel, reference, new List<ulong>(Context.Message.MentionedUserIds), ",");
				}).Start();
			}
		}
	}
}
