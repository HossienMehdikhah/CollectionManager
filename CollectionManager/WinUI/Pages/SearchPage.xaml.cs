using Microsoft.UI.Xaml.Controls;
using CollectionManager.WinUI.ViewModels;
using WinUI;
namespace CollectionManager.WinUI.Pages;

public sealed partial class SearchPage : Page
{
    public SearchPage()
    {
        DataContext = App.GetService<SearchPageViewModel>();
        InitializeComponent();
    }

    public SearchPageViewModel ViewModel { get => (SearchPageViewModel)DataContext; }
}
