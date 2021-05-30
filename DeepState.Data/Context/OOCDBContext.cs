using Discord;
using Microsoft.EntityFrameworkCore;
using System;
using DartsDiscordBots.Utilities;
using DeepState.Data.Models;

namespace DeepState.Data.Context
{
	public class OOCDBContext : DbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("DATABASE"));
		}

		public DbSet<OOCItem> OutOfContextRecords { get; set; }
	}
}
