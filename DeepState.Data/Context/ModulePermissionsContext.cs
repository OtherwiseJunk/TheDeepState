using Microsoft.EntityFrameworkCore;
using System;
using DeepState.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Data.Context
{
    public class ModulePermissionsContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("DATABASE"));
        }
        public DbSet<GuildModulePermissions> GuildModulePermissions { get; set; }
        public DbSet<ChannelModulePermissions> ChannelModulePermissions { get; set; }
    }
}
