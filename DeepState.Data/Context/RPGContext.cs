using DeepState.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
				.HasValue<Item>("non_consumable")
				.HasValue<ConsumableItem>("consumable")
				.HasValue<HealingPotion>("health_items");
		}
		public DbSet<Character> Characters { get; set; }
		public DbSet<RPGConfiguration> RPGConfigs { get; set; }
		public DbSet<Item> Items { get; set; }
	}
}
