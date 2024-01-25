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
    public static void Pages(IServiceCollection services)
    {
        services.AddTransient<MainPage>();
    }
    public static void ViewModels(IServiceCollection services)
    {
        services.AddTransient<MainpageViewModel>();
    }
    public static void Managers(IServiceCollection services)
    {
        services.AddTransient<SiteManager>();
    }
    public static void Services(IServiceCollection services)
    {
        services.AddTransient<IGameSiteCrawler, Par30gamesSiteCrawler>();
    }
}
