using DeepState.Data.Models;
using DeepState.Utilities;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Models
{
	public class HungerGamesGameState
	{
		public IGuild Guild { get; set; }
		public IMessageChannel TributeChannel { get; set; }
		public IMessageChannel CorpseChannel { get; set; }
		public IRole TributeRole { get; set; }
		public IRole CorpseRole { get; set; }
		public List<HungerGamesTribute> Tributes { get; set; }
		public EventStage CurrentStage { get; set; }
	}
}
