using DeepState.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace DeepState.Data.Context
{
    public class ToDoContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("DATABASE"));
        }
        public DbSet<ToDoItem> ToDoItems { get; set; }
    }
}
