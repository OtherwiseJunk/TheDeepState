using DeepState.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace DeepState.Data.Context
{
	public class HungerGamesContext : DbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("DATABASE"));
		}
		public DbSet<HungerGamesTribute> Tributes { get; set; }
		public DbSet<HungerGamesPrizePool> PrizePools { get; set; }
		public DbSet<HungerGamesServerConfiguration> GuildConfigurations { get; set; }
	}
}
