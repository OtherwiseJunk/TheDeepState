using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Data.Models
{
	public class LibcoinEconomicStatistics
	{
		public double TotalCirculation { get; set; }
		public double MeanBalance { get; set; }
		public double MedianBalance { get; set; }
		public ulong PoorestUser { get; set; }
		public ulong RichestUser { get; set; }
		public double GiniCoefficient { get; set; }
	}
}
