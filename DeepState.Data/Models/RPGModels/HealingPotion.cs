using DartsDiscordBots.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Data.Models
{
	class HealingPotion : ConsumableItem
	{
		public int HealingDiceSize;

		public void DrinkPotion(Character character)
		{
			Uses--;
			character.Hitpoints += new Dice(HealingDiceSize).Roll();
			if(character.Hitpoints > character.MaximumHitpoints)
			{
				character.Hitpoints = character.MaximumHitpoints;
			}
		}
	}
}
