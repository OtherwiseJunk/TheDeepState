using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Data.Models.RPGModels
{
	public abstract class ConsumableItem : Item
	{
		public int Uses { get; set; }
		public string ConsumeMessage { get; set; }
		public abstract void Use(Character character, ITextChannel channel);
	}
}
