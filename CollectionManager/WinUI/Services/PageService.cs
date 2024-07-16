using CollectionManager.WinUI.Contracts;
using CollectionManager.WinUI.Pages;
using CollectionManager.WinUI.ViewModels;
using CollectionManager.WinUI.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
namespace CollectionManager.WinUI.Services;

public class PageService : IPageService
{
    private readonly Dictionary<string, Type> _pages = [];

    public PageService()
    {
        Configure<FeedPage, FeedPageViewModel>();
        Configure<SearchPage, SearchPageViewModel>();
        Configure<MarketGameListedPage, MarketGameListedPageViewModel>();
        Configure<DisplayGamePage, DisplayGameViewModel>();
    }
    public Type GetPageType(string key)
    {
        lock (_pages)
        {            
            if (!_pages.Any(x=>x.Key.Equals(key)))
            {
                throw new ArgumentException($"Page not found: {key}. Did you forget to call PageService.Configure?");
            }
        }
        return Type.GetType(key)!;
    }
    private void Configure<V, VM>()
        where V : Page
        where VM : ObservableObject
    {
        lock (_pages)
        {
            var key = typeof(V).FullName!;
            if (_pages.ContainsKey(key))
            {
                throw new ArgumentException($"The key {key} is already configured in PageService");
            }

            var type = typeof(VM);
            if (_pages.ContainsValue(type))
            {
                throw new ArgumentException($"This type is already configured with key {_pages.First(p => p.Value == type).Key}");
            }

            _pages.Add(key, type);
        }
    }
}
