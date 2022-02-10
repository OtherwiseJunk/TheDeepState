using DartsDiscordBots.Utilities;
using DeepState.Data.Constants;
using DeepState.Data.Models;
using DeepState.Data.Services;
using DeepState.Models;
using DeepState.Modules.Preconditions;
using DeepState.Service;
using Discord;
using Discord.Commands;
using SkiaSharp;
using Svg;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Modules
{
	[Group("rpg"), Name("RPG Module")]
	public class RPGModule : ModuleBase
	{
		public RPGService _rpgService { get; set; }
		public UserRecordsService _userService { get; set; }
		public ImagingService _imagingService { get; set; }
		public RPGModule(RPGService rpgService, UserRecordsService userService, ImagingService imagingService)
		{
			_rpgService = rpgService;
			_userService = userService;
			_imagingService = imagingService;
		}

		[Command("newchar"), Alias("nc")]
		[RequireLibcoinBalance(RPGConstants.NewCharacterCost)]
		public async Task GenerateNewCharacter(string flags = "")
		{
			Character character = _rpgService.GetCharacter((IGuildUser)Context.User);
			if (character == null)
			{
				try
				{
					using (WebClient wc = new WebClient())
					{
						string guidId = Guid.NewGuid().ToString();
						string svgFile = $"{guidId}.svg";
						wc.DownloadFile($"https://avatars.dicebear.com/api/avataaars/{guidId}.svg", svgFile);
						var svgDoc = SvgDocument.Open<SvgDocument>(svgFile, null);
						Stream stream = new MemoryStream();
						svgDoc.Draw().Save(stream, System.Drawing.Imaging.ImageFormat.Png);


						string avatarUrl = _imagingService.UploadImage(RPGConstants.AvatarFolder, stream);
						character = _rpgService.CreateNewCharacter((IGuildUser)Context.User, avatarUrl);
						_userService.Deduct(Context.User.Id, Context.Guild.Id, RPGConstants.NewCharacterCost);
						if (flags == "f" || flags == "fite")
						{
							await Context.Channel.SendMessageAsync("Today your character woke up and chose _violence_.");
							_rpgService.ToggleCharacterPvPFlag((IGuildUser)Context.Message.Author);
						}
						await Context.Channel.SendMessageAsync($"Ok I rolled you up a new character! {RPGConstants.NewCharacterCost} Libcoin has been deducted from your account.", embed: _rpgService.BuildCharacterEmbed(character));
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("Shit broke");
					Console.WriteLine(ex.Message);
				}
			}
			else
			{
				await Context.Channel.SendMessageAsync("Looks like you already have a character.", embed: _rpgService.BuildCharacterEmbed(character));
			}
		}

		[Command("char"), Alias("c")]
		public async Task RetrieveCharacter()
		{
			Character character = _rpgService.GetCharacter((IGuildUser)Context.User);
			if (character == null)
			{
				await Context.Channel.SendMessageAsync($"Sorry friend, you haven't rolled up a character yet. It costs {RPGConstants.NewCharacterCost} Libcoin but it's worth it!");
			}
			else
			{
				await Context.Channel.SendMessageAsync(embed: _rpgService.BuildCharacterEmbed(character));
			}
		}

		[Command("pvp")]
		public async Task TogglePvPStatus()
		{
			_rpgService.ToggleCharacterPvPFlag((IGuildUser) Context.User);
			Character character = _rpgService.GetCharacter((IGuildUser)Context.User);
			if (character.PvPFlagged)
			{
				await Context.Channel.SendMessageAsync($"Ok, I'll let everyone know {character.Name} is ready to rumble!");
			}
			else
			{
				await Context.Channel.SendMessageAsync($"Ok, I'll let everyone know {character.Name} is character of peace. For now.");
			}
		}

		[Command("pvplist"), Alias("saturdaynight","goodfites","fightclub","flist", "fc", "fl")]
		public async Task ShowPVPEnabledFighters()
		{
			List<Character> pvpCharacters = _rpgService.GetPVPCharacters();
			await Context.Channel.SendMessageAsync(embed: _rpgService.BuildPvPListEmbed(pvpCharacters));
		}

		[Command("challenge"), Alias("fight","chal", "fite")]
		[Summary("Fight someone who is PvP flagged. You have to be PvP flagged too.")]
		public async Task ChallengeCharacter([Remainder, Summary("The name of the character you want to fight. They might be PvP Flagged")]string target)
		{
			Character attacker = _rpgService.GetCharacter((IGuildUser) Context.Message.Author);
			Character defender = _rpgService.GetFighter(target);
			if (defender == null)
			{
				await Context.Channel.SendMessageAsync($"I don't see anyone named {target} in the PvP list. Sorry.");
			}
			else if (attacker == null)
			{
				await Context.Channel.SendMessageAsync("I don't think you have a character, friend.");
			}
			else if (!attacker.PvPFlagged)
			{
				await Context.Channel.SendMessageAsync("I know you're excited, but you have to be PvP flagged before you can punch people yanno?!");
			}
			else if (attacker.Name == defender.Name && attacker.DiscordUserId == defender.DiscordUserId)
			{
				await Context.Channel.SendMessageAsync("You can't fight yourself, get out of here.");
			}
			else
			{
				EmbedBuilder embed = new EmbedBuilder();
				CombatStats stats = _rpgService.Fight(attacker, defender);

				string attackerStatus = attacker.Hitpoints > 0 ? $"{attacker.Hitpoints} HP remaining" : "Cadaverific";
				string defenderStatus = defender.Hitpoints > 0 ? $"{defender.Hitpoints} HP remaining" : "Cadaverific";
				if (attacker.Hitpoints <= 0)
				{
					KillCharacter(attacker, defender, true, embed);
				}
				else
				{
					KillCharacter(defender, attacker, false, embed);
				}
				embed.AddField($"{attacker.Name} Combat Stats", $"{stats.AttackerHits} Hits. {stats.AttackerMisses} Misses. {stats.AttackerDmg} Damage done.{Environment.NewLine}Status: {attackerStatus}");
				embed.AddField($"{defender.Name} Combat Stats", $"{stats.DefenderHits} Hits. {stats.DefenderMisses} Misses. {stats.DefenderDmg} Damage done.{Environment.NewLine}Status: {defenderStatus}");

				await Context.Channel.SendMessageAsync(embed: embed.Build());
			}
		}

		public EmbedBuilder KillCharacter(Character corpse, Character murderer, bool wasCorpseAttacker, EmbedBuilder embed)
		{
			int corpseGold = corpse.Gold;
			int libcoinPayout = corpseGold * 15;

			if (wasCorpseAttacker)
			{
				embed.Title = $"{corpse.Name} fucked around and found out. F's in the chat.";
			}
			else
			{
				embed.Title = $"{corpse.Name} got got.";
			}
			embed.ImageUrl = murderer.AvatarUrl;
			embed.ThumbnailUrl = corpse.AvatarUrl;

			if (libcoinPayout > 0)
			{
				Context.Channel.SendMessageAsync($"{corpse.Name} had {corpseGold} in their pocket, so I've issued a {libcoinPayout} Libcoin payout.");
			}
			_rpgService.KillCharacter(corpse);

			return embed;
		}

	}
}
