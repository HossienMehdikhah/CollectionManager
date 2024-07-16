using CollectionManager.WinUI.ViewModels;
using Microsoft.UI.Xaml.Controls;
using WinUI;
namespace CollectionManager.WinUI.Pages;

public sealed partial class FeedPage : Page
{
    public FeedPage()
    {
        DataContext = App.GetService<FeedPageViewModel>();
        InitializeComponent();
    }

    public FeedPageViewModel ViewModel { get => (FeedPageViewModel)DataContext; }
}
