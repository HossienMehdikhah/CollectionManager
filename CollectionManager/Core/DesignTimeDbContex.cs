using CollectionManager.Core.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
namespace CollectionManager.Core;

public class DesignTimeDbContext : IDesignTimeDbContextFactory<Context>
{
    public Context CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<Context>()
            .UseSqlite(PathHelper.GetSqliteDatabaseConnectionString);
        return new Context(optionsBuilder.Options);
    }
}