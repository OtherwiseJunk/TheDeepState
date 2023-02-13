using DartsDiscordBots.Models;
using DartsDiscordBots.Services.Interfaces;
using DeepState.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Data.Services
{
    internal class BestOfService : IBestOfService
    {
        public IDbContextFactory<BestOfContext> dataContextFactory { get; set; }
        public BestOfService(IDbContextFactory<BestOfContext> contextFactory) { }

        public bool IsBestOf(ulong messageId)
        {
            return false;
        }

        public void CreateBestOf(BestOf bestOf)
        {
            Console.Write("Created BestOf");
        }
    }
}
