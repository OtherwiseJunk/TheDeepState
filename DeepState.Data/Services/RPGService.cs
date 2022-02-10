using DeepState.Data.Context;
using DeepState.Data.Models;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DartsDiscordBots.Utilities;

namespace DeepState.Data.Services
{
	public class RPGService
	{
		UserRecordsService _userRecords { get; set; }
		public Character CreateNewCharacter(IGuildUser user, string avatarUrl)
		{
			using (RPGContext context = new RPGContext())
			{
				Character newCharacter = new Character(user.Id, avatarUrl);
				context.Characters.Add(newCharacter);
				context.SaveChanges();
				return newCharacter;
			}
		}

		public Character GetCharacter(IGuildUser user)
		{
			using (RPGContext context = new RPGContext())
			{
				return context.Characters.FirstOrDefault(c => c.DiscordUserId == user.Id);
			}
		}

		public List<Character> GetPVPCharacters()
		{
			using (RPGContext context = new RPGContext())
			{
				return context.Characters.AsQueryable().Where(c => c.PvPFlagged).ToList();
			}
		}

		public void ToggleCharacterPvPFlag(IGuildUser user)
		{
			using (RPGContext context = new RPGContext())
			{
				Character character = context.Characters.FirstOrDefault(c => c.DiscordUserId == user.Id);
				character.PvPFlagged = !character.PvPFlagged;

				context.SaveChanges();
			}
		}

		public void KillCharacter(Character corpse)
		{
			using (RPGContext context = new RPGContext())
			{
				context.Characters.Remove(corpse);
				context.SaveChanges();

			}
		}

		public Character GetFighter(string characterName)
		{
			return GetPVPCharacters().FirstOrDefault(c => c.Name.ToLower() == characterName);
		}

		public Embed BuildCharacterEmbed(Character character)
		{
			EmbedBuilder builder = new EmbedBuilder();
			builder.ImageUrl = character.AvatarUrl;
			builder.ThumbnailUrl = "https://d338t8kmirgyke.cloudfront.net/icons/icon_pngs/000/005/780/original/swords.png";
			if (character.PvPFlagged)
			{
				builder.ThumbnailUrl = "https://spng.pngfind.com/pngs/s/273-2730354_this-free-icons-png-design-of-flaming-sword.png";
			}			
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
