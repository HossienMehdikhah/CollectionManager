using CollectionManager.WinUI.ViewModels;
using Microsoft.UI.Xaml.Controls;
namespace CollectionManager.WinUI.Views;

public sealed partial class ShellPage : Page
{
    public ShellPage(ShellViewModel shellViewModel)
    {
        ViewModel = shellViewModel;
        InitializeComponent();

        ViewModel.NavigationService.Frame = NavigationFrame;
        ViewModel.NavigationViewService.Initialize(NavigationViewControl);
    }

    public ShellViewModel ViewModel { get; }
}
