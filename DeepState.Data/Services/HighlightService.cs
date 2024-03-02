using DeepState.Data.Context;
using DeepState.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeepState.Data.Services
{
    public class HighlightService
    {
        private IDbContextFactory<HighlightContext> _dbContextFactory { get; }
        public HighlightService(IDbContextFactory<HighlightContext> dbContextFactory) {
            _dbContextFactory = dbContextFactory;
        }

        public void CreateHighlight(ulong userId, string triggerPhrase)
        {
            using (HighlightContext db = _dbContextFactory.CreateDbContext())
            {
                db.Highlights.Add(new Highlight(userId, triggerPhrase.ToLower()));
                db.SaveChanges();
            }
        }

        public List<Highlight> GetHighlights()
        {
            using (HighlightContext db = _dbContextFactory.CreateDbContext())
            {
                return db.Highlights.ToList();
            }
        }

        public List<Highlight> GetHighlightsForUser(ulong userId)
        {
            using (HighlightContext db = _dbContextFactory.CreateDbContext())
            {
                return db.Highlights.Where(highlight =>  highlight.UserId == userId).ToList();
            }
        }

        public void DeleteHighlight(Highlight highlight)
        {
            using (HighlightContext db = _dbContextFactory.CreateDbContext())
            {
                db.Highlights.Attach(highlight);
                db.Highlights.Remove(highlight);
                db.SaveChanges();
            }
        }
    }
}
