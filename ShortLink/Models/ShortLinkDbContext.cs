using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShortLink.Models
{
    public class ShortLinkDbContext : DbContext
    {
        public ShortLinkDbContext(DbContextOptions<ShortLinkDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var options = new DbContextOptionsBuilder<ShortLinkDbContext>()
                .EnableSensitiveDataLogging()
                .Options;
        }

        public DbSet<Url> Urls { get; set; }
    }
}
