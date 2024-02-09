using CollectionManager.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CollectionManager.Core;

public class Context : DbContext
{
    public Context(DbContextOptions options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GameSet>(x =>
        {
            x.Property(x => x.Id)
            .ValueGeneratedOnAdd();
        });
    }

    public virtual DbSet<GameSet> Games { get; set; }
}
