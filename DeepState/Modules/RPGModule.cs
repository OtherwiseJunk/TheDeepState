using DartsDiscordBots.Utilities;
using DeepState.Data.Constants;
using DeepState.Data.Models.RPGModels;
using DeepState.Data.Services;
using DeepState.Modules.Preconditions;
using DeepState.Service;
using Discord;
using Discord.Commands;
using Svg;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

		[Group("config")]
		[RequireOwner(Group = "AllowedUsers"), RequireUserPermission(GuildPermission.ManageMessages, Group = "AllowedUsers")]
		public class Configuration : ModuleBase
		{
			public RPGService _rpgService { get; set; }
			public Configuration(RPGService rpgService)
			{
				_rpgService = rpgService;
			}
			[Command("obituary"), Alias("obit")]
			public async Task ConfigureObituariesChannel()
			{
				_rpgService.SetRPGObituaryChannel(Context.Guild.Id, Context.Channel.Id);
				await Context.Message.AddReactionAsync(Emoji.Parse("👍"));
			}
		}

		[Command("newchar"), Alias("nc")]
		[RequireLibcoinBalance(RPGConstants.NewCharacterCost)]
		[Summary("Generate a new character. Costs some libcoin. You can add a 'fite' or 'f' flag to flag the character for PvP from creation.")]
		public async Task GenerateNewCharacter(string flags = "")
		{
			MessageReference msgReference = new MessageReference(Context.Message.Id);
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
							await Context.Channel.SendMessageAsync("Today your character woke up and chose _violence_.", messageReference: msgReference);
							_rpgService.ToggleCharacterPvPFlag((IGuildUser)Context.Message.Author);
							character = _rpgService.GetCharacter((IGuildUser)Context.Message.Author);
						}
						await Context.Channel.SendMessageAsync($"Ok I rolled you up a new character! {RPGConstants.NewCharacterCost} Libcoin has been deducted from your account.", embed: _rpgService.BuildCharacterEmbed(character), messageReference: msgReference);
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
				await Context.Channel.SendMessageAsync("Looks like you already have a character.", embed: _rpgService.BuildCharacterEmbed(character), messageReference: msgReference);
			}
		}

		[Command("char"), Alias("c")]
		[Summary("View your current character, if you have one.")]
		public async Task RetrieveCharacter()
		{
			MessageReference msgReference = new MessageReference(Context.Message.Id);
			Character character = _rpgService.GetCharacter((IGuildUser)Context.User);
			if (character == null)
			{
				await Context.Channel.SendMessageAsync($"Sorry friend, you haven't rolled up a character yet. It costs {RPGConstants.NewCharacterCost} Libcoin but it's worth it!", messageReference: msgReference);
			}
			else
			{
				await Context.Channel.SendMessageAsync(embed: _rpgService.BuildCharacterEmbed(character), messageReference: msgReference);
			}
		}

		[Command("pvp")]
		[Summary("Toggle whether or not your character is down to PvP. You can view them in the >rpg pvplist list, and PvP flagged characters will have flaming swords when retireved with >rpg char.")]
		public async Task TogglePvPStatus()
		{
			MessageReference msgReference = new MessageReference(Context.Message.Id);
			_rpgService.ToggleCharacterPvPFlag((IGuildUser)Context.User);
			Character character = _rpgService.GetCharacter((IGuildUser)Context.User);
			if (character.PvPFlagged)
			{
				await Context.Channel.SendMessageAsync($"Ok, I'll let everyone know {character.Name} is ready to rumble!", messageReference: msgReference);
			}
			else
			{
				await Context.Channel.SendMessageAsync($"Ok, I'll let everyone know {character.Name} is character of peace. For now.", messageReference: msgReference);
			}
		}

		[Command("pvplist"), Alias("saturdaynight", "goodfites", "fightclub", "flist", "fc", "fl")]
		[Summary("View a list of characters down to punch people to death. Or maybe get punched to death, yanno.")]
		public async Task ShowPVPEnabledFighters()
		{
			MessageReference msgReference = new MessageReference(Context.Message.Id);
			List<Character> pvpCharacters = _rpgService.GetPVPCharacters();
			await Context.Channel.SendMessageAsync(embed: _rpgService.BuildPvPListEmbed(pvpCharacters), messageReference: msgReference);
		}

		[Command("challenge"), Alias("fight", "chal", "fite")]
		[Summary("Fight someone who is PvP flagged. You have to be PvP flagged too.")]
		public async Task ChallengeCharacter([Remainder, Summary("The name of the character you want to fight. They might be PvP Flagged")] string target = "")
		{
			MessageReference msgReference = new MessageReference(Context.Message.Id);
			Character attacker = _rpgService.GetCharacter((IGuildUser)Context.Message.Author);
			Character defender;
			if (target == "")
			{
				List<Character> pvpers = _rpgService.GetPVPCharacters().Where(c => c.DiscordUserId != Context.Message.Author.Id).ToList();
				pvpers.Shuffle();
				defender = pvpers.First();
			}
			else
			{
				defender = _rpgService.GetFighter(target);
			}

			if (defender == null)
			{
				await Context.Channel.SendMessageAsync($"I don't see anyone named {target} in the PvP list. Sorry.", messageReference: msgReference);
			}
			else if (attacker == null)
			{
				await Context.Channel.SendMessageAsync("I don't think you have a character, friend.", messageReference: msgReference);
			}
			else if (!attacker.PvPFlagged)
			{
				await Context.Channel.SendMessageAsync("I know you're excited, but you have to be PvP flagged before you can punch people yanno?!", messageReference: msgReference);
			}
			else if (attacker.Name == defender.Name && attacker.DiscordUserId == defender.DiscordUserId)
			{
				await Context.Channel.SendMessageAsync("You can't fight yourself, get out of here.", messageReference: msgReference);
			}
			else
			{
				EmbedBuilder embed = new EmbedBuilder();
				CombatStats stats = _rpgService.Fight(attacker, defender);

				string attackerStatus = attacker.Hitpoints > 0 ? $"{attacker.Hitpoints} HP remaining" : "Cadaverific";
				string defenderStatus = defender.Hitpoints > 0 ? $"{defender.Hitpoints} HP remaining" : "Cadaverific";
				string victorMessage;
				if (attacker.Hitpoints <= 0)
				{
					WrapUpCombat(defender, attacker, true, Context.Guild.Id, embed);
				}
				else
				{
					WrapUpCombat(attacker, defender, false, Context.Guild.Id, embed);
				}
				embed.AddField($"{attacker.Name} Combat Stats", $"{stats.AttackerHits} Hits. {stats.AttackerMisses} Misses. {stats.AttackerDmg} Damage done.{Environment.NewLine}Status: {attackerStatus}");
				embed.AddField($"{defender.Name} Combat Stats", $"{stats.DefenderHits} Hits. {stats.DefenderMisses} Misses. {stats.DefenderDmg} Damage done.{Environment.NewLine}Status: {defenderStatus}");

				await Context.Channel.SendMessageAsync(embed: embed.Build(), messageReference: msgReference);
			}
		}

		[Command("items"), Alias("i")]
		[Summary("List all your character's items")]
		public async Task GetCharacterItems(){
			Character character = _rpgService.GetCharacter((IGuildUser) Context.Message.Author);
			MessageReference msgReference = new MessageReference(Context.Message.Id);
			if (character == null)
			{
				await Context.Channel.SendMessageAsync("You don't have a character, so they don't have items you see.", messageReference: msgReference);
				return;
			}

			if(character.Items.Count > 0)
			{
				EmbedBuilder embed = new EmbedBuilder();
				embed.WithTitle($"{character.Name}'s Items");
				foreach(Item item in character.Items)
				{
					embed.AddField($"{item.ItemID}. {item.Name}", item.Description);
				}
				await Context.Channel.SendMessageAsync(embed: embed.Build());
			}
			else
			{
				await Context.Channel.SendMessageAsync("Ok that's easy. You have none.");
			}
		}

		[Command("use"), Alias("u")]
		[Summary("Consume an item, if possible. itemID value can be retireved from the `>pvp items` list.")]
		public async Task UseItem([Summary("The id of the item to get, from `>pvp items` list.")] int itemId)
		{
			MessageReference msgReference = new MessageReference(Context.Message.Id);
			Character character = _rpgService.GetCharacter((IGuildUser)Context.Message.Author);
			if (character == null)
			{
				await Context.Channel.SendMessageAsync("You don't have a character, so they don't have items you see.", messageReference: msgReference);
				return;
			}

			Item item = _rpgService.GetItem(character, itemId);
			if (item == null)
			{
				await Context.Channel.SendMessageAsync($"Sorry, I didn't see item {itemId} in your list.", messageReference: msgReference);
				return;
			}

			if (_rpgService.IsItemConsumable(item))
			{
				_rpgService.UseItem(character, (ConsumableItem)item, (ITextChannel)Context.Channel);
			}
			else
			{
				await Context.Channel.SendMessageAsync($"Sorry, {item.Name} is not a consumable item. Maybe you need to `>rpg equip` it?", messageReference: msgReference);
			}
		}

		[Command("loot")]
		[RequireOwner()]
		public async Task lootTest()
		{
			Character character = _rpgService.GetCharacter((IGuildUser)Context.Message.Author);
			Loot loot = new Loot { items = new List<Item> { RPGConstants.StrangeMeat } };
			foreach(Item item in loot.items)
			{
				item.character = character;
				character.Items.Add(item);
			}
			character.Items.AddRange(loot.items);
			_rpgService.UpdateCharacter(character);
		}

		public void WrapUpCombat(Character winner, Character loser, bool wasCorpseAttacker, ulong guildId, EmbedBuilder embed)
		{
			winner.PlayersMurdered++;
			Loot loot = LootCharacter(loser);
			winner.Gold += loot.Gold;
			int xpGained = CalculateXPGain(winner, loser);
			winner.XP += xpGained;
			string resultMessage;
			if (loot.items.Count > 0)
			{
				foreach(Item item in loot.items)
				{
					item.character = winner;
					winner.Items.Add(item);
				}
				resultMessage = $"For slaying {loser.Name}, {winner.Name} got {loot.Gold} gold and {xpGained} XP. They also found {loot.items.Count} items!";
			}
			else
			{
				resultMessage = $"For slaying {loser.Name}, {winner.Name} got {loot.Gold} gold and {xpGained} XP";
			}
			if (winner.XP >= 10 * winner.Level)
			{
				winner.XP -= 10 * winner.Level;
				winner.LevelUp();
				resultMessage += $"{Environment.NewLine}Level up!";
			}
			embed.AddField("Result", resultMessage);
			_rpgService.UpdateCharacter(winner);
			KillCharacter(loser, winner, wasCorpseAttacker, guildId, embed);
		}
		public int CalculateXPGain(Character murderer, Character corpse)
		{
			double levelDifferenceMultiplier = corpse.Level / murderer.Level;
			Random rand = new Random(Guid.NewGuid().GetHashCode());
			return Convert.ToInt32(Math.Floor(levelDifferenceMultiplier * rand.Next(1,5)));
		}
		public Loot LootCharacter(Character corpse)
		{
			Loot loot = new();
			Random random = new Random(Guid.NewGuid().GetHashCode());
			if(corpse.Gold <= 1)
			{
				loot.Gold = random.Next(1, 4);
			}
			else
			{
				loot.Gold = random.Next(1, corpse.Gold);
			}

			if(corpse.Items.Count > 0)
			{
				foreach(Item item in corpse.Items)
				{
					if (new Dice(10).Roll() > 2)
					{
						loot.items.Add(item);
					}
				}	
			}
			else
			{
				if(new Dice(10).Roll() > 8)
				{
					loot.items.Add(RPGConstants.StrangeMeat);
				}
			}

			return loot;
		}

		public EmbedBuilder KillCharacter(Character corpse, Character murderer, bool wasCorpseAttacker, ulong guildIdForPayout, EmbedBuilder embed)
		{
			int corpseGold = corpse.Gold;
			int libcoinPayout = corpseGold * 2;

			if (wasCorpseAttacker)
			{
				embed.Title = $"{corpse.Name} fucked around and found out. F's in the chat.";
			}
			else
			{
				embed.Title = $"{corpse.Name} got got.";
			}
			embed.ThumbnailUrl = murderer.AvatarUrl;

			if (libcoinPayout > 0)
			{
				Context.Channel.SendMessageAsync($"{corpse.Name} had {corpseGold} in their pocket, so I've issued a {libcoinPayout} Libcoin payout.");
				_userService.Grant(corpse.DiscordUserId, guildIdForPayout, libcoinPayout);
			}
			SendObituary(corpse, murderer, Context.Guild);
			_rpgService.KillCharacter(corpse);

			return embed;
		}
		public void SendObituary(Character corpse, Character murderer, IGuild murderLocation)
		{
			List<RPGConfiguration> configs = _rpgService.GetConfigurations();
			foreach(RPGConfiguration config in configs)
			{
				IGuild guild = Context.Client.GetGuildAsync(config.DiscordGuildId).Result;
				string deathMessage = config.DiscordGuildId == murderLocation.Id ? $"{corpse.Name} was killed by {murderer.Name} in THIS VERY SERVER!" : $"{corpse.Name} was killed by {murderer.Name} in a strange unknowable land.";
				ITextChannel channel = (ITextChannel) guild.GetChannelAsync(config.ObituaryChannelId).Result;
				EmbedBuilder embed = new EmbedBuilder();
				embed.Title = RPGConstants.ObituaryTitles.GetRandom();
				embed.ThumbnailUrl = murderer.AvatarUrl;
				embed.ImageUrl = corpse.AvatarUrl;
				embed.AddField("A murder most foul", deathMessage);
				channel.SendMessageAsync(embed: embed.Build());
			}
		}
	}

	
}
