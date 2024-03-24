using DeepState.Data.Models.RPGModels;
using Microsoft.EntityFrameworkCore;
using System;


namespace DeepState.Data.Context
{
	public class RPGContext : DbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("DATABASE"));
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<HealingItem>()
				.HasOne(i => i.Character)
				.WithMany(c => c.Items);
		}
		public DbSet<Character> Characters { get; set; }
		public DbSet<RPGConfiguration> RPGConfigs { get; set; }
		public DbSet<HealingItem> Items { get; set; }
	}
}
