using Microsoft.EntityFrameworkCore;
using DeepState.Data.Context;
using System;
using DeepState.Data.Models;
using DartsDiscordBots.Utilities;
using System.IO;
using System.Linq;
using DartsDiscordBots.Services;

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
				return context.OutOfContextRecords.AsQueryable().FirstOrDefaultAsync(oocr => oocr.ImageUrl == base64Image).Result != null;
			}

		}

		
	}
}
