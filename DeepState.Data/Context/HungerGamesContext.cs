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

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<HungerGamesTribute>()
				.Property(t => t.IsAlive)
				.HasDefaultValue(true);
			modelBuilder.Entity<HungerGamesTribute>()
				.Property(t => t.DeathMessage)
				.HasDefaultValue("");
			modelBuilder.Entity<HungerGamesTribute>()
				.Property(t => t.ObituaryMessage)
				.HasDefaultValue("");
		}
		public DbSet<HungerGamesTribute> Tributes { get; set; }
		public DbSet<HungerGamesPrizePool> PrizePools { get; set; }
		public DbSet<HungerGamesServerConfiguration> GuildConfigurations { get; set; }
	}
}
