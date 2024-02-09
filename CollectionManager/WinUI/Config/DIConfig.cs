using CollectionManager.Core;
using CollectionManager.Core.Contracts.Services;
using CollectionManager.Core.Factories;
using CollectionManager.Core.Managers;
using CollectionManager.Core.Services;
using CollectionManager.Core.Utilities;
using CollectionManager.WinUI.UserControls;
using CollectionManager.WinUI.ViewModels;
using CollectionManager.WinUI.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WinUI;
namespace CollectionManager.WinUI.Config;

internal class DIConfig
{
    public static IServiceCollection Config(IServiceCollection services)
    {
        Pages(services);
        UserControls(services);
        ViewModels(services);
        Managers(services);
        Services(services);
        services.AddTransient<MainWindow>();
        services.AddTransient<AngleSharpFactory>();
        return services;
    }
    private static void Pages(IServiceCollection services)
    {
        services.AddTransient<MainPage>();
        services.AddTransient<SearchPage>();
    }
    private static void UserControls(IServiceCollection services)
    {
        services.AddTransient<ContentDisplayUserControl>();
    }
    private static void ViewModels(IServiceCollection services)
    {
        services.AddTransient<MainpageViewModel>();
        services.AddTransient<ContentDisplayViewModel>();
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<SearchPageViewModel>();
    }
    private static void Managers(IServiceCollection services)
    {
        services.AddTransient<SiteManager>();
    }
    private static void Services(IServiceCollection services)
    {
        services.AddTransient<IGameSiteCrawler, Par30gamesSiteCrawler>();
        services.AddDbContext<Context>(x=>x.UseSqlite(PathHelper.GetSqliteDatabaseConnectionString));
    }
}
