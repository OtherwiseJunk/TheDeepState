using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeepState.Data.Context
{
	public class OOCDBContext : DbContext
	{
		public OOCDBContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<OOCItem> OutOfContextRecords { get; set; }
	}
}
