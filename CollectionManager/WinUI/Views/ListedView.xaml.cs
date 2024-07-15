using CollectionManager.Core.Managers;
using CollectionManager.Core.Models;
using CollectionManager.WinUI.Utilities;
using CollectionManager.WinUI.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using WinUI;
namespace CollectionManager.WinUI.Views;


public sealed partial class ListedView : Page
{
    public ListedViewModel ViewModel { get; set; }
    public ListedView()
    {
        ViewModel=App.GetService<ListedViewModel>();
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        if (Enum.TryParse<MarkedType>((string)e.Parameter, out var markettype))
        {
            var databseCollection = new IncrementalSourceFromDbByMarktype(App.GetService<SiteManager>(), markettype);
            mainListView.ViewModel.GamePages = new(databseCollection);
        }
        else
        {
            var searchResultCollection = new IncrementalSourceFromWebSite(App.GetService<SiteManager>(), (string)e.Parameter);
            mainListView.ViewModel.GamePages = new(searchResultCollection);
        }

        mainListView.InitializeComponent();
    }

    
}
