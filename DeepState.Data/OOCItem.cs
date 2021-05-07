using Discord;
using System;

namespace DeepState.Data
{
	public class OOCItem
	{
		public IGuildUser ReportingUser { get; set; }
		public string Base64Image { get; set; }
		public DateTime DateStored { get; set; }
	}
}
