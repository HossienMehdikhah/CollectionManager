using CollectionManagerWebService.Models;
using Microsoft.EntityFrameworkCore;

namespace CollectionManagerWebService
{
    public class Context : DbContext
    {
        public Context() { }
        public Context(DbContextOptions<Context> options) : base(options) 
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<Game> Links { get; set; }
    }
}
