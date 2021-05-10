using Discord;
using Microsoft.EntityFrameworkCore;
using System;
using DartsDiscordBots.Utilities;

namespace DeepState.Data.Context
{
	public class OOCDBContext : DbContext
	{
		public OOCDBContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<OOCItem> OutOfContextRecords { get; set; }

		public bool ImageExists(string base64Image)
		{
			return OutOfContextRecords.FirstOrDefaultAsync(oocr => oocr.Base64Image == base64Image).Result != null;
		}

		public void AddRecord(ulong reportingUserId, string base64Image)
		{
			OutOfContextRecords.Add(new OOCItem
			{
				ReportingUserId = reportingUserId,
				Base64Image = base64Image,
				DateStored = DateTime.Now
			});

			this.SaveChanges();
		}

		public OOCItem GetRandomRecord()
		{
			return OutOfContextRecords.ToListAsync().Result.GetRandom();
		}
	}
}
