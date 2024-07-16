using CollectionManager.WinUI.ViewModels;
using Microsoft.UI.Xaml.Controls;
using WinUI;
namespace CollectionManager.WinUI.Pages;

public sealed partial class MarketGameListedPage : Page
{
    public MarketGameListedPageViewModel ViewModel { get => (MarketGameListedPageViewModel)DataContext; }
    public MarketGameListedPage()
    {
        DataContext = App.GetService<MarketGameListedPageViewModel>();
        InitializeComponent();
    }
}
