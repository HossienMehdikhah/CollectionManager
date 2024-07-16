using CollectionManager.WinUI.ViewModels;
using Microsoft.UI.Xaml.Controls;
using WinUI;
namespace CollectionManager.WinUI.Views;

public sealed partial class DisplayGamePage : Page
{
    public DisplayGamePage()
    {
        DataContext = App.GetService<DisplayGameViewModel>();
        InitializeComponent();
    }

    public DisplayGameViewModel ViewModel { get => (DisplayGameViewModel)DataContext; }
}
