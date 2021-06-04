using DeepState.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace DeepState.Data.Context
{
	public class HungerGamesContext : DbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("DATABASE"));
		}

		public DbSet<HungerGamesTributes> Tributes { get; set; }
		public DbSet<HungerGamesPrizePool> PrizePools { get; set; }
	}
}
