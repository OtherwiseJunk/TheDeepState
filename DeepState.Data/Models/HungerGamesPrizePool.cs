using System.ComponentModel.DataAnnotations;

namespace DeepState.Data.Models
{
	public class HungerGamesPrizePool
	{
		[Key]
		public int PrizePoolID { get; set; }
		public ulong DiscordGuildId { get; set; }
		public double PrizePool { get; set; }
	}
}
