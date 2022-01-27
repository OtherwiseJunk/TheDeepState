using DeepState.Data.Context;
using DeepState.Data.Models;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Data.Services
{
	public class RPGService
	{
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

		public Embed BuildCharacterEmbed(Character character)
		{
			EmbedBuilder builder = new EmbedBuilder();
			builder.ImageUrl = character.AvatarUrl;
			builder.ThumbnailUrl = "https://d338t8kmirgyke.cloudfront.net/icons/icon_pngs/000/005/780/original/swords.png";
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
	}
}
