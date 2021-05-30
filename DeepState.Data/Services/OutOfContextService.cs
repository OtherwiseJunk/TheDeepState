using Microsoft.EntityFrameworkCore;
using DeepState.Data.Context;
using System;
using System.Collections.Generic;
using System.Text;
using DeepState.Data.Models;
using DartsDiscordBots.Utilities;

namespace DeepState.Data.Services
{
	public class OutOfContextService
	{
		IDbContextFactory<OOCDBContext> _contextFactory { get; set; }
		public OutOfContextService(IDbContextFactory<OOCDBContext> contextFactory)
		{
			_contextFactory = contextFactory;
		}
		public bool ImageExists(string base64Image)
		{
			using(OOCDBContext context = _contextFactory.CreateDbContext())
			{
				return context.OutOfContextRecords.FirstOrDefaultAsync(oocr => oocr.Base64Image == base64Image).Result != null;
			}

		}

		public void DeleteImage(string base64Image)
		{
			using (OOCDBContext context = _contextFactory.CreateDbContext())
			{
				OOCItem itemToDelete = context.OutOfContextRecords.FirstOrDefaultAsync(oocr => oocr.Base64Image == base64Image).Result;
				context.OutOfContextRecords.Remove(itemToDelete);

				context.SaveChanges();
			}
		}

		public void AddRecord(ulong reportingUserId, string base64Image)
		{
			using (OOCDBContext context = _contextFactory.CreateDbContext())
			{
				context.OutOfContextRecords.Add(new OOCItem
				{
					ReportingUserId = reportingUserId,
					Base64Image = base64Image,
					DateStored = DateTime.Now
				});

				context.SaveChanges();
			}
			
		}

		public OOCItem GetRandomRecord()
		{
			using (OOCDBContext context = _contextFactory.CreateDbContext())
			{
				return context.OutOfContextRecords.ToListAsync().Result.GetRandom();
			}
		}
	}
}
