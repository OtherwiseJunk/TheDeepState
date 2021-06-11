using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Data.Models
{
	public class HungerGamesServerConfiguration
	{
		[Key]
		public int ConfigurationId { get; set; }
		public ulong DiscordGuildId { get; set; }
		public ulong TributeAnnouncementChannelId { get; set; }
		public ulong CorpseAnnouncementChannelId { get; set; }

	}
}
