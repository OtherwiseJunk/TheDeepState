using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Data.Models.RPGModels
{
	public class RPGConfiguration
	{
		[Key]
		public ulong DiscordGuildId { get; set; }
		public ulong ObituaryChannelId { get; set; }
	}
}
