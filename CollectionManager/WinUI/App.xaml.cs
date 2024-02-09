using CollectionManager.Core.Models;
using CollectionManager.WinUI.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUI
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost host;
        private Window _mainwindow;

        public App()
        {
            var builder = Host.CreateApplicationBuilder();
            builder.Configuration.AddJsonFile("appsettings.json");
            DIConfig.Config(builder.Services);
            builder.Services.Configure<CollectionManagerOption>(builder.Configuration.GetRequiredSection(nameof(CollectionManagerOption)));
            host = builder.Build();
            this.InitializeComponent();
        }

        public static T GetServices<T>() where T : class => ((App)Current).host.Services.GetRequiredService<T>();

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            _mainwindow = GetServices<MainWindow>();
            _mainwindow.Activate();
        }
    }
}
