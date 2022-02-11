using DartsDiscordBots.Utilities;
using DeepState.Data.Context;
using DeepState.Data.Models;
using DeepState.Models;
using Discord;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeepState.Data.Services
{
	public class RPGService
	{
		IDbContextFactory<RPGContext> _contextFactory { get; set; }

		public RPGService(IDbContextFactory<RPGContext> contextFactory)
		{
			_contextFactory = contextFactory;
		}
		public Character CreateNewCharacter(IGuildUser user, string avatarUrl)
		{
			using (RPGContext context = _contextFactory.CreateDbContext())
			{
				Character newCharacter = new Character(user.Id, avatarUrl);
				context.Characters.Add(newCharacter);
				context.SaveChanges();
				return newCharacter;
			}
		}

		public List<RPGConfiguration> GetConfigurations()
		{
			using(RPGContext context = _contextFactory.CreateDbContext())
			{
				return context.RPGConfigs.ToList();
				
			}
		}

		public RPGConfiguration GetGuildConfiguration(ulong guildId)
		{
			using(RPGContext context = _contextFactory.CreateDbContext())
			{
				return context.RPGConfigs.FirstOrDefault(config => config.DiscordGuildId == guildId);
			}
		}

		public void CreateRPGConfiguration(RPGConfiguration config)
		{
			using (RPGContext context = _contextFactory.CreateDbContext())
			{
				context.RPGConfigs.Add(config);
				context.SaveChanges();
			}
		}

		public void UpdateRPGConfiguration(RPGConfiguration config)
		{
			using (RPGContext context = _contextFactory.CreateDbContext())
			{
				context.Entry(config).State = EntityState.Modified;
				context.SaveChanges();
			}
		}

		public void SetRPGObituaryChannel(ulong guildId, ulong channelId)
		{
			RPGConfiguration config = GetGuildConfiguration(guildId);
			if(config == null)
			{
				CreateRPGConfiguration(new RPGConfiguration { DiscordGuildId = guildId, ObituaryChannelId = channelId });
			}
			else
			{
				config.ObituaryChannelId = channelId;
				UpdateRPGConfiguration(config);
			}
		}

		public Character GetCharacter(IGuildUser user)
		{
			using (RPGContext context = _contextFactory.CreateDbContext())
			{
				return context.Characters.FirstOrDefault(c => c.DiscordUserId == user.Id);
			}
		}

		public List<Character> GetPVPCharacters()
		{
			using (RPGContext context = _contextFactory.CreateDbContext())
			{
				return context.Characters.AsQueryable().Where(c => c.PvPFlagged).ToList();
			}
		}

		public void ToggleCharacterPvPFlag(IGuildUser user)
		{
			using (RPGContext context = _contextFactory.CreateDbContext())
			{
				Character character = context.Characters.FirstOrDefault(c => c.DiscordUserId == user.Id);
				character.PvPFlagged = !character.PvPFlagged;

				context.SaveChanges();
			}
		}

		public void KillCharacter(Character corpse)
		{
			using (RPGContext context = _contextFactory.CreateDbContext())
			{
				context.Characters.Remove(corpse);
				context.SaveChanges();

			}
		}

		public Character GetFighter(string characterName)
		{
			return GetPVPCharacters().FirstOrDefault(c => c.Name.ToLower() == characterName.ToLower());
		}

		public CombatStats Fight(Character attacker, Character defender)
		{
			Dice d9 = new(9);
			CombatStats stats = new CombatStats(d9.Roll() + attacker.Mobility, d9.Roll() + defender.Mobility);
			while (attacker.Hitpoints > 0 && defender.Hitpoints > 0)
			{
				if (stats.AttackersTurn)
				{
					stats = SingleAttack(stats, attacker, defender, true);
				}
				else
				{
					stats = SingleAttack(stats, defender, attacker, false);
				}
				stats.AttackersTurn = !stats.AttackersTurn;
			}

			return stats;
		}

		public CombatStats SingleAttack(CombatStats stats, Character attacker, Character defender, bool attackerInitiatedCombat)
		{
			Dice d9 = new(9);
			Dice d4 = new(4);

			int attack = d9.Roll() + attacker.Power;
			int defense = d9.Roll() + defender.Mobility;
			if (attack > defense)
			{
				int dmg = attacker.Power + d4.Roll();
				defender.Hitpoints -= dmg;
				if (attackerInitiatedCombat)
				{
					stats.AttackerDmg += dmg;
					stats.AttackerHits++;
				}
				else
				{
					stats.DefenderDmg += dmg;
					stats.DefenderHits++;
				}
			}
			else
			{
				if (attackerInitiatedCombat)
				{
					stats.AttackerMisses++;
				}
				else
				{
					stats.DefenderMisses++;
				}
			}


			return stats;
		}

		public Embed BuildCharacterEmbed(Character character)
		{
			EmbedBuilder builder = new EmbedBuilder();
			builder.ThumbnailUrl = character.AvatarUrl;		
			builder.AddField("Name", character.Name);
			builder.AddField("Level", character.Level);
			builder.AddField("Power", character.Power);
			builder.AddField("Mobility", character.Mobility);
			builder.AddField("Fortitude", character.Fortitude);
			builder.AddField("Gold", character.Gold);
			builder.AddField("XP", character.XP);
			builder.AddField("Hitpoints", $"{character.Hitpoints}/{character.MaximumHitpoints}");

			return builder.Build();
		}

		public void UpdateCharacter(Character character)
		{
			using(RPGContext context = _contextFactory.CreateDbContext())
			{
				context.Entry(character).State = EntityState.Modified;
				context.SaveChanges();
			}
		}
		public Embed BuildPvPListEmbed(List<Character> characters)
		{
			EmbedBuilder builder = new EmbedBuilder();
			builder.Title = "People who aren't $&#!ing cowards";
			foreach(Character character in characters.Where(c => c.PvPFlagged))
			{				
				builder.AddField(character.Name, $"Level {character.Level} character");
			}

			return builder.Build();
		}
	}
}
