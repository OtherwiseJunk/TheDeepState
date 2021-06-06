using DartsDiscordBots.Permissions;
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
using Utils = DeepState.Utilities.Utilities;

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
		[RequireDayOfMonthRange(1, 7)]
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
				if (tributeRole != null)
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
			List<HungerGamesTribute> tributes = _service.GetPagedTributeList(Context.Guild.Id, out currentPage);

			if (tributes.Count == 0)
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

		[Command("pot")]
		public async Task GetGuildPot()
		{
			if (_service.PrizePoolExists(Context.Guild.Id))
			{
				_ = Context.Channel.SendMessageAsync($"Looks like there's a grand total of {_service.GetPrizePool(Context.Guild.Id).ToString("F8")} on the line!");
			}
		}

		[Command("roleup")]
		[RequireOwner]
		public async Task AssignTributeRoles()
		{
			IRole tributeRole = Context.Guild.Roles.First(r => r.Name == HungerGameConstants.TributeRoleName);
			List<HungerGamesTribute> tributes = _service.GetTributeList(Context.Guild.Id);
			foreach (HungerGamesTribute tribute in tributes)
			{
				IGuildUser user = Context.Guild.GetUserAsync(tribute.DiscordUserId).Result;
				if (!user.RoleIds.Contains(tributeRole.Id))
				{
					var obj = user.AddRoleAsync(tributeRole);
					while (!obj.IsCompleted) { }
					if (obj.Exception != null)
					{
						Console.WriteLine($"Adding Tribute Role failed for {user.Username}. Exception: {obj.Exception.Message}");
					}
					else
					{
						Console.WriteLine($"Added Tribute Role to {user.Username}");
					}
				}
			}
		}

		[Command("setchnl")]
		[RequireUserPermission(GuildPermission.ManageMessages)]
		public async Task SetAnnouncementChannel()
		{
			if (_service.AnnouncementConfigurationExists(Context.Guild.Id))
			{
				await Context.Channel.SendMessageAsync("Someone already registered a channel for this, I'll overwrite it and use this channel instead.");
			}

			_service.SetAnnouncementChannel(Context.Guild.Id, Context.Channel.Id);
			await Context.Channel.SendMessageAsync("Ok, i'll do my daily obituaries here, starting on the 8th of every month!");
		}

		[Command("testfrag")]
		[RequireUserPermission(GuildPermission.ManageMessages)]
		public async Task TestFrag(ulong mentionedUser = 0)
		{
			Random rand = Utils.CreateSeededRandom();
			IGuildUser victim;
			if (mentionedUser != 0)
			{
				IGuildUser user = Context.Guild.GetUserAsync(mentionedUser).Result;
				victim = user;
			}
			else
			{
				IGuildUser user = Context.Guild.GetUserAsync(Context.Client.CurrentUser.Id).Result;;
				victim = user;
			}
			var pronounDict = Utils.GetUserPronouns(victim, Context.Guild);
			List<HungerGamesTribute> tributes = _service.GetTributeList(Context.Guild.Id);
			string goreyDetails = HungerGameUtilities.GetCauseOfDeathDescription(victim, Context.Guild, tributes, pronounDict);
			string obituary = HungerGameUtilities.GetObituary(pronounDict, victim);
			_ = Context.Channel.SendMessageAsync(embed: HungerGameUtilities.BuildTributeDeathEmbed(victim, goreyDetails, obituary, rand.Next(1, 12)));
		}
	}
}
