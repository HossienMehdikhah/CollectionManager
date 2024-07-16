﻿using CollectionManager.Core;
using CollectionManager.Core.Contracts.Services;
using CollectionManager.Core.Factories;
using CollectionManager.Core.Managers;
using CollectionManager.Core.Services;
using CollectionManager.Core.Utilities;
using CollectionManager.WinUI.Activations;
using CollectionManager.WinUI.Contracts;
using CollectionManager.WinUI.Pages;
using CollectionManager.WinUI.Services;
using CollectionManager.WinUI.Singleton;
using CollectionManager.WinUI.UserControls;
using CollectionManager.WinUI.ViewModels;
using CollectionManager.WinUI.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using WinUI;
namespace CollectionManager.WinUI.Config;

public class DIConfig
{
    public static IServiceCollection Config(IServiceCollection services)
    {
        ViewModels(services);
        Managers(services);
        Services(services);
        services.AddTransient<MainWindow>();
        services.AddTransient<AngleSharpFactory>();
        services.AddSingleton<FeedPageViewModelSingleton>();
        services.AddSingleton<SearchPageViewModelSingleton>();
        return services;
    }       
    private static void ViewModels(IServiceCollection services)
    {
        services.AddTransient<FeedPageViewModel>();
        services.AddTransient<ContentDisplayViewModel>();
        services.AddTransient<ShellViewModel>();
        services.AddTransient<SearchPageViewModel>();
        services.AddTransient<GamelistedViewUserControlViewModel>();
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
