using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DeepState.Data.Models
{
	public class UserRecord
	{
		[Key]
		public int UserId { get; set; }
		public ulong DiscordUserId { get; set; }
		public ulong DiscordGuildId { get; set; }
		public decimal LibcraftCoinBalance { get; set; }
		public int TableFlipCount { get; set; }
		public bool TimeOut { get; set; }
	}
}
