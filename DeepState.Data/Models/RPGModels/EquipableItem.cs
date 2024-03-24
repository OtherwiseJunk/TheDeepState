using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Data.Models.RPGModels
{
	public class EquipableItem : Item
	{
		public int ItemID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int Price { get; set; }
		EquipmentSlot Slot { get; set; }
	}

	public enum EquipmentSlot
	{
		Helmet,
		Chest,
		Hands,
		Legs,
		Feet,
		Necklace,
		Ring,
		OHWeapon,
		THWeapon
	}
}
