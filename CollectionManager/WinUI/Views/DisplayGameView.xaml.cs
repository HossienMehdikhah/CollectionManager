using CollectionManager.WinUI.ViewModels;
using Microsoft.UI.Xaml.Controls;
using WinUI;
namespace CollectionManager.WinUI.Views;

public sealed partial class DisplayGameView : Page
{
    public DisplayGameView()
    {
        DataContext = App.GetService<DisplayGameViewModel>();
        InitializeComponent();
    }

    public DisplayGameViewModel ViewModel { get => (DisplayGameViewModel)DataContext; }
}
