using CollectionManager.Core;
using CollectionManager.Core.Contracts.Services;
using CollectionManager.Core.Factories;
using CollectionManager.Core.Managers;
using CollectionManager.Core.Models;
using CollectionManager.Core.Services;
using CollectionManager.Core.Utilities;
using CollectionManager.WinUI.Activations;
using CollectionManager.WinUI.Contracts;
using CollectionManager.WinUI.Services;
using CollectionManager.WinUI.Singleton;
using CollectionManager.WinUI.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using WinUI;
using Microsoft.Extensions.Configuration;
namespace CollectionManager.WinUI.Config;

public class DIConfig
{
    public static IServiceCollection Config(IServiceCollection services, ConfigurationManager configuration)
    {
        services.Configure<CollectionManagerOption>(configuration.GetRequiredSection(nameof(CollectionManagerOption)));
        ViewModels(services);
        Managers(services);
        Services(services);
        services.AddTransient<MainWindow>();
        services.AddTransient<AngleSharpFactory>();
        services.AddSingleton<FeedPageViewModelSingleton>();
        services.AddSingleton<SearchPageViewModelSingleton>();
        services.AddSingleton<DisplayGamePageViewModelSingleton>();
        return services;
    }       
    private static void ViewModels(IServiceCollection services)
    {
        services.AddTransient<FeedPageViewModel>();
        services.AddTransient<ContentDisplayUserControlViewModel>();
        services.AddTransient<ShellViewModel>();
        services.AddTransient<SearchPageViewModel>();
        services.AddTransient<GamelistedUserControlViewModel>();
        services.AddTransient<DisplayGameViewModel>();
        services.AddTransient<MarketGameListedPageViewModel>();
        services.AddTransient<LoadingUserControlViewModel>();
    }
    private static void Managers(IServiceCollection services)
    {
        services.AddTransient<SiteManager>();
    }
    private static void Services(IServiceCollection services)
    {
        services.AddTransient<IGameSiteCrawler, Par30gamesSiteCrawler>();
        services.AddDbContext<Context>(x=>x.UseSqlite(PathHelper.GetSqliteDatabaseConnectionString));
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<INavigationViewService, NavigationViewService>();
        services.AddSingleton<IActivationService, ActivationService>();
        services.AddSingleton<IActivationHandler, DefaultActivationHandler>();
        services.AddSingleton<IPageService, PageService>();
        services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();
    }
}
