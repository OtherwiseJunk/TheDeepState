using DeepState.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Data.Context
{
	public class ModTeamRequestContext : DbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("DATABASE"));
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ModTeamRequest>()
				.Property(mtr => mtr.Status)
				.HasDefaultValue(RequestStatus.Opened);
			modelBuilder.Entity<ModTeamRequest>()
				.Property(mtr => mtr.Price)
				.HasDefaultValue(null);
			modelBuilder.Entity<ModTeamRequest>()
				.Property(mtr => mtr.CreationDatetime)
				.HasDefaultValue(DateTime.Now);
		}
		public DbSet<ModTeamRequest> Requests { get; set; }
	}
}
