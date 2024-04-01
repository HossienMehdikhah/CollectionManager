using CollectionManager.Core.Utilities;
using CollectionManager.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CollectionManager.Core.Contracts.Services;
using CollectionManager.Core.Services;
using Microsoft.Extensions.Logging;

namespace ConsoleApp;

public class Program
{
    public static void Main(string[] args)
    {
        IServiceCollection services = new ServiceCollection();
        services.AddLogging();
        var DbPath = "C:\\Users\\hiera\\AppData\\Local\\CollectionManager\\CollectionManager.db";
        services.AddDbContext<Context>(x => x.UseSqlite($"Data Source={DbPath};"));
        services.AddTransient<IGameSiteCrawler, Par30gamesSiteCrawler>();
        test(services.BuildServiceProvider()).Wait();
    }

    private static async Task test(IServiceProvider service)
    {
        var context = service.GetRequiredService<Context>();
        var badDataGames = context.Games.AsTracking().Where(x=>x.Thumbnail == null).ToList();
        var crawler = service.GetRequiredService<IGameSiteCrawler>();
        foreach (var badGame in badDataGames)
        {
            try
            {
                var gamePage = await crawler.GetPageAsync(badGame.Uri);
                badGame.Thumbnail = gamePage.Thumbnail;
                badGame.Name = gamePage.Name;
                context.Update(badGame);
                await context.SaveChangesAsync();
            }
            catch
            {
                continue;
            }
        }
    }
}
