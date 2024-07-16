using CollectionManager.WinUI.ViewModels;
using Microsoft.UI.Xaml.Controls;
using WinUI;
namespace CollectionManager.WinUI.UserControls;

public sealed partial class GamelistedViewUserControl : UserControl
{
    public GamelistedViewUserControl()
    {
        DataContext = App.GetService<GamelistedUserControlViewModel>();
        InitializeComponent();        
    }

    public GamelistedUserControlViewModel ViewModel { get => (GamelistedUserControlViewModel)DataContext; }
}
