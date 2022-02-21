using DartsDiscordBots.Utilities;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Data.Models.RPGModels
{
	public class HealingItem : ConsumableItem
	{
		public int HealingDiceSize;

		public override void Use(Character character, ITextChannel channel)
		{
			Uses--;
			int healing = new Dice(HealingDiceSize).Roll();
			character.Hitpoints += healing;
			if (character.Hitpoints > character.MaximumHitpoints)
			{
				character.Hitpoints = character.MaximumHitpoints;
			}
			channel.SendMessageAsync(String.Format(ConsumeMessage, character.Name, healing));
		}
	}
}
