using DeepState.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeepState.Data.Context
{
	public class GuildUserRecordContext : DbContext
	{
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("DATABASE"));
		}

		public DbSet<UserRecord> UserRecords { get; set; }

		
	}
}
