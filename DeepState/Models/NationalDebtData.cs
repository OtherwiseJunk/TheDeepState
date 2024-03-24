using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Models
{

	public class NationalDebtData
	{
		public string effectiveDate { get; set; }
		public float governmentHoldings { get; set; }
		public float publicDebt { get; set; }
		public float totalDebt { get; set; }
	}

}
