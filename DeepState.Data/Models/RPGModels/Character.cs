using DartsDiscordBots.Utilities;
using DeepState.Data.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Data.Models.RPGModels
{
	public class Character
	{
		[Key]
		public ulong DiscordUserId { get; set; }
		public string Name { get; set; }
		public int Level { get; set; }
		public int Power { get; set; }
		public int Mobility { get; set; }
		public int Fortitude { get; set; }
		public int Hitpoints { get; set; }
		public int MaximumHitpoints { get; set; }		
		public int Gold { get; set; }
		public int XP { get; set; }
		public string AvatarUrl { get; set; }
		public int PlayersMurdered { get; set; }
		public bool PvPFlagged { get; set; }
		public List<Item> Items { get; set; }

		public Character()
		{

		}
		public Character(ulong discordId, string avatarUrl)
		{
			AvatarUrl = avatarUrl;
			DiscordUserId = discordId;
			Name = $"{RPGConstants.Names.GetRandom()} the {RPGConstants.titles.GetRandom()}";
			int[] stats = RPGConstants.StartingStatArrays.GetRandom();
			Power = stats[0];
			Mobility = stats[1];
			Fortitude = stats[2];
			Level = 1;
			MaximumHitpoints = new Dice(9).Roll() + Fortitude;
			Hitpoints = MaximumHitpoints;
			XP = 0;
			Gold = 0;
			PlayersMurdered = 0;
			PvPFlagged = false;
		}

		public void LevelUp()
		{
			int[] levelUpStats = RPGConstants.LevelUpArrays.GetRandom();
			Power += levelUpStats[0];
			Mobility += levelUpStats[1];
			Fortitude += levelUpStats[2];
			int newHitpoints = new Dice(9).Roll() + Fortitude;
			MaximumHitpoints += newHitpoints;
			Hitpoints += newHitpoints;
			Level++;
			XP = 0;
		}
	}
}
