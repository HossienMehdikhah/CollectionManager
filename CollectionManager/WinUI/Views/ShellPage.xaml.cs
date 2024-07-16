using CollectionManager.WinUI.ViewModels;
using Microsoft.UI.Xaml.Controls;
using WinUI;
namespace CollectionManager.WinUI.Views;

public sealed partial class ShellPage : Page
{
    public ShellPage()
    {
        DataContext = App.GetService<ShellViewModel>();
        InitializeComponent();
        ViewModel.NavigationService.Frame = NavigationFrame;
        ViewModel.NavigationViewService.Initialize(NavigationViewControl);
    }

    public ShellViewModel ViewModel { get => (ShellViewModel)DataContext; }
}
