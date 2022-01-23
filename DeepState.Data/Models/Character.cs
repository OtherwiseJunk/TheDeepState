using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Data.Models
{
	public class Character
	{
		public string Name { get; set; }
		public int Level { get; set; }
		public int Power { get; set; }
		public int Mobility { get; set; }
		public int Fortitude { get; set; }
		public int Hitpoints { get; set; }
		public int Gold { get; set; }
		public int XP { get; set; }
		public List<Item> Inventory { get; set; }
		public EquipableItem Head { get; set; }
		public EquipableItem Chest { get; set; }
		public EquipableItem Hands { get; set; }
		public EquipableItem Legs { get; set; }
		public EquipableItem Feet { get; set; }
		public EquipableItem Necklace { get; set; }
		public EquipableItem LeftRing { get; set; }
		public EquipableItem RightRing { get; set; }
		public EquipableItem LeftWeapon { get; set; }
		public EquipableItem RightWeapon { get; set; }
		public ulong DiscordUserId { get; set; }
	}
}
