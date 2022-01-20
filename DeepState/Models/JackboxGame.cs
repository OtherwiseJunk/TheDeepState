using DartsDiscordBots.Modules.Jackbox.Models;
using Discord;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Modules
{
	public class JackboxGame
	{
		[Key]
		public int ID { get; set; }
		public ulong DiscordGuildId { get; set; }
		public string Name { get; set; }
		public string PlayerName { get; set; }
		public string Description { get; set; }
		public int JackboxVersion { get; set; }
		public int MaxPlayers { get; set; }
		public int MinPlayers { get; set; }
		public IEmote VotingEmoji { get; set; }
		public bool HasAudience { get; set; }
		public override string ToString()
		{
			string emote = VotingEmoji as Emote != null ? VotingEmoji.ToString() : (VotingEmoji as Emoji).Name;
			string audienceText = HasAudience ? "" : "(No Audience)";
			double rating = Ratings.Select(r => r.Rating).DefaultIfEmpty(0).Average();
			return $"{emote} {Name} ({MinPlayers}-{MaxPlayers} {PlayerName}) {audienceText} Rating:{rating} ({Ratings.Count} votes) Jackbox Version: {JackboxVersion}";
		}
		public List<GameRating> Ratings { get; set; }
	}
}
