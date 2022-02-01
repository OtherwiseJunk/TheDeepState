using DartsDiscordBots.Permissions;
using DartsDiscordBots.Services.Interfaces;
using DeepState.Constants;
using HGC = DeepState.Data.Constants.HungerGameConstants;
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
using DDBUtils = DartsDiscordBots.Utilities.BotUtilities;
using DeepState.Data.Constants;

namespace DeepState.Modules
{
	[Group("hungergames"), Alias("hg", "hungry")]
	public class HungerGamesModule : ModuleBase
	{
		public HungerGamesService _hgService { get; set; }
		public UserRecordsService _urService { get; set; }
		public IMessageReliabilityService _messenger { get; set; }

		public HungerGamesModule(HungerGamesService service, IMessageReliabilityService messenger, UserRecordsService urService)
		{
			_hgService = service;
			_urService = urService;
			_messenger = messenger;
		}

		[Command("register"), Alias("reg", "greg")]
		[RequireLibcoinBalance(HGC.CostOfAdmission)]
		[RequireDayOfMonthRange(1, 3, Group = SharedConstants.HungerGamesRegistrationDateGroup), RequireDayOfMonthRange(11, 13, Group = SharedConstants.HungerGamesRegistrationDateGroup), RequireDayOfMonthRange(21, 23, Group = SharedConstants.HungerGamesRegistrationDateGroup)]
		public async Task RegisterHungerGameTribute()
		{
			if (Context.Guild.Id == SharedConstants.LibcraftGuildId)
			{
				if (Context.Channel.Id == SharedConstants.LCBotCommandsChannel)
				{
					await RegisterTribute();
				}
				if(Context.Channel.Id == SharedConstants.LCShitpostChannelId)
				{
					await Context.Channel.SendMessageAsync("This user was attempting to use the command where it's not allowed. Everyone Laugh.", messageReference: Context.Message.Reference);
				}
				else
				{
					await Context.Message.AddReactionAsync(Emote.Parse(SharedConstants.BooHooCrackerID));
				}
			}
			else
			{
				await RegisterTribute();
			}
		}

		public async Task RegisterTribute()
		{
			IRole tributeRole = Context.Guild.Roles.FirstOrDefault(r => r.Name.ToLower() == HGC.TributeRoleName.ToLower());
			if (SharedConstants.KnownSocks.Contains(Context.Message.Author.Id))
			{
				await Context.Channel.SendMessageAsync("Sorry, I'm prejudiced against SockAccounts. They creep me out.");
			}
			else if (_hgService.TributeExists(Context.Guild.Id, Context.User.Id))
			{
				await Context.Channel.SendMessageAsync("Sorry, you're already registered for this month's game!");
			}
			else
			{
				_hgService.RegisterTribute(Context.Guild.Id, Context.User.Id);
				string imageUrl;
				if (Utils.PercentileCheck(10))
				{
					imageUrl = "https://media3.giphy.com/media/2Yd3r2GLy9bXakxzqC/giphy.gif";
				}
				else
				{
					imageUrl = "https://media1.tenor.com/images/f9da8dd0e06d31730afb9ad12abed53c/tenor.gif?itemid=17203535";
				}
				await Context.Channel.SendMessageAsync($"Gosh you're brave. Ok! I've registered you as a Tribute in this month's ⛈️ **T H U N D E R D O M E** ⛈️, and deducted {HGC.CostOfAdmission.ToString("F8")} libcoins from your account. Good luck! {Environment.NewLine} {imageUrl}");
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
			List<HungerGamesTribute> tributes = _hgService.GetPagedTributeList(Context.Guild.Id, out currentPage);

			if (tributes.Count == 0)
			{
				_ = Context.Channel.SendMessageAsync("This is a channel of COWARDS because no one has signed up as tribute for the games!");
			}
			else
			{
				new Thread(() =>
				{
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
			if (_hgService.PrizePoolExists(Context.Guild.Id))
			{
				await Context.Channel.SendMessageAsync($"Looks like there's a grand total of {_hgService.GetPrizePool(Context.Guild.Id).ToString("F8")} on the line!");
			}
		}

		[Command("readycheck"), Alias("rc")]
		[RequireUserPermission(ChannelPermission.ManageMessages)]
		public async Task ReadyCheck()
		{
			bool tributeRoleExists = RoleExists(HGC.TributeRoleName, Context);
			bool championRoleExists = RoleExists(HGC.ChampionRoleName, Context);
			bool corpseRoleExists = RoleExists(HGC.CorpseRoleName, Context);

			await Context.Channel.SendMessageAsync(@$"Tribute Role Exists? {tributeRoleExists}
Champion Role Exists? {championRoleExists}
Corpse Role Exists? {corpseRoleExists}");
		}

		public bool RoleExists(string RoleName, ICommandContext context)
		{
			IRole role = context.Guild.Roles.FirstOrDefault(r => r.Name.ToLower() == RoleName.ToLower());
			if (role != null)
			{
				return true;
			}
			return false;
		}

		[Command("roleup")]
		[RequireOwner]
		public async Task AssignTributeRoles()
		{
			IRole tributeRole = Context.Guild.Roles.First(r => r.Name.ToLower() == HGC.TributeRoleName.ToLower());
			List<HungerGamesTribute> tributes = _hgService.GetTributeList(Context.Guild.Id);
			foreach (HungerGamesTribute tribute in tributes)
			{
				IGuildUser user = Context.Guild.GetUserAsync(tribute.DiscordUserId, CacheMode.AllowDownload).Result;
				if (!user.RoleIds.Contains(tributeRole.Id) && tribute.IsAlive)
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

		[Command("tributechannel")]
		[RequireRoleName(HGC.TributeRoleName)]
		[RequireUserPermission(GuildPermission.ManageMessages)]
		public async Task SetTributeAnnouncementChannel()
		{
			if (_hgService.TributeAnnouncementConfigurationExists(Context.Guild.Id))
			{
				await Context.Channel.SendMessageAsync("Someone already registered a channel for this, I'll overwrite it and use this channel instead.");
			}

			_hgService.SetTributeAnnouncementChannel(Context.Guild.Id, Context.Channel.Id);
			await Context.Channel.SendMessageAsync("Ok, i'll do my daily obituaries here, starting on the 8th of every month!");
		}

		[Command("corpsechannel")]
		[RequireRoleName(HGC.CorpseRoleName)]
		[RequireUserPermission(GuildPermission.ManageMessages)]
		public async Task SetCorpseAnnouncementChannel()
		{
			if (_hgService.CorpseAnnouncementConfigurationExists(Context.Guild.Id))
			{
				await Context.Channel.SendMessageAsync("Someone already registered a channel for this, I'll overwrite it and use this channel instead.");
			}

			_hgService.SetCorpseAnnouncementChannel(Context.Guild.Id, Context.Channel.Id);
			await Context.Channel.SendMessageAsync("Ok, i'll do my daily obituaries here, starting on the 8th of every month!");
		}

		[Command("testfrag")]
		[RequireRoleName(HGC.TributeRoleName)]
		[RequireUserPermission(GuildPermission.ManageMessages, Group = SharedConstants.AdminsOnlyGroup)]
		[RequireOwner(Group = SharedConstants.AdminsOnlyGroup)]
		public async Task TestFrag(ulong mentionedUser = 0)
		{
			Random rand = Utils.CreateSeededRandom();
			IGuildUser victim;
			ulong victimDiscordId;
			if (mentionedUser != 0)
			{
				victimDiscordId = mentionedUser;
			}
			else
			{
				victimDiscordId = Context.Client.CurrentUser.Id;
			}
			victim = Context.Guild.GetUserAsync(victimDiscordId, CacheMode.AllowDownload).Result;
			string victimName = DDBUtils.GetDisplayNameForUser(victim);
			var pronounDict = Utils.GetUserPronouns(victim, Context.Guild);
			List<HungerGamesTribute> tributes = _hgService.GetTributeList(Context.Guild.Id);
			string goreyDetails = HungerGameUtilities.GetCauseOfDeathDescription(victimDiscordId, victimName, Context.Guild, tributes, pronounDict);
			string obituary = HungerGameUtilities.GetObituary(pronounDict, victim);
			_ = Context.Channel.SendMessageAsync(embed: HungerGameUtilities.BuildTributeDeathEmbed(victim, goreyDetails, obituary, rand.Next(1, 12)));
		}

		[Command("1984"), Alias("avadakedavra")]
		[RequireOwner()]
		public async Task removeHGRoles()
		{
			

			foreach (HungerGamesServerConfiguration config in _hgService.GetAllConfigurations())
			{
				IGuild guild = Context.Client.GetGuildAsync(config.DiscordGuildId).Result;
				IRole tributeRole = guild.Roles.First(r => r.Name.ToLower() == HungerGameConstants.TributeRoleName.ToLower());
				IRole corpseRole = guild.Roles.FirstOrDefault(r => r.Name.ToLower() == HungerGameConstants.CorpseRoleName.ToLower());
				foreach ( IGuildUser user in guild.GetUsersAsync().Result)
				{
					try
					{
						await user.RemoveRoleAsync(tributeRole, new RequestOptions { AuditLogReason = "Junk fucked up yanno? Gotta cover their back." });
					}
					catch
					{
						Console.WriteLine("They didn't have that role whoops");
					}
					try
					{
						await user.RemoveRoleAsync(corpseRole, new RequestOptions { AuditLogReason = "Junk fucked up yanno? Gotta cover their back." });
					}
					catch
					{
						Console.WriteLine("They didn't have that role whoops");
					}
				}
			}
		}
		[Command("testRun")]
		[RequireOwner()]
		public async Task testRun()
		{
			HungerGameUtilities.DailyEvent(_hgService, null, Context.Client);
		}

		[Command("cleanup"), Alias("wrap")]
		[RequireUserPermission(ChannelPermission.ManageMessages)]
		public async Task runCleanup()
		{
			HungerGamesServerConfiguration config = _hgService.GetAllConfigurations().FirstOrDefault(c => c.DiscordGuildId == Context.Guild.Id);
			IRole tributeRole = Context.Guild.Roles.FirstOrDefault(r => r.Name.ToLower() == HungerGameConstants.TributeRoleName.ToLower());
			IRole corpseRole = Context.Guild.Roles.FirstOrDefault(r => r.Name.ToLower() == HungerGameConstants.CorpseRoleName.ToLower());
			IRole championRole = Context.Guild.Roles.FirstOrDefault(r => r.Name.ToLower() == HungerGameConstants.ChampionRoleName.ToLower());
			IMessageChannel tributeAnnouncementChannel = (IMessageChannel)await Context.Guild.GetChannelAsync(config.TributeAnnouncementChannelId);
			Console.WriteLine(@$"Tribute Role Null? {tributeRole == null}");
			Console.WriteLine(@$"Corpse Role Null? {corpseRole == null}");
			Console.WriteLine(@$"Champion Role Null? {championRole == null}");
			if (config != null)
			{
				HungerGameUtilities.RunHungerGamesCleanup(Context.Guild, tributeAnnouncementChannel, tributeRole, corpseRole, championRole, _hgService.GetTributeList(Context.Guild.Id), _hgService, _urService);
			}

		}



		[Command("announce"), Alias("anc")]
		[RequireOwner()]
		public async Task MakeAnnouncement([Remainder] string announcement)
		{
			foreach (HungerGamesServerConfiguration config in _hgService.GetAllConfigurations())
			{
				IMessageChannel announcementChannel = (IMessageChannel)await Context.Guild.GetChannelAsync(config.TributeAnnouncementChannelId);
				if (announcementChannel != null)
				{
					await announcementChannel.SendMessageAsync(announcement);
				}
			}
		}
	}
}
