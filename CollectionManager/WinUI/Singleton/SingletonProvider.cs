using Microsoft.Extensions.DependencyInjection;
namespace CollectionManager.WinUI.Singleton;

public class SingletonProvider
{
    private SingletonProvider()
    {
        SiteManagerSingleton = new SiteManagerSingleton();
    }
    public static void Build(IServiceCollection services)
    {
        services.AddSingleton(new SingletonProvider());
    }

    public SiteManagerSingleton SiteManagerSingleton { get; set; }
}
