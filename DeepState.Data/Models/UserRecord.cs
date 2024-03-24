using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace DeepState.Data.Models
{
	public class UserRecord
	{
		[Key]
		public int UserId { get; set; }
		public ulong DiscordUserId { get; set; }
		public ulong DiscordGuildId { get; set; }
		public double LibcraftCoinBalance { get; set; }
		public int TableFlipCount { get; set; }
		public bool TimeOut { get; set; }
		public DateTime LastTimePosted { get; set; }
	}
}
