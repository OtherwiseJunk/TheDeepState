using System.ComponentModel.DataAnnotations;

namespace DeepState.Data.Models
{
	public class HungerGamesTribute
	{
		[Key]
		public int TributeId { get; set; }
		public ulong DiscordGuildId { get; set; }
		public ulong DiscordUserId { get; set; }
		public bool IsAlive { get; set; }
		public string DeathMessage { get; set; }
		public string ObituaryMessage { get; set; }
		public int District { get; set; }
	}
}
