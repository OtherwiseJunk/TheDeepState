﻿using DartsDiscordBots.Permissions;
using DartsDiscordBots.Services.Interfaces;
using DeepState.Data.Constants;
using DeepState.Data.Models;
using DeepState.Data.Services;
using DeepState.Modules.Preconditions;
using DeepState.Utilities;
using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
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
			IRole tributeRole = Context.Guild.Roles.FirstOrDefault(r => r.Name == HungerGameConstants.TributeRoleName);
			if (_service.TributeExists(Context.Guild.Id, Context.User.Id))
			{
				await Context.Channel.SendMessageAsync("Sorry, you're already registered for this month's game!");
			}
			else
			{
				_service.RegisterTribute(Context.Guild.Id, Context.User.Id);
				await Context.Channel.SendMessageAsync($"Gosh you're brave. Ok! I've registered you as a Tribute in this month's ⛈️ **T H U N D E R D O M E** ⛈️, and deducted {HungerGameConstants.CostOfAdmission.ToString("F8")} libcoins from your account. Good luck! {Environment.NewLine} https://media1.tenor.com/images/f9da8dd0e06d31730afb9ad12abed53c/tenor.gif?itemid=17203535");
				if(tributeRole != null)
				{
					_ = ((IGuildUser)Context.User).AddRoleAsync(tributeRole);
				}
			}
		}

		[Command("tributes"), Alias("walkingcorpses", "walkingdead")]
		[Summary("Returns the list of registered tributes for this server.")]
		public async Task GetTributeList()
		{
			int currentPage;
			List<HungerGamesTributes> tributes = _service.GetPagedTributeList(Context.Guild.Id, out currentPage);

			if(tributes.Count == 0)
			{
				_ = Context.Channel.SendMessageAsync("This is a channel of COWARDS because no one has signed up as tribute for the games!");
			}
			else
			{
				new Thread(() => {
					Embed embed = HungerGameUtilities.BuildTributeEmbed(tributes, currentPage, Context.Guild); 
					IUserMessage msg = Context.Channel.SendMessageAsync(embed: embed).Result;
					msg.AddReactionAsync(new Emoji("⬅️"));
					msg.AddReactionAsync(new Emoji("➡️"));					
				}).Start();
			}
		}

		[Command("roleup")]
		[RequireOwner, RequireRoleName(HungerGameConstants.TributeRoleName)]
		public async Task AssignTributeRoles()
		{
			IRole tributeRole = Context.Guild.Roles.First(r => r.Name == HungerGameConstants.TributeRoleName);
			List<HungerGamesTributes> tributes = _service.GetTributeList(Context.Guild.Id);
			foreach(HungerGamesTributes tribute in tributes)
			{
				IGuildUser user = Context.Guild.GetUserAsync(tribute.DiscordUserId).Result;
				_ = user.AddRoleAsync(tributeRole);
			}
		}
	}
}
