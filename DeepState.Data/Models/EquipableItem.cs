using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Data.Models
{
	public class EquipableItem : Item
	{
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
