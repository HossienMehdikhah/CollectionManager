using Microsoft.UI.Xaml.Controls;
using CollectionManager.WinUI.ViewModels;
using WinUI;

namespace CollectionManager.WinUI.Views;

public sealed partial class SearchPage : Page
{
    public SearchPage()
    {
        DataContext = App.GetService<SearchPageViewModel>();
        this.InitializeComponent();
    }
    private SearchPageViewModel ViewModel { get => (SearchPageViewModel)DataContext; }
}
