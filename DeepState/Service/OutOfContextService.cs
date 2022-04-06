using Microsoft.EntityFrameworkCore;
using DeepState.Data.Context;
using System;
using DeepState.Data.Models;
using DartsDiscordBots.Utilities;
using System.IO;

namespace DeepState.Service
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
				return context.OutOfContextRecords.FirstOrDefaultAsync(oocr => oocr.ImageUrl == base64Image).Result != null;
			}

		}

		public void DeleteImage(string imageUrl)
		{
			using (OOCDBContext context = _contextFactory.CreateDbContext())
			{
				OOCItem itemToDelete = context.OutOfContextRecords.FirstOrDefaultAsync(oocr => oocr.ImageUrl == imageUrl).Result;
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

					ImageUrl = base64Image,
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

		public void MoveToTheCloud(ImagingService _image)
		{
			using(OOCDBContext context = _contextFactory.CreateDbContext())
			{
				int totalRecords = context.OutOfContextRecords.CountAsync().Result;
				int processedRecords = 1;
				foreach(OOCItem item in context.OutOfContextRecords)
				{
					string base64 = item.ImageUrl.Replace("image/jpeg;base64,", "");
					Stream imageStream = Converters.GetImageStreamFromBase64(base64);
					item.ImageUrl = _image.UploadImage("OutOfContext", imageStream);
					Console.WriteLine($"[OOC SERVICE] Successfully processed {processedRecords} of {totalRecords}");
					processedRecords++;
				}
				context.SaveChanges();
			}
		}
	}
}
