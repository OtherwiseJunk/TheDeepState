using DeepState.Data.Context;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeepState.Modules
{
	public class OutOfContextModule : ModuleBase
	{
		public OOCDBContext _DBContext { get; set; }

		public OutOfContextModule(OOCDBContext context)
		{
			_DBContext = context;
		}
	}
}
