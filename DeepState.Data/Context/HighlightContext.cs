using DeepState.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace DeepState.Data.Context
{
    public class HighlightContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("DATABASE"));
        }
        public DbSet<Highlight> Highlights { get; set; }
    }
}
