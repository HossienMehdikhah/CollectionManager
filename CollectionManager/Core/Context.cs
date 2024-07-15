using CollectionManager.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CollectionManager.Core;

public class Context : DbContext
{
    public Context(DbContextOptions options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GameSet>(x =>
        {
            x.Property(x => x.Id)
            .ValueGeneratedOnAdd();
            x.Property(x => x.Name)
            .IsRequired();
            x.Property(x => x.Uri)
            .IsRequired();
            x.Property(x => x.Thumbnail)
            .IsRequired();
            x.Property(x => x.AddedDate);
        });
    }

    public virtual DbSet<GameSet> Games { get; set; }
}
