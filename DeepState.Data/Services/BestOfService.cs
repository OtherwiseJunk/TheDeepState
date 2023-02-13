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
    public class BestOfService : IBestOfService
    {
        public IDbContextFactory<BestOfContext> dataContextFactory { get; set; }
        public BestOfService(IDbContextFactory<BestOfContext> contextFactory) { }

        public bool IsBestOf(ulong messageId)
        {
            using(BestOfContext context = dataContextFactory.CreateDbContext())
            {
                return context.BestOfs.FirstOrDefault(x => x.MessageId == messageId) != null;
            }
        }

        public void CreateBestOf(BestOf bestOf)
        {
            using (BestOfContext context = dataContextFactory.CreateDbContext())
            {
                context.BestOfs.Add(bestOf);
                context.SaveChanges();
            }
        }
    }
}
