using CollectionManager.WinUI.ViewModels;
using Microsoft.UI.Xaml.Controls;
using WinUI;
namespace CollectionManager.WinUI.UserControls;

public sealed partial class GamelistedViewUserControl : UserControl
{
    public GamelistedViewUserControlViewModel ViewModel { get => (GamelistedViewUserControlViewModel)DataContext; }
    public GamelistedViewUserControl()
    {
        DataContext = App.GetService<GamelistedViewUserControlViewModel>();
    }
}
