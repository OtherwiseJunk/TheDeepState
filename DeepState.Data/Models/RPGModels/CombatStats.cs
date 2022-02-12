using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Models
{
	public class CombatStats
	{
		public bool AttackersTurn { get; set; }
		public int DefenderHits { get; set; }
		public int DefenderMisses { get; set; }
		public int DefenderDmg { get; set; }
		public int AttackerHits { get; set; }
		public int AttackerMisses { get; set; }
		public int AttackerDmg { get; set; }

		public CombatStats(int attackerInitiative, int defenderInitiative)
		{
			AttackersTurn = attackerInitiative >= defenderInitiative;
			DefenderHits = 0;
			DefenderMisses = 0;
			DefenderDmg = 0;
			AttackerHits = 0;
			AttackerMisses = 0;
			AttackerDmg = 0;
		}
	}
}
