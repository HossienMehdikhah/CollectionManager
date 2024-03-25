using CollectionManager.Core.Models;
using CollectionManager.WinUI.Config;
using CollectionManager.WinUI.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using WinUIEx;
namespace WinUI;
public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        var builder = Microsoft.Extensions.Hosting.Host.CreateApplicationBuilder();
        builder.Configuration.AddJsonFile("appsettings.json");
        builder.Logging.ClearProviders();
        builder.Logging.AddDebug();
        DIConfig.Config(builder.Services);
        builder.Services.Configure<CollectionManagerOption>(builder.Configuration.GetRequiredSection(nameof(CollectionManagerOption)));
        Host = builder.Build();
        UnhandledException += App_UnhandledException;
    }

    public IHost Host
    {
        get;
    }
    public static WindowEx MainWindow { get; } = new MainWindow();
    public static T GetService<T>() where T : class
    {
        if ((Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        return service;
    }
    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        
    }
    protected override async void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);
        await GetService<IActivationService>().ActivateAsync(args);
    }
}