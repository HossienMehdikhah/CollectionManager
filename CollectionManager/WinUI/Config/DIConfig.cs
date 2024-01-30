using CollectionManager.Core.Contracts.Services;
using CollectionManager.Core.Factories;
using CollectionManager.Core.Managers;
using CollectionManager.Core.Services;
using CollectionManager.WinUI.ViewModels;
using CollectionManager.WinUI.Views;
using Microsoft.Extensions.DependencyInjection;
using WinUI;
namespace CollectionManager.WinUI.Config;

internal class DIConfig
{
    public static IServiceCollection Config(IServiceCollection services)
    {
        Pages(services);
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
    }
    private static void ViewModels(IServiceCollection services)
    {
        services.AddTransient<MainpageViewModel>();
    }
    private static void Managers(IServiceCollection services)
    {
        services.AddTransient<SiteManager>();
    }
    private static void Services(IServiceCollection services)
    {
        services.AddTransient<IGameSiteCrawler, Par30gamesSiteCrawler>();
    }
}
