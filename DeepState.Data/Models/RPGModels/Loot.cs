using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Data.Models.RPGModels
{
	public class Loot
	{
		public Loot()
		{
			items = new();
		}
		public int Gold;
		public List<Item> items;
	}
}
