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
			modelBuilder.Entity<Item>()
				.HasDiscriminator<string>("item_type")
				.HasValue<HealingItem>("health_items");
		}
		public DbSet<Character> Characters { get; set; }
		public DbSet<RPGConfiguration> RPGConfigs { get; set; }
		public DbSet<Item> Items { get; set; }
	}
}
