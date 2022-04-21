using Discord;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeepState.Data.Models
{
	public class OOCItem
	{
		[Key]
		public int ItemID { get; set; }
		public ulong ReportingUserId { get; set; }
        public string ImageUrl { get; set; }
		public ulong DiscordGuildId { get; set; }
        public DateTime DateStored { get; set; }
	}
}
